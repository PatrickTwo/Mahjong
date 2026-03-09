using System;

namespace Mahjong
{
    #region 牌类型枚举
    /// <summary>
    /// 麻将牌类型枚举
    /// </summary>
    public enum TileType
    {
        // 饼牌
        Dot1, Dot2, Dot3, Dot4, Dot5, Dot6, Dot7, Dot8, Dot9,

        // 万牌
        Bamboo1, Bamboo2, Bamboo3, Bamboo4, Bamboo5, Bamboo6, Bamboo7, Bamboo8, Bamboo9,

        // 条牌
        Character1, Character2, Character3, Character4, Character5, Character6, Character7, Character8, Character9,

        // 风牌
        EastWind, SouthWind, WestWind, NorthWind,

        // 将牌
        RedDragon, GreenDragon, WhiteDragon
    }
    #endregion
}
