using System;

namespace Mahjong
{
    #region 胡牌方式枚举
    /// <summary>
    /// 胡牌方式枚举
    /// </summary>
    public enum WinMethod
    {
        SelfDraw,   // 自摸
        Discard,    // 点炮
        KongDraw,   // 杠上开花
        RobKong     // 抢杠胡
    }
    #endregion
}
