using System;
using UnityEngine;

namespace Mahjong
{
    #region 麻将游戏主控制器
    /// <summary>
    /// 麻将游戏主控制器
    /// </summary>
    public class MahjongGameManager : MonoSingleton<MahjongGameManager>
    {
        private TilePool tilePool;
        private PlayerManager playerManager;
        private GameFlowController flowController; // 游戏流程控制器
        private WinChecker winChecker;
        private ScoreCalculator scoreCalculator;

        public GameState CurrentState { get; private set; }
        public int CurrentPlayerIndex { get; private set; }

        public event Action<GameState> OnStateChanged;
        public event Action<Player, PlayerAction> OnPlayerAction;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            InitializeComponents();
        }
        private void Update()
        {
            flowController.Update();
        }
        // 初始化
        private void InitializeComponents()
        {
            tilePool = new TilePool();
            playerManager = new PlayerManager();
            flowController = new GameFlowController(this);
            winChecker = new WinChecker();
            scoreCalculator = new ScoreCalculator();
        }
        // 启动游戏
        public void StartGame()
        {
            tilePool.Initialize();
            playerManager.InitializePlayers();
            flowController.StartGame();
        }

        /// <summary>
        /// 触发状态变化事件
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
