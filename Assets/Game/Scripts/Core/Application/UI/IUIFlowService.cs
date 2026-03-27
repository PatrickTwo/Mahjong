namespace Mahjong
{
    /// <summary>
    /// UI 导航服务接口。
    /// </summary>
    public interface IUIFlowService
    {
        #region 面板控制

        /// <summary>
        /// 显示指定面板。
        /// </summary>
        /// <param name="panelId">面板唯一标识。</param>
        void ShowPanel(string panelId);

        /// <summary>
        /// 隐藏指定面板。
        /// </summary>
        /// <param name="panelId">面板唯一标识。</param>
        void HidePanel(string panelId);

        #endregion
    }
}
