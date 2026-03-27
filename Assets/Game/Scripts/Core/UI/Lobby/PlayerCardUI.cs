using Mahjong;
using Mahjong.Core.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 单个玩家卡片 UI 组件。
/// </summary>
public class PlayerCardUI : BaseUI
{
    #region 字段与属性

    private bool isOccupied; // 是否已被玩家占用
    public bool IsOccupied => isOccupied;
    private LobbyPlayerCardViewData currentViewData; // 当前卡片绑定的只读数据
    public LobbyPlayerCardViewData CurrentViewData => currentViewData;

    [Header("Occupied Panel")]
    [SerializeField] private GameObject occupiedPanel; // 已被占用时显示的内容
    [SerializeField] private Button avatarBtn; // 玩家头像按钮
    [SerializeField] private TextMeshProUGUI nicknameText; // 玩家昵称
    [SerializeField] private TextMeshProUGUI readyStatusText; // 准备状态
    [SerializeField] private Button kickPlayerBtn; // 踢人按钮

    [Header("Unoccupied Panel")]
    [SerializeField] private GameObject unoccupiedPanel; // 空位时显示的内容
    [SerializeField] private Button addAIPlayerBtn; // 添加 AI 按钮
    [SerializeField] private Button invitePlayerBtn; // 邀请玩家按钮

    #endregion

    #region UI 事件绑定

    protected override void SetupUIEvents()
    {
        base.SetupUIEvents();
        BindUIEvent(avatarBtn.onClick, OnAvatarBtnClick);
        BindUIEvent(addAIPlayerBtn.onClick, OnAddAIPlayerBtnClick);
        BindUIEvent(invitePlayerBtn.onClick, OnInvitePlayerBtnClick);
        BindUIEvent(kickPlayerBtn.onClick, OnKickPlayerBtnClick);
    }

    #endregion

    #region UI 事件

    /// <summary>
    /// 点击踢人按钮。
    /// </summary>
    private void OnKickPlayerBtnClick()
    {
        string playerName = currentViewData == null ? "未知玩家" : currentViewData.PlayerName;
        HLogger.Log($"点击了踢出房间按钮，目标玩家：{playerName}");
        // TODO: 踢出房间
    }

    /// <summary>
    /// 点击邀请玩家按钮。
    /// </summary>
    private void OnInvitePlayerBtnClick()
    {
        HLogger.Log("点击了邀请玩家按钮");
        // TODO: 生成邀请链接并复制到剪贴板
    }

    /// <summary>
    /// 点击添加 AI 按钮。
    /// </summary>
    private void OnAddAIPlayerBtnClick()
    {
        LobbyPresenter.Instance.AddAIPlayer(AIDifficulty.Normal);
    }

    /// <summary>
    /// 点击头像按钮。
    /// </summary>
    private void OnAvatarBtnClick()
    {
        LobbyPresenter.Instance.OpenPlayerInfoPanel();
    }

    #endregion

    #region 视图刷新

    /// <summary>
    /// 切换卡片占用状态。
    /// </summary>
    /// <param name="occupied">是否已占用。</param>
    private void ToggleOccupiedState(bool occupied)
    {
        occupiedPanel.SetActive(occupied);
        unoccupiedPanel.SetActive(!occupied);
    }

    /// <summary>
    /// 设置玩家卡片视图数据。
    /// </summary>
    /// <param name="viewData">玩家卡片视图数据。</param>
    public void SetViewData(LobbyPlayerCardViewData viewData)
    {
        isOccupied = true;
        currentViewData = viewData;
        nicknameText.text = viewData.PlayerName;
        readyStatusText.text = viewData.ReadyStatusText;
        ToggleOccupiedState(true);
    }

    /// <summary>
    /// 释放当前卡片占用。
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
