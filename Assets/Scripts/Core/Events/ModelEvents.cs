using Mahjong.System.TypeEventSystem;

namespace Mahjong
{
    /// <summary>
    /// 添加玩家事件
    /// </summary>
    public struct AddPlayerEvent : IEvent
    {
        public Player Player;
        public AddPlayerEvent(Player player)
        {
            Player = player;
        }
    }
    /// <summary>
    /// 移除玩家事件
    /// </summary>
    public struct RemovePlayerEvent : IEvent
    {
        public Player Player;
        public RemovePlayerEvent(Player player)
        {
            Player = player;
        }
    }
}