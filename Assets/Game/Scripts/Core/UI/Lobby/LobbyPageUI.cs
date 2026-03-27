using Mahjong;
using Mahjong.Core.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 大厅页面 UI。
/// </summary>
public class LobbyPageUI : BasePageUI
{
    #region 字段

    [Header("Left Up")]
    [SerializeField] private Button startGameBtn; // 开始游戏按钮
    [SerializeField] private Button playSettingBtn; // 玩法设置按钮
    [Header("Right Up")]
    [SerializeField] private Button gameSettingBtn; // 游戏设置按钮
    [SerializeField] private TextMeshProUGUI roomIDText; // 房间号文本
    [SerializeField] private Button joinRoomBtn; // 加入房间按钮
    [Header("Left Down")]
    [SerializeField] private ChatBoxViewUI chatBoxView; // 聊天框视图
    [SerializeField] private PlayerListViewUI playerListView; // 玩家列表视图
    [Header("Right Down")]
    [SerializeField] private Toggle micTog; // 麦克风开关
    [SerializeField] private Toggle speakerTog; // 扬声器开关

    #endregion

    #region 生命周期

    protected override void Awake()
    {
        base.Awake();
        LobbyPresenter.Instance.Initialize();
    }

    private void OnEnable()
    {
        LobbyPresenter.Instance.ReadModelChanged += OnLobbyReadModelChanged;
        ApplyReadModel(LobbyPresenter.Instance.CurrentReadModel);
    }

    private void OnDisable()
    {
        LobbyPresenter.Instance.ReadModelChanged -= OnLobbyReadModelChanged;
    }

    #endregion

    #region 初始化

    /// <summary>
    /// 初始化 UI 事件绑定。
    /// </summary>
    protected override void SetupUIEvents()
    {
        base.SetupUIEvents();
        BindUIEvent(startGameBtn.onClick, LobbyPresenter.Instance.StartGame);
        BindUIEvent(playSettingBtn.onClick, LobbyPresenter.Instance.OpenPlaySettingPanel);
        BindUIEvent(gameSettingBtn.onClick, LobbyPresenter.Instance.OpenGameSettingPanel);
        BindUIEvent(joinRoomBtn.onClick, LobbyPresenter.Instance.OpenJoinRoomPanel);
        BindUIEvent(micTog.onValueChanged, LobbyPresenter.Instance.SetMicEnabled);
        BindUIEvent(speakerTog.onValueChanged, LobbyPresenter.Instance.SetSpeakerEnabled);
    }

    #endregion

    #region 界面刷新

    /// <summary>
    /// 响应大厅只读模型变化。
    /// </summary>
    /// <param name="readModel">大厅只读模型。</param>
    private void OnLobbyReadModelChanged(LobbyReadModel readModel)
    {
        ApplyReadModel(readModel);
    }

    /// <summary>
    /// 根据只读模型刷新页面。
    /// </summary>
    /// <param name="readModel">大厅只读模型。</param>
    private void ApplyReadModel(LobbyReadModel readModel)
    {
        if (readModel == null)
        {
            return;
        }

        roomIDText.text = readModel.RoomDisplayText;
        startGameBtn.interactable = readModel.CanStartGame;
        micTog.SetIsOnWithoutNotify(readModel.IsMicEnabled);
        speakerTog.SetIsOnWithoutNotify(readModel.IsSpeakerEnabled);
    }

    #endregion
}
