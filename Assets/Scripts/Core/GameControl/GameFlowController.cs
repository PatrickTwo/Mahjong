using System;
using System.Collections.Generic;
using Mahjong.GameControl.States;
using UnityEngine;

namespace Mahjong
{
    /// <summary>
    /// 游戏流程控制器
    /// </summary>
    public class GameFlowController
    {
        private readonly MahjongGameManager game;
        /// <summary>
        /// 游戏状态字典
        /// </summary>
        private readonly Dictionary<GameState, IGameState> states;
        private IGameState currentState;
        public GameState CurrentState => currentState.StateType;

        public MahjongGameManager Game => game;
        #region 构造函数
        public GameFlowController(MahjongGameManager game)
        {
            if (game == null)
                throw new ArgumentNullException(nameof(game));
            this.game = game;
            states = new Dictionary<GameState, IGameState>
            {
                { GameState.LobbyWaiting, new LobbyWaitingState(this) },
                { GameState.Dealing, new DealingState(this) },
                { GameState.Playing, new PlayingState(this) },
                { GameState.TingDeclared, new TingDeclaredState(this) },
                { GameState.Win, new WinState(this) },
                { GameState.Draw, new DrawState(this) },
                { GameState.Ended, new EndedState(this) }
            };
        }
        #endregion
        #region 流程控制
        /// <summary>
        /// 初始化游戏流程
        /// </summary>
        public void InitializeGame()
        {
            currentState = states[GameState.LobbyWaiting];
            currentState.Enter();
        }
        public void UpdateState()
        {
            currentState?.Update();
        }
        public void TransitionToState(GameState newState)
        {
            if (states.TryGetValue(newState, out IGameState nextState))
            {
                if (currentState?.CanTransitionTo(newState) == false)
                {
                    Debug.LogError($"无法从 {currentState.StateType} 转换到 {newState}");
                }

                currentState?.Exit();
                currentState = nextState;
                currentState.Enter();

                game.TriggerStateChanged(newState);
            }
            else
            {
                Debug.LogError($"未知的游戏状态: {newState}");
            }
        }
        #endregion
        public void ProcessPlayerAction(Player player, PlayerAction action, MahjongTile tile = null)
        {
            currentState?.Update();

            // 将操作传递给当前状态处理
            if (currentState is IPlayerActionHandler actionHandler)
            {
                actionHandler.HandlePlayerAction(player, action, tile);
            }
        }
    }
}
