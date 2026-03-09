using System;
using System.Collections.Generic;
using Mahjong.GameControl.States;



namespace Mahjong
{
    #region 游戏流程控制器（模块化状态机）
    /// <summary>
    /// 游戏流程控制器 - 模块化状态机实现
    /// </summary>
    public class GameFlowController
    {
        private MahjongGameManager game;
        private Dictionary<GameState, IGameState> states;
        private IGameState currentState;

        public MahjongGameManager Game => game;

        public GameFlowController(MahjongGameManager game)
        {
            this.game = game ?? throw new ArgumentNullException(nameof(game));
            InitializeStates();
        }

        private void InitializeStates()
        {
            states = new Dictionary<GameState, IGameState>
            {
                { GameState.Initializing, new InitializingState(this) },
                { GameState.Dealing, new DealingState(this) },
                { GameState.Playing, new PlayingState(this) },
                { GameState.TingDeclared, new TingDeclaredState(this) },
                { GameState.Win, new WinState(this) },
                { GameState.Draw, new DrawState(this) },
                { GameState.Ended, new EndedState(this) }
            };
        }

        public void StartGame()
        {
            TransitionToState(GameState.Initializing);
        }

        public void ProcessPlayerAction(Player player, PlayerAction action, MahjongTile tile = null)
        {
            currentState?.Update();

            // 将操作传递给当前状态处理
            if (currentState is IPlayerActionHandler actionHandler)
            {
                actionHandler.HandlePlayerAction(player, action, tile);
            }
        }

        public void TransitionToState(GameState newState)
        {
            if (states.TryGetValue(newState, out IGameState nextState))
            {
                if (currentState?.CanTransitionTo(newState) == false)
                {
                    throw new InvalidOperationException($"无法从 {currentState.StateType} 转换到 {newState}");
                }

                currentState?.Exit();
                currentState = nextState;
                currentState.Enter();

                game.TriggerStateChanged(newState);
            }
            else
            {
                throw new ArgumentException($"未知的游戏状态: {newState}");
            }
        }

        public GameState CurrentState => currentState?.StateType ?? GameState.Initializing;

        public void Update()
        {
            currentState?.Update();
        }
    }
    #endregion
}
