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
        public GameFlowController FlowController => flowController;
        private WinChecker winChecker;
        private ScoreCalculator scoreCalculator;

        public int CurrentPlayerIndex { get; private set; }
        private void Awake()
        {
            InitializeComponents();
            // 初始化程序
            flowController.InitializeGame();
        }
        private void Update()
        {
            flowController.UpdateState();
        }
        // 初始化
        private void InitializeComponents()
        {
            tilePool = new TilePool();
            tilePool.Initialize();

            flowController = new GameFlowController(this);
            winChecker = new WinChecker();
            scoreCalculator = new ScoreCalculator();
        }

        /// <summary>
        /// 触发状态变化事件
        /// </summary>
        /// <param name="newState">新的游戏状态</param>
        public void TriggerStateChanged(GameState newState)
        {
            // 状态变更事件发送
            EventSystemManager.Instance.ModelEventSystem.Send(new EnterStateEvent(newState));
        }
    }
    #endregion
}
