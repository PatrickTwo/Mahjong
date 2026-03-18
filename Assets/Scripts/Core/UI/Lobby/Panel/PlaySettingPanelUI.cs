using Mahjong.Core.UI;
using UnityEngine;
using UnityEngine.UI;

public class PlaySettingPanelUI : BasePanelUI
{
    protected override string PanelID => PanelIDConst.PlaySettingPanelID;
    [SerializeField] private Button closeBtn;


    protected override void AddUIListener()
    {
        base.AddUIListener();
        RegisterUIListener(closeBtn.onClick, Hide);
    }
}
