using System;
using Mahjong;
using Mahjong.Core.UI;
using Sirenix.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 单个玩家卡片UI组件
/// 显示在首页玩家列表中
/// 每个玩家卡片包含玩家头像、昵称、准备状态、玩家操作菜单（踢出房间）等信息
/// </summary>
public class PlayerCardUI : BaseUI
{
    private bool isOccupied = false; // 是否已被玩家占用
    public bool IsOccupied => isOccupied;
    private Player player; // 该玩家卡片当前占用的玩家
    public Player Player => player;
    // 该位置有玩家时显示的内容------------------------------------------------
    [Header("Occupied Panel")]
    [SerializeField] private GameObject occupiedPanel; // 已被玩家占用时显示的内容
    [SerializeField] private Button avatarBtn; // 玩家头像图像
    [SerializeField] private TextMeshProUGUI nicknameText; // 玩家昵称文本
    [SerializeField] private TextMeshProUGUI readyStatusText; // 准备状态显示
    [SerializeField] private Button kickPlayerBtn; // 踢出房间按钮
    // 该位置还未加入玩家时的相关UI------------------------------------------------
    [Header("Unoccupied Panel")]
    [SerializeField] private GameObject unoccupiedPanel; // 未被玩家占用时显示的内容
    [SerializeField] private Button addAIPlayerBtn; // 添加AI玩家按钮
    [SerializeField] private Button invitePlayerBtn; // 邀请玩家按钮

    protected override void AddUIListener()
    {
        base.AddUIListener();
        RegisterUIListener(avatarBtn.onClick, OnAvatarBtnClick);
        RegisterUIListener(addAIPlayerBtn.onClick, OnAddAIPlayerBtnClick);
        RegisterUIListener(invitePlayerBtn.onClick, OnInvitePlayerBtnClick);

        RegisterUIListener(kickPlayerBtn.onClick, OnKickPlayerBtnClick);
    }
    #region UI事件
    // 点击踢出房间按钮
    private void OnKickPlayerBtnClick()
    {
        HLogger.Log($"点击了踢出房间按钮");
        // TODO 踢出房间
    }
    // 点击邀请玩家按钮
    private void OnInvitePlayerBtnClick()
    {
        HLogger.Log($"点击了邀请玩家按钮");
        // TODO 根据当前房间号生成邀请链接并复制到剪贴板
    }
    // 点击添加AI玩家按钮
    private void OnAddAIPlayerBtnClick()
    {
        // 添加AI玩家
        UIRequestEventSystem.Send(new AddAIPlayerRequestEvent());
    }

    private void OnAvatarBtnClick()
    {
        // 点击玩家头像时显示玩家信息面板
        UIControlEventSystem.Send(new ShowPanelEvent(PanelIDConst.PlayerInfoPanelID));
    }
    #endregion
    // 切换占用状态
    private void ToggleOccupiedState(bool isOccupied)
    {
        occupiedPanel.SetActive(isOccupied);
        unoccupiedPanel.SetActive(!isOccupied);
    }
    #region 配置玩家信息
    // 设置卡片
    public void SetPlayer(Player player)
    {
        isOccupied = true;
        this.player = player;
        nicknameText.text = player.Info.PlayerName;
        // TODO 设置头像
        ToggleOccupiedState(true);
    }
    // 清除占用
    public void Release()
    {
        isOccupied = false;
        player = null;
        nicknameText.text = string.Empty;
        readyStatusText.text = string.Empty;
        ToggleOccupiedState(false);
    }
    #endregion
}