using System;

namespace Mahjong
{
    #region 胡牌牌型结果
    /// <summary>
    /// 胡牌牌型结果
    /// </summary>
    public class WinPattern
    {
        public int PungCount { get; set; }      // 刻子数量
        public int ChowCount { get; set; }      // 顺子数量
        public int SpecialCount { get; set; }   // 特殊组合数量
        public bool HasWindHead { get; set; }   // 是否有风头
        public bool HasDragonHead { get; set; } // 是否有将头
        public bool IsPureSuit { get; set; }    // 是否清一色
    }
    #endregion
}
