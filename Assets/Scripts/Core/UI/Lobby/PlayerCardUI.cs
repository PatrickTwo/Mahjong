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
    // 该位置有玩家时显示的内容------------------------------------------------
    private GameObject playerContent;
    private Button avatarBtn; // 玩家头像图像
    private TextMeshProUGUI nicknameText; // 玩家昵称文本
    private TextMeshProUGUI readyStatusText; // 准备状态显示
    // 该位置还未加入玩家时的相关UI------------------------------------------------
    private Button addPlayerBtn; // 添加玩家按钮
    private GameObject addPlayerMenu; // 添加玩家菜单相关内容
    private Button addAIPlayerBtn; // 添加AI玩家按钮
    private Button invitePlayerBtn; // 邀请玩家按钮
    // 玩家操作菜单相关内容------------------------------------------------
    private Toggle operationTog; // 玩家操作菜单切换按钮
    private GameObject operationMenu;
    private Button kickPlayerBtn; // 踢出房间按钮
    protected override void FindReference()
    {
        avatarBtn = transform.FindCompInChild<Button>("AvatarBtn");
        nicknameText = transform.FindCompInChild<TextMeshProUGUI>("NicknameText");
        readyStatusText = transform.FindCompInChild<TextMeshProUGUI>("ReadyStatusText");
        operationTog = transform.FindCompInChild<Toggle>("OperationTog");
        addPlayerBtn = transform.FindCompInChild<Button>("AddPlayerBtn");
        addPlayerMenu = transform.FindChildGo("AddPlayerMenu");
        addAIPlayerBtn = transform.FindCompInChild<Button>("AddAIPlayerBtn");
        invitePlayerBtn = transform.FindCompInChild<Button>("InvitePlayerBtn");
    }
    protected override void AddUIListener()
    {
        base.AddUIListener();
        RegisterUIListener(avatarBtn.onClick, OnAvatarBtnClick);
        RegisterUIListener(operationTog.onValueChanged, OnOperationTogChange);
        RegisterUIListener(addPlayerBtn.onClick, OnAddPlayerBtnClick);
        RegisterUIListener(addAIPlayerBtn.onClick, OnAddAIPlayerBtnClick);
        RegisterUIListener(invitePlayerBtn.onClick, OnInvitePlayerBtnClick);

        RegisterUIListener(kickPlayerBtn.onClick, OnKickPlayerBtnClick);
    }
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
        HLogger.Log($"点击了添加AI玩家按钮");
        // TODO 添加AI玩家
    }

    private void OnAddPlayerBtnClick()
    {
        // 点击添加玩家按钮时，显示添加玩家菜单
        addPlayerMenu.SetActive(true);
    }

    private void OnOperationTogChange(bool arg0)
    {
        HLogger.Log($"点击玩家操作菜单");
    }

    private void OnAvatarBtnClick()
    {
        // 点击玩家头像时显示玩家信息面板
        UIEventSystem.Send(new ShowPanelEvent(PanelIDConst.PlayerInfoPanelID));
    }
    // 更新玩家卡片显示信息
    public void UpdatePlayerDisplay(Player player)
    {
        
    }
}