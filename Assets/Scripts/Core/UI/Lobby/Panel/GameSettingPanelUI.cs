using System;
using UnityEngine;
using UnityEngine.UI;


using Mahjong.Core.UI;

public class GameSettingPanelUI : BasePanelUI
{
    protected override string PanelID => PanelIDConst.GameSettingPanelID;
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

