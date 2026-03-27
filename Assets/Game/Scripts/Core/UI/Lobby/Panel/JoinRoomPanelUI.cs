using Mahjong;
using Mahjong.Core.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 加入房间面板。
/// </summary>
public class JoinRoomPanelUI : BasePanelUI
{
    #region 字段

    protected override string PanelID => PanelIDConst.JoinRoomPanel;
    [SerializeField] private Button closeBtn; // 关闭按钮
    [SerializeField] private TMP_InputField roomIDInput; // 房间 ID 输入框
    [SerializeField] private Button joinRoomBtn; // 加入房间按钮

    #endregion

    #region 初始化

    /// <summary>
    /// 初始化 UI 事件。
    /// </summary>
    protected override void SetupUIEvents()
    {
        base.SetupUIEvents();
        BindUIEvent(closeBtn.onClick, Hide);
        BindUIEvent(joinRoomBtn.onClick, OnJoinRoomButtonClick);
    }

    #endregion

    #region UI 事件

    /// <summary>
    /// 点击加入房间按钮。
    /// </summary>
    private void OnJoinRoomButtonClick()
    {
        LobbyPresenter.Instance.JoinRoom(roomIDInput.text);
    }

    #endregion
}
