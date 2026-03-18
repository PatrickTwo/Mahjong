using Mahjong.System.TypeEventSystem;

namespace Mahjong
{
    /// <summary>
    /// 玩家轮到掷骰子事件
    /// </summary>
    public struct PlayerTurnToRollEvent : IEvent
    {
        public Player Player;
        
        public PlayerTurnToRollEvent(Player player)
        {
            Player = player;
        }
    }

    /// <summary>
    /// 玩家掷骰子事件
    /// </summary>
    public struct PlayerDiceRolledEvent : IEvent
    {
        public Player Player;
        public int DiceResult;
        
        public PlayerDiceRolledEvent(Player player, int diceResult)
        {
            Player = player;
            DiceResult = diceResult;
        }
    }

    /// <summary>
    /// 庄家选择完成事件
    /// </summary>
    public struct BankerSelectedEvent : IEvent
    {
        public Player Banker;
        
        public BankerSelectedEvent(Player banker)
        {
            Banker = banker;
        }
    }

    /// <summary>
    /// 请求掷骰子事件（由UI触发）
    /// </summary>
    public struct RequestRollDiceEvent : IEvent
    {
        public Player Player;
        
        public RequestRollDiceEvent(Player player)
        {
            Player = player;
        }
    }
}