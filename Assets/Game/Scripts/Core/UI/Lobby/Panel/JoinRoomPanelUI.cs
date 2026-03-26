using Mahjong.Core.UI;
using TMPro;
using UnityEngine.UI;
using Mahjong;
using UnityEngine;

public class JoinRoomPanelUI : BasePanelUI
{
    protected override string PanelID => PanelIDConst.JoinRoomPanelID;
    [SerializeField] private Button closeBtn; // 关闭按钮
    [SerializeField] private TMP_InputField roomIDInput; // 房间ID输入框
    [SerializeField] private Button joinRoomBtn; // 加入房间按钮

    protected override void SetupUIEvents()
    {
        base.SetupUIEvents();
        BindUIEvent(closeBtn.onClick, Hide);
        BindUIEvent(joinRoomBtn.onClick, OnJoinRoomButtonClick);
    }

    private void OnJoinRoomButtonClick()
    {
        // TODO 验证输入的ID的合法性（是否为空，是否为数字）
        bool isRoomIDValid = true;
        if (!isRoomIDValid)
            return;

        UIRequestEventSystem.Send(new OnJoinButtonClick(roomIDInput.text));
    }
}