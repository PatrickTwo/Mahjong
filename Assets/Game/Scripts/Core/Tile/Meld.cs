using System;
using System.Collections.Generic;

namespace Mahjong
{
    #region 牌组类
    /// <summary>
    /// 牌组类（刻子、顺子、对子等）
    /// </summary>
    public class Meld
    {
        public MeldType Type { get; private set; }
        public List<MahjongTile> Tiles { get; private set; }
        public bool IsConcealed { get; private set; } // 是否暗牌

        public Meld(MeldType type, List<MahjongTile> tiles, bool concealed = false)
        {
            Type = type;
            Tiles = tiles;
            IsConcealed = concealed;
        }
    }
    #endregion
}
