using System;

namespace Mahjong
{
    /// <summary>
    /// 对局 HUD Presenter。
    /// </summary>
    public sealed class GameHudPresenter
    {
        #region 单例

        private static readonly Lazy<GameHudPresenter> LazyInstance = new Lazy<GameHudPresenter>(() => new GameHudPresenter());

        /// <summary>
        /// 获取对局 HUD Presenter 单例。
        /// </summary>
        public static GameHudPresenter Instance => LazyInstance.Value;

        #endregion

        #region 字段

        private readonly IEventBusService eventBusService;
        private bool isInitialized;
        private GameState currentState;
        private Player currentRollingPlayer;
        private Player lastDicePlayer;
        private int lastDiceResult;
        private Player banker;
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

        private void OnPlayerTurnToRoll(PlayerTurnToRollEvent evt)
        {
            currentRollingPlayer = evt.Player;
            promptMessage = $"{evt.Player.Info.PlayerName} 请掷骰。";
            RefreshReadModel();
        }

        private void OnPlayerDiceRolled(PlayerDiceRolledEvent evt)
        {
            lastDicePlayer = evt.Player;
            lastDiceResult = evt.DiceResult;
            currentRollingPlayer = null;
            promptMessage = $"{evt.Player.Info.PlayerName} 掷出了 {evt.DiceResult} 点。";
            RefreshReadModel();
        }

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
