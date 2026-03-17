using Mahjong.Core.UI;
using TMPro;
using UnityEngine.UI;
using Mahjong;

public class JoinRoomPanelUI : BasePanelUI
{
    protected override string PanelID => PanelIDConst.JoinRoomPanelID;
    private Button closeBtn; // 关闭按钮
    private TMP_InputField roomIDInput; // 房间ID输入框
    private Button joinRoomBtn; // 加入房间按钮

    protected override void FindReference()
    {
        base.FindReference();
        closeBtn = transform.FindCompInChild<Button>("CloseBtn");
        roomIDInput = transform.FindCompInChild<TMP_InputField>("RoomIDInput");
        joinRoomBtn = transform.FindCompInChild<Button>("JoinRoomBtn");
    }
    protected override void AddUIListener()
    {
        base.AddUIListener();
        RegisterUIListener(closeBtn.onClick, Hide);
        RegisterUIListener(joinRoomBtn.onClick, OnJoinRoomButtonClick);
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