using System.Collections.Generic;

namespace Mahjong
{
    /// <summary>
    /// 选庄状态。
    /// </summary>
    public class BankerSelectionState : BaseGameState
    {
        #region 属性

        public override GameState StateType => GameState.BankerSelection;
        public bool IsSelectionComplete => isSelectionComplete;

        #endregion

        #region 字段

        private readonly IEventBusService eventBusService;
        private BankerSelectionService bankerSelectionService;
        private bool isSelectionComplete;

        #endregion

        #region 构造函数

        public BankerSelectionState(GameFlowController controller) : base(controller)
        {
            eventBusService = EventBusService.Instance;
        }

        #endregion

        #region 生命周期

        public override void Enter()
        {
            base.Enter();
            HLogger.LogSuccess($"进入 {StateType} 状态");

            GameSceneManager.LoadGameScene(SceneNameConst.GameScene);
            eventBusService.UIRequestEventSystem.AddListener<RequestRollDiceEvent>(OnRequestRollDice);

            List<Player> players = PlayerManager.Instance.GetPlayers();
            bankerSelectionService = new BankerSelectionService(
                players,
                OnPlayerTurnToRoll,
                OnPlayerDiceRolled,
                OnBankerSelected);

            isSelectionComplete = false;
            bankerSelectionService.StartSelection();
        }

        public override void Exit()
        {
            base.Exit();
            eventBusService.UIRequestEventSystem.RemoveListener<RequestRollDiceEvent>(OnRequestRollDice);
            bankerSelectionService = null;
        }

        #endregion

        #region 事件处理

        private void OnRequestRollDice(RequestRollDiceEvent evt)
        {
            HLogger.Log($"收到玩家 {evt.Player.Info.PlayerName} 的掷骰请求");
            bankerSelectionService.RollDiceForPlayer(evt.Player);
        }

        /// <summary>
        /// 通知当前轮到的玩家掷骰。
        /// </summary>
        /// <param name="player">当前玩家。</param>
        private void OnPlayerTurnToRoll(Player player)
        {
            eventBusService.UIControlEventSystem.Send(new PlayerTurnToRollEvent(player));
        }

        /// <summary>
        /// 通知某位玩家已经完成掷骰。
        /// </summary>
        /// <param name="player">掷骰玩家。</param>
        /// <param name="diceResult">骰子结果。</param>
        private void OnPlayerDiceRolled(Player player, int diceResult)
        {
            eventBusService.UIControlEventSystem.Send(new PlayerDiceRolledEvent(player, diceResult));
        }

        /// <summary>
        /// 庄家确定后进入发牌阶段。
        /// </summary>
        /// <param name="banker">庄家玩家。</param>
        private void OnBankerSelected(Player banker)
        {
            eventBusService.UIControlEventSystem.Send(new BankerSelectedEvent(banker));
            isSelectionComplete = true;
            Controller.TransitionToState(GameState.Dealing);
        }

        #endregion

        #region 状态迁移

        public override bool CanTransitionTo(GameState nextState)
        {
            return nextState == GameState.Dealing;
        }

        #endregion
    }
}
