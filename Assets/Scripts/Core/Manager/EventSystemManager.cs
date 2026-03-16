
using Mahjong.System.TypeEventSystem;

public class EventSystemManager : MonoSingleton<EventSystemManager>
{
    // UI模块事件系统
    private readonly IEventSystem uiEventSystem = new TypeEventSystem();
    public IEventSystem UIEventSystem => uiEventSystem;
    // 逻辑层事件系统
    private readonly IEventSystem logicEventSystem = new TypeEventSystem();
    public IEventSystem LogicEventSystem => logicEventSystem;
}