using System;

namespace Mahjong
{
    #region 玩家操作枚举
    /// <summary>
    /// 玩家操作枚举
    /// </summary>
    public enum PlayerAction
    {
        DrawTile,       // 摸牌
        DiscardTile,    // 打牌
        Pung,           // 碰牌
        Kong,           // 杠牌
        DeclareTing,    // 听牌
        Win,            // 胡牌
        Skip            // 跳过
    }
    #endregion
}
