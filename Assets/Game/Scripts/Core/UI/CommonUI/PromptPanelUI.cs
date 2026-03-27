using Mahjong.Core.UI;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 通用提示面板。
/// </summary>
public class PromptPanelUI : BasePanelUI
{
    #region 字段

    protected override string PanelID => PanelIDConst.PromptPanel;
    [SerializeField] private Button closeBtn; // 关闭按钮

    #endregion

    #region 初始化

    /// <summary>
    /// 初始化 UI 事件。
    /// </summary>
    protected override void SetupUIEvents()
    {
        base.SetupUIEvents();
        BindUIEvent(closeBtn.onClick, Hide);
    }

    #endregion
}
