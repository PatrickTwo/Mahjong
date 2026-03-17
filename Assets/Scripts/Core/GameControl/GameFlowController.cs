using System;
using System.Collections.Generic;
using Mahjong;
using UnityEngine;

namespace Mahjong
{
    /// <summary>
    /// 游戏流程控制器
    /// 只负责游戏状态的转换和管理，并将状态同步到游戏管理器
    /// </summary>
    public class GameFlowController
    {
        private readonly MahjongGameManager gameManager;
        /// <summary>
        /// 游戏状态字典
        /// </summary>
        private readonly Dictionary<GameState, IGameState> states;
        private IGameState currentState;
        public GameState CurrentState => currentState.StateType;

        #region 构造函数
        public GameFlowController(MahjongGameManager game)
        {
            if (game == null)
                throw new ArgumentNullException(nameof(game));
            this.gameManager = game;
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
        public void InitializeGame()
        {
            currentState = states[GameState.LobbyWaiting];
            currentState.Enter();
        }
        /// <summary>
        /// 执行单签游戏状态的更新
        /// </summary>
        public void UpdateState()
        {
            currentState?.Update();
        }
        /// <summary>
        /// 转换到新的游戏状态
        /// </summary>
        /// <param name="newState">新的游戏状态</param>
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

                gameManager.TriggerStateChanged(newState);
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
