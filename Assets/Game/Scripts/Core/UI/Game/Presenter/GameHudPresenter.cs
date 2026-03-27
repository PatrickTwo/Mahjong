using System;

namespace Mahjong
{
    /// <summary>
    /// 对局 HUD Presenter。
    /// </summary>
    public class GameHudPresenter : LazySingleton<GameHudPresenter>
    {
        #region 字段

        /// <summary>
        /// 事件总线服务，用于监听和发送事件。
        /// </summary>
        private readonly IEventBusService eventBusService;

        /// <summary>
        /// 标记是否已完成事件订阅初始化，防止重复绑定。
        /// </summary>
        private bool isInitialized;

        /// <summary>
        /// 当前对局阶段状态。
        /// </summary>
        private GameState currentState;

        /// <summary>
        /// 当前轮到掷骰的玩家。
        /// </summary>
        private Player currentRollingPlayer;

        /// <summary>
        /// 上一个完成掷骰的玩家。
        /// </summary>
        private Player lastDicePlayer;

        /// <summary>
        /// 上一次掷骰结果。
        /// </summary>
        private int lastDiceResult;

        /// <summary>
        /// 当前庄家。
        /// </summary>
        private Player banker;

        /// <summary>
        /// HUD 提示文案。
        /// </summary>
        private string promptMessage;

        #endregion

        #region 事件与属性

        /// <summary>
        /// 只读模型更新事件。
        /// </summary>
        public event Action<GameHudReadModel> ReadModelChanged;

        /// <summary>
        /// 当前只读模型。
        /// </summary>
        public GameHudReadModel CurrentReadModel { get; private set; }

        #endregion

        #region 构造函数

        private GameHudPresenter()
        {
            eventBusService = EventBusService.Instance;
            currentState = GameState.LobbyWaiting;
            promptMessage = "等待进入对局阶段。";
        }

        #endregion

        #region 初始化

        /// <summary>
        /// 初始化 Presenter。
        /// </summary>
        public void Initialize()
        {
            if (isInitialized)
            {
                RefreshReadModel();
                return;
            }

            eventBusService.ModelEventSystem.AddListener<EnterStateEvent>(OnEnterState);
            eventBusService.UIControlEventSystem.AddListener<PlayerTurnToRollEvent>(OnPlayerTurnToRoll);
            eventBusService.UIControlEventSystem.AddListener<PlayerDiceRolledEvent>(OnPlayerDiceRolled);
            eventBusService.UIControlEventSystem.AddListener<BankerSelectedEvent>(OnBankerSelected);

            isInitialized = true;
            RefreshReadModel();
        }

        #endregion

        #region 对外操作

        /// <summary>
        /// 请求指定玩家执行掷骰。
        /// </summary>
        /// <param name="player">执行掷骰的玩家。</param>
        public void RequestRollDice(Player player)
        {
            if (player == null)
            {
                HLogger.LogFail("请求掷骰失败，玩家为空。");
                return;
            }

            Initialize();
            eventBusService.UIRequestEventSystem.Send(new RequestRollDiceEvent(player));
        }

        #endregion

        #region 事件回调

        /// <summary>
        /// 对局阶段切换回调，根据新阶段更新提示文案。
        /// </summary>
        private void OnEnterState(EnterStateEvent evt)
        {
            currentState = evt.State;

            if (evt.State == GameState.BankerSelection)
            {
                ResetBankerSelectionContext();
                promptMessage = "等待玩家依次掷骰选庄。";
            }
            else if (evt.State == GameState.Playing)
            {
                promptMessage = "对局进行中。";
            }
            else if (evt.State == GameState.Dealing)
            {
                promptMessage = "正在发牌。";
            }
            else
            {
                promptMessage = $"当前阶段：{evt.State}";
            }

            RefreshReadModel();
        }

        /// <summary>
        /// 玩家轮到掷骰回调，更新当前掷骰玩家和提示文案。
        /// </summary>
        private void OnPlayerTurnToRoll(PlayerTurnToRollEvent evt)
        {
            currentRollingPlayer = evt.Player;
            promptMessage = $"{evt.Player.Info.PlayerName} 请掷骰。";
            RefreshReadModel();
        }

        /// <summary>
        /// 玩家完成掷骰回调，记录掷骰结果并更新提示文案。
        /// </summary>
        private void OnPlayerDiceRolled(PlayerDiceRolledEvent evt)
        {
            lastDicePlayer = evt.Player;
            lastDiceResult = evt.DiceResult;
            currentRollingPlayer = null;
            promptMessage = $"{evt.Player.Info.PlayerName} 掷出了 {evt.DiceResult} 点。";
            RefreshReadModel();
        }

        /// <summary>
        /// 庄家确定回调，记录庄家信息并更新提示文案。
        /// </summary>
        private void OnBankerSelected(BankerSelectedEvent evt)
        {
            banker = evt.Banker;
            currentRollingPlayer = null;
            promptMessage = $"庄家已确定：{evt.Banker.Info.PlayerName}";
            RefreshReadModel();
        }

        #endregion

        #region 内部逻辑

        /// <summary>
        /// 重置选庄上下文数据。
        /// </summary>
        private void ResetBankerSelectionContext()
        {
            currentRollingPlayer = null;
            lastDicePlayer = null;
            lastDiceResult = 0;
            banker = null;
        }

        /// <summary>
        /// 刷新只读模型。
        /// </summary>
        private void RefreshReadModel()
        {
            string currentRollingPlayerName = currentRollingPlayer == null ? string.Empty : currentRollingPlayer.Info.PlayerName;
            string lastDicePlayerName = lastDicePlayer == null ? string.Empty : lastDicePlayer.Info.PlayerName;
            string bankerName = banker == null ? string.Empty : banker.Info.PlayerName;
            bool isBankerSelectionVisible = currentState == GameState.BankerSelection;
            bool shouldShowPlayerOperationPanel = currentState == GameState.Playing || currentState == GameState.TingDeclared;

            CurrentReadModel = new GameHudReadModel(
                currentState,
                isBankerSelectionVisible,
                shouldShowPlayerOperationPanel,
                promptMessage,
                currentRollingPlayerName,
                lastDicePlayerName,
                lastDiceResult,
                bankerName);

            Action<GameHudReadModel> handler = ReadModelChanged;
            handler?.Invoke(CurrentReadModel);
        }

        #endregion
    }
}
