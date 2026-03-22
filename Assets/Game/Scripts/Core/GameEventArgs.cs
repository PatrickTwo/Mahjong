using System;

namespace Mahjong
{
    #region 游戏事件参数基类
    /// <summary>
    /// 游戏事件参数基类
    /// </summary>
    public abstract class GameEventArgs : EventArgs
    {
        public DateTime Timestamp { get; } = DateTime.Now;
    }
    #endregion

    #region 玩家操作事件
    /// <summary>
    /// 玩家操作事件
    /// </summary>
    public class PlayerActionEventArgs : GameEventArgs
    {
        public Player Player { get; set; }
        public PlayerAction Action { get; set; }
        public MahjongTile Tile { get; set; }
    }
    #endregion

    #region 胡牌事件
    /// <summary>
    /// 胡牌事件
    /// </summary>
    public class WinEventArgs : GameEventArgs
    {
        public Player Winner { get; set; }
        public WinMethod Method { get; set; }
        public MahjongTile WinningTile { get; set; }
        public ScoreResult Score { get; set; }
    }
    #endregion
}
