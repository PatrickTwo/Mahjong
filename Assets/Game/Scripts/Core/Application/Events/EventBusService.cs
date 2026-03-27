using System;
using Mahjong.System.TypeEventSystem;

namespace Mahjong
{
    /// <summary>
    /// 事件总线服务。
    /// </summary>
    public sealed class EventBusService : IEventBusService
    {
        #region 单例

        private static readonly Lazy<EventBusService> LazyInstance = new Lazy<EventBusService>(() => new EventBusService());

        /// <summary>
        /// 获取事件总线服务单例。
        /// </summary>
        public static EventBusService Instance => LazyInstance.Value;

        #endregion

        #region 属性

        /// <summary>
        /// UI 控制事件系统。
        /// </summary>
        public IEventSystem UIControlEventSystem => EventSystemManager.Instance.UIControlEventSystem;

        /// <summary>
        /// UI 请求事件系统。
        /// </summary>
        public IEventSystem UIRequestEventSystem => EventSystemManager.Instance.UIRequestEventSystem;

        /// <summary>
        /// 模型事件系统。
        /// </summary>
        public IEventSystem ModelEventSystem => EventSystemManager.Instance.ModelEventSystem;

        #endregion

        #region 构造函数

        private EventBusService()
        {
        }

        #endregion
    }
}
