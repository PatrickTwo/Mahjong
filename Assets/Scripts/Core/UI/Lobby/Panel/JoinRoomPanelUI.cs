using Mahjong.Core.UI;
using UnityEngine;
using UnityEngine.UI;

public class JoinRoomPanelUI : BasePanelUI
{
    protected override string PanelID => PanelIDConst.JoinRoomPanelID;
    private Button closeBtn;

    protected override void FindReference()
    {
        base.FindReference();
        closeBtn = transform.FindCompInChild<Button>("CloseBtn");
    }
    protected override void AddUIListener()
    {
        base.AddUIListener();
        RegisterUIListener(closeBtn.onClick, Hide);
    }
}