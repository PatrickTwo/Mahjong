using Mahjong.System.TypeEventSystem;

namespace Mahjong
{
    /// <summary>
    /// 添加玩家事件
    /// </summary>
    public struct AddPlayerEvent : IEvent
    {
        public Player player;
        public AddPlayerEvent(Player player)
        {
            this.player = player;
        }
    }
    /// <summary>
    /// 移除玩家事件
    /// </summary>
    public struct RemovePlayerEvent : IEvent
    {
        public Player player;
        public RemovePlayerEvent(Player player)
        {
            this.player = player;
        }
    }
}