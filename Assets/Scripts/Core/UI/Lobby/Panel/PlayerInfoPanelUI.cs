using Mahjong.Core.UI;
using UnityEngine.UI;

public class PlayerInfoPanelUI : BasePanelUI
{
    protected override string PanelID => PanelIDConst.PlayerInfoPanelID;
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
