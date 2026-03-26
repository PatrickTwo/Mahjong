using UnityEngine;
using UnityEngine.UI;

using Mahjong.Core.UI;

public class PromptPanelUI : BasePanelUI
{
    protected override string PanelID => PanelIDConst.PromptPanelID;
    private Button closeBtn;


    protected override void SetupUIEvents()
    {
        base.SetupUIEvents();
        BindUIEvent(closeBtn.onClick, Hide);
    }
}