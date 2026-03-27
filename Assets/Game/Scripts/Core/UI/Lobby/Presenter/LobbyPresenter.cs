using System;
using Mahjong.Core.UI;

namespace Mahjong
{
    /// <summary>
    /// 大厅页面 Presenter。
    /// </summary>
    public class LobbyPresenter : LazySingleton<LobbyPresenter>
    {
        #region 字段

        private readonly ILobbyService lobbyService;
        private readonly IUIFlowService uiFlowService;
        private readonly IAudioSettingService audioSettingService;
        private readonly IEventBusService eventBusService;
        private bool isInitialized;

        #endregion

        #region 事件与属性

        public event Action<LobbyReadModel> ReadModelChanged;
        public LobbyReadModel CurrentReadModel { get; private set; }

        #endregion

        #region 构造函数

        private LobbyPresenter()
        {
            lobbyService = new LobbyService();
            uiFlowService = new UIFlowService();
            audioSettingService = new LobbyAudioSettingService();
            eventBusService = EventBusService.Instance;
        }

        #endregion

        #region 初始化

        public void Initialize()
        {
            if (isInitialized)
            {
                RefreshReadModel();
                return;
            }

            eventBusService.ModelEventSystem.AddListener<AddPlayerEvent>(OnAddPlayer);
            eventBusService.ModelEventSystem.AddListener<RemovePlayerEvent>(OnRemovePlayer);
            eventBusService.ModelEventSystem.AddListener<EnterStateEvent>(OnEnterState);

            isInitialized = true;
            RefreshReadModel();
        }

        #endregion

        #region 页面操作

        public void OpenJoinRoomPanel()
        {
            EnsureInitialized();
            uiFlowService.ShowPanel(PanelIDConst.JoinRoomPanel);
        }

        public void OpenGameSettingPanel()
        {
            EnsureInitialized();
            uiFlowService.ShowPanel(PanelIDConst.GameSettingPanel);
        }

        public void OpenPlaySettingPanel()
        {
            EnsureInitialized();
            uiFlowService.ShowPanel(PanelIDConst.PlaySettingPanel);
        }

        public void OpenPlayerInfoPanel()
        {
            EnsureInitialized();
            uiFlowService.ShowPanel(PanelIDConst.PlayerInfoPanel);
        }

        public void StartGame()
        {
            EnsureInitialized();

            bool success = lobbyService.TryStartGame(out string message);
            if (success)
            {
                HLogger.LogSuccess(message);
            }
            else
            {
                HLogger.LogFail(message);
            }

            RefreshReadModel();
        }

        public void JoinRoom(string roomId)
        {
            EnsureInitialized();

            bool success = lobbyService.TryJoinRoom(roomId, out string message);
            if (!success)
            {
                HLogger.LogFail(message);
                return;
            }

            HLogger.LogSuccess(message);
            uiFlowService.HidePanel(PanelIDConst.JoinRoomPanel);
            RefreshReadModel();
        }

        public void SetMicEnabled(bool isEnabled)
        {
            EnsureInitialized();
            audioSettingService.SetMicEnabled(isEnabled);
            HLogger.Log($"麦克风状态已切换为：{(isEnabled ? "开启" : "关闭")}");
            RefreshReadModel();
        }

        public void SetSpeakerEnabled(bool isEnabled)
        {
            EnsureInitialized();
            audioSettingService.SetSpeakerEnabled(isEnabled);
            HLogger.Log($"扬声器状态已切换为：{(isEnabled ? "开启" : "关闭")}");
            RefreshReadModel();
        }
        /// <summary>
        /// 添加 AI 玩家。
        /// </summary>
        /// <param name="difficulty"></param>
        public void AddAIPlayer(AIDifficulty difficulty, int cardIndex)
        {
            EnsureInitialized();

            bool success = lobbyService.TryAddAIPlayer(difficulty, cardIndex, out string message);
            if (success)
            {
                HLogger.LogSuccess(message);
            }
            else
            {
                HLogger.LogFail(message);
            }

            RefreshReadModel();
        }
        /// <summary>
        /// 踢出玩家
        /// </summary>
        public void KickPlayer(int playerId)
        {
            EnsureInitialized();

            bool success = lobbyService.TryKickPlayer(playerId, out string message);
            if (success)
            {
                HLogger.LogSuccess(message);
            }
            else
            {
                HLogger.LogFail(message);
            }

            RefreshReadModel();
        }

        #endregion

        #region 内部逻辑

        private void EnsureInitialized()
        {
            if (!isInitialized)
            {
                Initialize();
            }
        }

        private void RefreshReadModel()
        {
            bool isMicEnabled = audioSettingService.GetMicEnabled();
            bool isSpeakerEnabled = audioSettingService.GetSpeakerEnabled();
            CurrentReadModel = lobbyService.BuildReadModel(isMicEnabled, isSpeakerEnabled);

            Action<LobbyReadModel> handler = ReadModelChanged;
            handler?.Invoke(CurrentReadModel);
        }

        #endregion

        #region 事件回调

        private void OnAddPlayer(AddPlayerEvent evt)
        {
            RefreshReadModel();
        }

        private void OnRemovePlayer(RemovePlayerEvent evt)
        {
            RefreshReadModel();
        }

        private void OnEnterState(EnterStateEvent evt)
        {
            RefreshReadModel();
        }

        #endregion
    }
}
