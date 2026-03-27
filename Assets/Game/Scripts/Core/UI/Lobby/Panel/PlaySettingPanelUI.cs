using Mahjong.Core.UI;
using UnityEngine;
using UnityEngine.UI;

public class PlaySettingPanelUI : BasePanelUI
{
    protected override string PanelID => PanelIDConst.PlaySettingPanel;
    [SerializeField] private Button closeBtn;


    protected override void SetupUIEvents()
    {
        base.SetupUIEvents();
        BindUIEvent(closeBtn.onClick, Hide);
    }
}
