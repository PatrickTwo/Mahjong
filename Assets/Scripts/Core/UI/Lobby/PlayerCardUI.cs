using Mahjong.Core.UI;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// 单个玩家卡片UI组件
/// 显示在首页玩家列表中
/// 每个玩家卡片包含玩家头像、昵称、准备状态、玩家操作菜单（踢出房间）等信息
/// </summary>
public class PlayerCardUI : BaseUI
{
    // UI组件引用
    private Button avatarBtn; // 玩家头像图像
    private TextMeshProUGUI nicknameText; // 玩家昵称文本
    private TextMeshProUGUI readyStatusText; // 准备状态显示
    private Toggle operationTog; // 玩家操作菜单切换按钮

    protected override void FindReference()
    {
        avatarBtn = transform.FindCompInChild<Button>("AvatarBtn");
        nicknameText = transform.FindCompInChild<TextMeshProUGUI>("NicknameText");
        readyStatusText = transform.FindCompInChild<TextMeshProUGUI>("ReadyStatusText");
        operationTog = transform.FindCompInChild<Toggle>("OperationTog");
    }
    protected override void AddUIListener()
    {
        base.AddUIListener();
        RegisterUIListener(avatarBtn.onClick, OnAvatarBtnClick);
        RegisterUIListener(operationTog.onValueChanged, OnOperationTogChange);
    }

    private void OnOperationTogChange(bool arg0)
    {
        HLogger.Log($"玩家操作菜单");
    }

    private void OnAvatarBtnClick()
    {
       // 点击玩家头像时显示玩家信息面板
       UIEventSystem.Send(new ShowPanelEvent(PanelIDConst.PlayerInfoPanelID));
    }
}