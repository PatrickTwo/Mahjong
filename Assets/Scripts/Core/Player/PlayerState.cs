using Mahjong;

public interface IPlayerState : IBaseState<PlayerState> { }

public enum PlayerState
{
    Idle,           // 空闲
    Drawing,        // 摸牌
    Discarding,     // 切牌（打牌）
    DeclaringTing,  // 听牌
    DeclaringKong,  // 杠牌
    DeclaringWin,   // 胡牌
}