using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mahjong
{
    /// <summary>
    /// 游戏流程控制器。
    /// 只负责状态切换与流程推进，不承载 UI 展示逻辑。
    /// </summary>
    public class GameFlowController
    {
        #region 字段与属性

        private readonly MahjongGameManager gameManager;
        private readonly Dictionary<GameState, IGameState> states;
        private IGameState currentState;

        /// <summary>
        /// 当前游戏状态。
        /// </summary>
        public GameState CurrentState => currentState.StateType;

        private readonly IEventBusService eventBusService = EventBusService.Instance;

        #endregion

        #region 构造函数

        public GameFlowController(MahjongGameManager game)
        {
            if (game == null)
            {
                throw new ArgumentNullException(nameof(game));
            }

            gameManager = game;
            states = new Dictionary<GameState, IGameState>
            {
                { GameState.LobbyWaiting, new LobbyWaitingState(this, eventBusService) },
                { GameState.BankerSelection, new BankerSelectionState(this, eventBusService) },
                { GameState.Dealing, new DealingState(this, eventBusService) },
                { GameState.Playing, new PlayingState(this, eventBusService) },
                { GameState.TingDeclared, new TingDeclaredState(this, eventBusService) },
                { GameState.Win, new WinState(this, eventBusService) },
                { GameState.Draw, new DrawState(this, eventBusService) },
                { GameState.Ended, new EndedState(this, eventBusService) }
            };
        }

        #endregion

        #region 流程控制

        /// <summary>
        /// 初始化游戏状态机。
        /// </summary>
        public void InitializeGame()
        {
            currentState = states[GameState.LobbyWaiting];
            currentState.Enter();
            gameManager.TriggerStateChanged(currentState.StateType);
        }

        /// <summary>
        /// 更新当前状态。
        /// </summary>
        public void UpdateState()
        {
            currentState?.Update();
        }

        /// <summary>
        /// 切换到新的游戏状态。
        /// </summary>
        /// <param name="newState">目标状态。</param>
        public void TransitionToState(GameState newState)
        {
            if (!states.TryGetValue(newState, out IGameState nextState))
            {
                Debug.LogError($"未知的游戏状态：{newState}");
                return;
            }

            if (currentState != null && !currentState.CanTransitionTo(newState))
            {
                Debug.LogError($"无法从 {currentState.StateType} 切换到 {newState}");
                return;
            }

            currentState?.Exit();
            currentState = nextState;
            currentState.Enter();
            gameManager.TriggerStateChanged(newState);
        }

        /// <summary>
        /// 处理玩家动作。
        /// </summary>
        /// <param name="player">发起动作的玩家。</param>
        /// <param name="action">动作类型。</param>
        /// <param name="tile">相关牌张。</param>
        public void ProcessPlayerAction(Player player, PlayerAction action, MahjongTile tile = null)
        {
            currentState?.Update();

            if (currentState is IPlayerActionHandler actionHandler)
            {
                actionHandler.HandlePlayerAction(player, action, tile);
            }
        }

        #endregion
    }
}
