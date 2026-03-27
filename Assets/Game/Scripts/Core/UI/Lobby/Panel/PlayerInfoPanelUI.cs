using Mahjong.Core.UI;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoPanelUI : BasePanelUI
{
    protected override string PanelID => PanelIDConst.PlayerInfoPanel;
    [SerializeField] private Button closeBtn;

    protected override void SetupUIEvents()
    {
        base.SetupUIEvents();
        BindUIEvent(closeBtn.onClick, Hide);
    }
}
