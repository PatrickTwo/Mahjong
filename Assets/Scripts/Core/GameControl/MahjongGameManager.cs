using System;
using UnityEngine;

namespace Mahjong
{
    #region 麻将游戏主控制器
    /// <summary>
    /// 麻将游戏主控制器
    /// </summary>
    public class MahjongGameManager : MonoBehaviour
    {
        private TilePool tilePool;
        private PlayerManager playerManager;
        private GameFlowController flowController;
        private WinChecker winChecker;
        private ScoreCalculator scoreCalculator;

        public GameState CurrentState { get; private set; }
        public int CurrentPlayerIndex { get; private set; }

        public event Action<GameState> OnStateChanged;
        public event Action<Player, PlayerAction> OnPlayerAction;

        public MahjongGameManager()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            tilePool = new TilePool();
            playerManager = new PlayerManager();
            flowController = new GameFlowController(this);
            winChecker = new WinChecker();
            scoreCalculator = new ScoreCalculator();
        }

        public void StartNewGame()
        {
            tilePool.Initialize();
            playerManager.InitializePlayers();
            flowController.StartGame();
        }

        /// <summary>
        /// 触发状态变化事件（只能在MahjongGame类内部调用）
        /// </summary>
        /// <param name="newState">新的游戏状态</param>
        public void TriggerStateChanged(GameState newState)
        {
            CurrentState = newState;
            OnStateChanged?.Invoke(newState);
        }
    }
    #endregion
}
