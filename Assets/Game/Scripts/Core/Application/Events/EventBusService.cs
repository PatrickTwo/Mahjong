using Mahjong.System.TypeEventSystem;

namespace Mahjong
{
    /// <summary>
    /// 事件总线服务。作为外观（Facade）对外暴露事件系统接口。
    /// </summary>
    public class EventBusService : LazySingleton<EventBusService>, IEventBusService
    {
        #region 构造函数

        private EventBusService() { }

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
    }
}
