using Mahjong;
using Mahjong.Core.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 单个玩家卡片组件。
/// </summary>
public class PlayerCardWidgetUI : BaseWidgetUI
{
    #region Fields

    private bool isOccupied;
    private LobbyPlayerCardViewData currentViewData;
    private int cardIndex;

    [Header("Occupied Panel")]
    [SerializeField] private GameObject occupiedPanel;
    [SerializeField] private Button avatarBtn;
    [SerializeField] private TextMeshProUGUI nicknameText;
    [SerializeField] private TextMeshProUGUI readyStatusText;
    [SerializeField] private Button kickPlayerBtn;

    [Header("Unoccupied Panel")]
    [SerializeField] private GameObject unoccupiedPanel;
    [SerializeField] private Button addAIPlayerBtn;
    [SerializeField] private Button invitePlayerBtn;

    #endregion

    #region Properties

    /// <summary>
    /// 获取当前卡片是否已被占用。
    /// </summary>
    public bool IsOccupied => isOccupied;

    /// <summary>
    /// 获取当前绑定的视图数据。
    /// </summary>
    public LobbyPlayerCardViewData CurrentViewData => currentViewData;

    /// <summary>
    /// 获取当前卡片索引。
    /// </summary>
    public int CardIndex => cardIndex;

    #endregion

    #region Lifecycle

    protected override void Awake()
    {
        base.Awake();
        RefreshCardIndex();
    }

    #endregion

    #region Internal

    /// <summary>
    /// 根据同级顺序刷新卡片索引。
    /// </summary>
    private void RefreshCardIndex()
    {
        cardIndex = transform.GetSiblingIndex();
    }

    #endregion

    #region Events

    protected override void SetupUIEvents()
    {
        base.SetupUIEvents();
        BindUIEvent(avatarBtn.onClick, OnAvatarBtnClick);
        BindUIEvent(addAIPlayerBtn.onClick, OnAddAIPlayerBtnClick);
        BindUIEvent(invitePlayerBtn.onClick, OnInvitePlayerBtnClick);
        BindUIEvent(kickPlayerBtn.onClick, OnKickPlayerBtnClick);
    }

    /// <summary>
    /// 处理踢出玩家按钮点击事件。
    /// </summary>
    private void OnKickPlayerBtnClick()
    {
        string playerName = currentViewData == null ? "\u672A\u77E5\u73A9\u5BB6" : currentViewData.PlayerName;
        HLogger.Log($"\u70B9\u51FB\u4E86\u8E22\u51FA\u623F\u95F4\u6309\u94AE\uFF0C\u76EE\u6807\u73A9\u5BB6\uFF1A{playerName}");

        if (currentViewData == null)
        {
            Debug.LogError("\u5F53\u524D\u5361\u7247\u6CA1\u6709\u7ED1\u5B9A\u73A9\u5BB6\u6570\u636E\uFF0C\u65E0\u6CD5\u6267\u884C\u8E22\u4EBA\u3002");
            return;
        }

        LobbyPresenter.Instance.KickPlayer(currentViewData.PlayerId);
    }

    /// <summary>
    /// 处理邀请玩家按钮点击事件。
    /// </summary>
    private void OnInvitePlayerBtnClick()
    {
        HLogger.Log("\u70B9\u51FB\u4E86\u9080\u8BF7\u73A9\u5BB6\u6309\u94AE\u3002");
        // TODO: generate invite link and copy to clipboard
    }

    /// <summary>
    /// 处理添加AI玩家按钮点击事件。
    /// </summary>
    private void OnAddAIPlayerBtnClick()
    {
        LobbyPresenter.Instance.AddAIPlayer(AIDifficulty.Normal, cardIndex);
    }

    /// <summary>
    /// 处理头像按钮点击事件。
    /// </summary>
    private void OnAvatarBtnClick()
    {
        LobbyPresenter.Instance.OpenPlayerInfoPanel();
    }

    #endregion

    #region View

    /// <summary>
    /// 切换占用状态。
    /// </summary>
    /// <param name="occupied">是否被占用。</param>
    private void ToggleOccupiedState(bool occupied)
    {
        occupiedPanel.SetActive(occupied);
        unoccupiedPanel.SetActive(!occupied);
    }

    /// <summary>
    /// 将视图数据应用到卡片。
    /// </summary>
    /// <param name="viewData">视图数据。</param>
    public void SetViewData(LobbyPlayerCardViewData viewData)
    {
        isOccupied = true;
        currentViewData = viewData;
        nicknameText.text = viewData.PlayerName;
        readyStatusText.text = viewData.ReadyStatusText;
        ToggleOccupiedState(true);
    }

    /// <summary>
    /// 清空当前卡片状态。
    /// </summary>
    public void Release()
    {
        isOccupied = false;
        currentViewData = null;
        nicknameText.text = string.Empty;
        readyStatusText.text = string.Empty;
        ToggleOccupiedState(false);
    }

    #endregion
}
