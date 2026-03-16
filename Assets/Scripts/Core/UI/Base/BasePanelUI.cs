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
        UIEventSystem.AddListener<ShowPanelEvent>(OnReceiveShowPanelEvent)
            .RemoveListenerWhenGameObjectDestroyed(gameObject);
    }

    protected override void Start()
    {
        base.Start();
    }

    /// 接收显示面板事件
    private void OnReceiveShowPanelEvent(ShowPanelEvent evt)
    {
        if (evt.PanelID != PanelID) return;
        Show();
    }

}
