using Mahjong.Core.UI;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoPanelUI : BasePanelUI
{
    protected override string PanelID => PanelIDConst.PlayerInfoPanelID;
    [SerializeField] private Button closeBtn;

    protected override void AddUIListener()
    {
        base.AddUIListener();
        RegisterUIListener(closeBtn.onClick, Hide);
    }
}
