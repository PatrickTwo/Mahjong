using Mahjong.System.TypeEventSystem;

/// <summary>
/// 显示面板事件
/// </summary>
public struct ShowPanelEvent : IEvent
{
    public string PanelID;
    public ShowPanelEvent(string panelID)
    {
        PanelID = panelID;
    }
}
/// <summary>
/// 隐藏面板事件
/// </summary>
public struct HidePanelEvent : IEvent
{
    public string PanelID;
    public HidePanelEvent(string panelID)
    {
        PanelID = panelID;
    }
}