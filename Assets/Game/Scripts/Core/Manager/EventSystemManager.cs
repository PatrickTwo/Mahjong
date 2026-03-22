
using Mahjong.System.TypeEventSystem;

public class EventSystemManager : MonoSingleton<EventSystemManager>
{
    // UI控制事件系统
    private readonly IEventSystem uiControlEventSystem = new TypeEventSystem();
    public IEventSystem UIControlEventSystem => uiControlEventSystem;
    // UI请求处理事件系统
    private readonly IEventSystem uiRequestEventSystem = new TypeEventSystem();
    public IEventSystem UIRequestEventSystem => uiRequestEventSystem;
    // 逻辑层事件系统
    private readonly IEventSystem modelEventSystem = new TypeEventSystem();
    public IEventSystem ModelEventSystem => modelEventSystem;
}