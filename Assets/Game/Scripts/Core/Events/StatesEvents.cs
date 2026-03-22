using Mahjong.System.TypeEventSystem;
using Mahjong.Core;
using Mahjong;
namespace Mahjong
{
    public struct EnterStateEvent : IEvent
    {
        public GameState State;
        public EnterStateEvent(GameState state)
        {
            State = state;
        }
    }
}
