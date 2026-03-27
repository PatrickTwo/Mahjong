using Mahjong.System.TypeEventSystem;

namespace Mahjong
{
    /// <summary>
    /// 事件总线服务接口。
    /// </summary>
    public interface IEventBusService
    {
        #region 事件系统

        /// <summary>
        /// UI 控制事件系统。
        /// </summary>
        IEventSystem UIControlEventSystem { get; }

        /// <summary>
        /// UI 请求事件系统。
        /// </summary>
        IEventSystem UIRequestEventSystem { get; }

        /// <summary>
        /// 模型事件系统。
        /// </summary>
        IEventSystem ModelEventSystem { get; }

        #endregion
    }
}
