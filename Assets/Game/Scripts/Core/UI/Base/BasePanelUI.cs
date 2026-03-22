using Mahjong.Core.UI;
using Mahjong.System.TypeEventSystem;
using UnityEngine;

/// <summary>
/// 面板UI基类
/// </summary>
public abstract class BasePanelUI : BaseUI
{
    protected abstract string PanelID { get; }
    protected override void Awake()
    {
        base.Awake();
        UIControlEventSystem.AddListener<ShowPanelEvent>(OnReceiveShowPanelEvent)
            .RemoveListenerWhenGameObjectDestroyed(gameObject);
        UIControlEventSystem.AddListener<HidePanelEvent>(OnReceiveHidePanelEvent)
            .RemoveListenerWhenGameObjectDestroyed(gameObject);
    }

    protected override void Start()
    {
        base.Start();
    }

    // 接收显示面板事件
    private void OnReceiveShowPanelEvent(ShowPanelEvent evt)
    {
        if (evt.PanelID != PanelID) return;
        Show();
    }
    // 接收隐藏面板事件
    private void OnReceiveHidePanelEvent(HidePanelEvent evt)
    {
        if (evt.PanelID != PanelID) return;
        Hide();
    }


}
