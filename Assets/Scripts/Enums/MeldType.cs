using System;

namespace Mahjong
{
    #region 牌组类型枚举
    /// <summary>
    /// 牌组类型枚举
    /// </summary>
    public enum MeldType
    {
        Pung,       // 刻子
        Chow,       // 顺子
        Kong,       // 杠
        Pair,       // 对子
        Special     // 特殊组合（风头/将头）
    }
    #endregion
}
