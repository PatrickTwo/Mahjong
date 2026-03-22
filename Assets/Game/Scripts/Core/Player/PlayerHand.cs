using System;
using System.Collections.Generic;
using System.Linq;

namespace Mahjong
{
    #region 玩家手牌管理类
    /// <summary>
    /// 玩家手牌管理类
    /// </summary>
    public class PlayerHand
    {
        public List<MahjongTile> Tiles { get; private set; } = new(); // 玩家手牌列表

        public bool IsTing { get; private set; } = false; // 是否已听牌
        public List<MahjongTile> TingTiles { get; private set; } = new(); // 听的牌

        public void AddTile(MahjongTile tile)
        {
            Tiles.Add(tile);
            SortTiles();
        }

        public void DiscardTile(MahjongTile tile)
        {
            Tiles.Remove(tile);
        }
        // 玩家听牌
        public void DeclareTing(List<MahjongTile> tingTiles)
        {
            IsTing = true;
            TingTiles = tingTiles;
        }
        // 对玩家手牌进行排序
        private void SortTiles()
        {
            Tiles = Tiles.OrderBy(t => t.Suit)
                         .ThenBy(t => t.Value)
                         .ToList();
        }
    }
    #endregion
}
