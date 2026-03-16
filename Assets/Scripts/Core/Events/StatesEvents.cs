using Mahjong.System.TypeEventSystem;
using Mahjong.Core;
using Mahjong;
public struct EnterStateEvent : IEvent
{
    public GameState State;
    public EnterStateEvent(GameState state)
    {
        State = state;
    }
}
