namespace Mahjong
{
    /// <summary>
    /// UI 导航服务。
    /// </summary>
    public class UIFlowService : IUIFlowService
    {
        #region 字段

        private readonly IEventBusService eventBusService;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造 UI 导航服务。
        /// </summary>
        public UIFlowService()
        {
            eventBusService = EventBusService.Instance;
        }

        #endregion

        #region 面板控制

        /// <summary>
        /// 显示指定面板。
        /// </summary>
        /// <param name="panelId">面板唯一标识。</param>
        public void ShowPanel(string panelId)
        {
            eventBusService.UIControlEventSystem.Send(new ShowPanelEvent(panelId));
        }

        /// <summary>
        /// 隐藏指定面板。
        /// </summary>
        /// <param name="panelId">面板唯一标识。</param>
        public void HidePanel(string panelId)
        {
            eventBusService.UIControlEventSystem.Send(new HidePanelEvent(panelId));
        }

        #endregion
    }
}
