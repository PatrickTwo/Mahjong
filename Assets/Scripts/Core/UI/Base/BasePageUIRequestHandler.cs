using Mahjong;
using Mahjong.System.TypeEventSystem;

public abstract class BasePageUIRequestHandler
{
    protected IEventSystem UIEventSystem => EventSystemManager.Instance.UIEventSystem;
}
