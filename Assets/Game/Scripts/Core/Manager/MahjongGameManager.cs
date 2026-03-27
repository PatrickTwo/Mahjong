using UnityEngine;

namespace Mahjong
{
    #region 麻将游戏主控制器

    /// <summary>
    /// 麻将游戏主控制器。
    /// </summary>
    public class MahjongGameManager : MonoSingleton<MahjongGameManager>
    {
        private TilePool tilePool;
        private PlayerManager playerManager;
        private GameFlowController flowController;
        public GameFlowController FlowController => flowController;
        private WinChecker winChecker;
        private ScoreCalculator scoreCalculator;
        private IEventBusService eventBusService;

        public int CurrentPlayerIndex { get; private set; }

        private void Awake()
        {
            InitializeComponents();
            GameHudPresenter.Instance.Initialize();
            flowController.InitializeGame();
        }

        private void Update()
        {
            flowController.UpdateState();
        }

        /// <summary>
        /// 初始化运行时组件。
        /// </summary>
        private void InitializeComponents()
        {
            eventBusService = EventBusService.Instance;

            tilePool = new TilePool();
            tilePool.Initialize();

            flowController = new GameFlowController(this);
            winChecker = new WinChecker();
            scoreCalculator = new ScoreCalculator();
        }

        /// <summary>
        /// 触发状态变更事件。
        /// </summary>
        /// <param name="newState">新的游戏状态。</param>
        public void TriggerStateChanged(GameState newState)
        {
            eventBusService.ModelEventSystem.Send(new EnterStateEvent(newState));
        }
    }

    #endregion
}
