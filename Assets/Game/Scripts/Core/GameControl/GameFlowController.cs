using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mahjong
{
    /// <summary>
    /// 游戏流程控制器。
    /// 负责集中维护状态注册、合法跳转关系以及启动期配置校验。
    /// </summary>
    public class GameFlowController
    {
        /// <summary>
        /// 单个状态的中心化定义。
        /// 包含状态实例工厂、允许跳转的目标状态，以及可选的动态守卫条件。
        /// </summary>
        private sealed class StateDefinition // sealed 限制类的继承或方法的重写
        {
            /// <summary>
            /// 用于创建状态实例的工厂方法。
            /// </summary>
            public Func<GameFlowController, IGameState> Factory { get; }

            /// <summary>
            /// 当前状态允许跳转到的目标状态集合。
            /// </summary>
            public HashSet<GameState> AllowedTransitions { get; }

            /// <summary>
            /// 运行时动态校验。
            /// 用于处理“目标状态合法，但还需要额外业务条件成立”这类场景。
            /// </summary>
            public Func<IGameState, GameState, bool> TransitionGuard { get; }
            
            public StateDefinition(
                Func<GameFlowController, IGameState> factory,
                IEnumerable<GameState> allowedTransitions,
                Func<IGameState, GameState, bool> transitionGuard = null)
            {
                Factory = factory ?? throw new ArgumentNullException(nameof(factory));
                AllowedTransitions = new HashSet<GameState>(allowedTransitions ?? Array.Empty<GameState>());
                TransitionGuard = transitionGuard;
            }
        }

        private readonly MahjongGameManager gameManager;
        /// 状态定义表。
        /// 是“状态枚举 + 实例工厂 + 合法跳转关系”的唯一维护入口。
        private readonly Dictionary<GameState, StateDefinition> stateDefinitions;
        /// 已创建的状态实例表。
        private readonly Dictionary<GameState, IGameState> states;
        private IGameState currentState; // 当前激活的游戏状态。
        public GameState CurrentState => currentState.StateType; // 当前游戏状态枚举。

        /// <summary>
        /// 构造游戏流程控制器，并在启动时完成状态配置构建与校验。
        /// </summary>
        /// <param name="game">游戏管理器。</param>
        public GameFlowController(MahjongGameManager game)
        {
            gameManager = game ?? throw new ArgumentNullException(nameof(game));
            stateDefinitions = BuildStateDefinitions();
            states = BuildStates();
            ValidateStateConfiguration();
        }

        /// <summary>
        /// 初始化游戏流程，进入大厅等待状态。
        /// </summary>
        public void InitializeGame()
        {
            currentState = states[GameState.LobbyWaiting];
            currentState.Enter();
        }

        /// <summary>
        /// 更新当前状态。
        /// 通常在游戏主循环中调用。
        /// </summary>
        public void UpdateState()
        {
            currentState?.Update();
        }

        /// <summary>
        /// 尝试切换到指定游戏状态。
        /// 所有状态切换都必须通过该统一入口执行。
        /// </summary>
        /// <param name="newState">目标状态。</param>
        public void TransitionToState(GameState newState)
        {
            if (!states.TryGetValue(newState, out IGameState nextState))
            {
                // 目标状态未注册，直接中断。
                Debug.LogError($"Unknown game state: {newState}");
                return;
            }

            if (currentState?.CanTransitionTo(newState) == false)
            {
                // 非法状态迁移直接中断，避免状态机约束失效。
                Debug.LogError($"Invalid transition: {currentState.StateType} -> {newState}");
                return;
            }

            currentState?.Exit();
            currentState = nextState;
            currentState.Enter();

            gameManager.TriggerStateChanged(newState);
        }

        /// <summary>
        /// 将玩家操作转发给当前状态处理。
        /// 仅支持实现了 <see cref="IPlayerActionHandler"/> 的状态。
        /// </summary>
        /// <param name="player">发起操作的玩家。</param>
        /// <param name="action">玩家操作类型。</param>
        /// <param name="tile">关联牌张，可为空。</param>
        public void ProcessPlayerAction(Player player, PlayerAction action, MahjongTile tile = null)
        {
            currentState?.Update();

            if (currentState is IPlayerActionHandler actionHandler)
            {
                actionHandler.HandlePlayerAction(player, action, tile);
            }
        }

        /// <summary>
        /// 由状态基类调用的统一跳转校验入口。
        /// 先校验目标状态是否在允许列表中，再执行可选的动态守卫。
        /// </summary>
        /// <param name="fromState">当前状态枚举。</param>
        /// <param name="stateInstance">当前状态实例。</param>
        /// <param name="toState">目标状态枚举。</param>
        /// <returns>是否允许迁移。</returns>
        public bool CanTransition(GameState fromState, IGameState stateInstance, GameState toState)
        {
            if (!stateDefinitions.TryGetValue(fromState, out StateDefinition definition))
            {
                Debug.LogError($"Missing state definition for {fromState}");
                return false;
            }

            if (!definition.AllowedTransitions.Contains(toState))
            {
                // 不在静态允许表中，直接判定为非法迁移。
                return false;
            }

            return definition.TransitionGuard?.Invoke(stateInstance, toState) ?? true;
        }

        /// <summary>
        /// 构建状态中心配置表。
        /// 后续新增状态时，优先只改这里。
        /// </summary>
        /// <returns>状态定义字典。</returns>
        private Dictionary<GameState, StateDefinition> BuildStateDefinitions()
        {
            return new Dictionary<GameState, StateDefinition>
            {
                {
                    GameState.LobbyWaiting,
                    new StateDefinition(
                        controller => new LobbyWaitingState(controller),
                        new[] { GameState.BankerSelection })
                },
                {
                    GameState.BankerSelection,
                    new StateDefinition(
                        controller => new BankerSelectionState(controller),
                        new[] { GameState.Dealing },
                        (state, _) => state is BankerSelectionState bankerSelectionState &&
                                      bankerSelectionState.IsSelectionComplete)
                },
                {
                    GameState.Dealing,
                    new StateDefinition(
                        controller => new DealingState(controller),
                        new[] { GameState.Playing })
                },
                {
                    GameState.Playing,
                    new StateDefinition(
                        controller => new PlayingState(controller),
                        new[] { GameState.TingDeclared, GameState.Win, GameState.Draw, GameState.Ended })
                },
                {
                    GameState.TingDeclared,
                    new StateDefinition(
                        controller => new TingDeclaredState(controller),
                        new[] { GameState.Win, GameState.Draw, GameState.Ended })
                },
                {
                    GameState.Win,
                    new StateDefinition(
                        controller => new WinState(controller),
                        new[] { GameState.Ended })
                },
                {
                    GameState.Draw,
                    new StateDefinition(
                        controller => new DrawState(controller),
                        new[] { GameState.Ended })
                },
                {
                    GameState.Ended,
                    new StateDefinition(
                        controller => new EndedState(controller),
                        new[] { GameState.LobbyWaiting })
                }
            };
        }

        /// <summary>
        /// 根据状态定义表创建所有状态实例。
        /// </summary>
        /// <returns>状态实例字典。</returns>
        private Dictionary<GameState, IGameState> BuildStates()
        {
            Dictionary<GameState, IGameState> result = new Dictionary<GameState, IGameState>(stateDefinitions.Count);

            foreach (KeyValuePair<GameState, StateDefinition> pair in stateDefinitions)
            {
                result[pair.Key] = pair.Value.Factory(this);
            }

            return result;
        }

        /// <summary>
        /// 启动期校验状态配置完整性。
        /// 确保：
        /// 1. 每个 GameState 都有定义
        /// 2. 每个定义都能创建对应实例
        /// 3. 状态字典键与实例声明的 StateType 一致
        /// 4. 所有目标状态都已定义
        /// </summary>
        private void ValidateStateConfiguration()
        {
            foreach (GameState state in Enum.GetValues(typeof(GameState)))
            {
                if (!stateDefinitions.ContainsKey(state))
                {
                    // 枚举已存在，但中心配置未维护，直接在启动期暴露问题。
                    throw new InvalidOperationException($"GameState {state} is missing a definition.");
                }

                if (!states.TryGetValue(state, out IGameState stateInstance))
                {
                    // 状态定义存在，但实例未成功创建。
                    throw new InvalidOperationException($"GameState {state} is missing a state instance.");
                }

                if (stateInstance.StateType != state)
                {
                    // 字典键和状态实例自我声明不一致，说明配置或实现有错误。
                    throw new InvalidOperationException(
                        $"State mismatch: key={state}, instance={stateInstance.StateType}");
                }
            }

            foreach (KeyValuePair<GameState, StateDefinition> pair in stateDefinitions)
            {
                foreach (GameState target in pair.Value.AllowedTransitions)
                {
                    if (!stateDefinitions.ContainsKey(target))
                    {
                        // 跳转目标未定义，会导致运行时主链路断裂。
                        throw new InvalidOperationException(
                            $"State {pair.Key} points to undefined target {target}.");
                    }
                }
            }
        }
    }
}
