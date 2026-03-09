using System;
using System.Collections.Generic;
using System.Linq;

namespace Mahjong
{
    /// <summary>
    /// 牌池管理类
    /// </summary>
    public class TilePool
    {
        private Stack<MahjongTile> wall = new();
        private const int DeadWallSize = 20; // 黄堆大小

        public int RemainingTiles => wall.Count;
        public bool IsDeadWallReached => wall.Count <= DeadWallSize;

        /// <summary>
        /// 初始化牌池，创建并洗牌
        /// </summary>
        public void Initialize()
        {
            // 创建136张牌
            List<MahjongTile> allTiles = CreateAllTiles();

            // 洗牌
            List<MahjongTile> shuffled = allTiles.OrderBy(x => Guid.NewGuid()).ToList();

            wall = new Stack<MahjongTile>(shuffled);
        }

        /// <summary>
        /// 从牌池中取出指定数量的牌
        /// </summary>
        /// <param name="count">要取出的牌数</param>
        /// <returns>取出的牌列表，如果牌数不足则返回空列表</returns>
        public List<MahjongTile> DrawTiles(int count)
        {
            List<MahjongTile> drawnTiles = new();
            for (int i = 0; i < count && wall.Count > 0; i++)
            {
                drawnTiles.Add(wall.Pop());
            }
            return drawnTiles;
        }

        /// <summary>
        /// 从黄堆中摸一张牌
        /// </summary>
        /// <returns>摸到的牌，如果没有牌则返回null</returns>
        public MahjongTile DrawFromDeadWall()
        {
            // 从牌尾摸牌（杠牌时使用）
            return wall.Count > DeadWallSize ? wall.Pop() : null;
        }
        #region 牌池创建
        /// <summary>
        /// 创建所有的麻将牌
        /// </summary>
        /// <returns>包含136张麻将牌的列表</returns>
        private List<MahjongTile> CreateAllTiles()
        {
            List<MahjongTile> tiles = new List<MahjongTile>();

            // 创建饼、万、条牌（各36张）
            CreateSuitTiles(SuitType.Dot, tiles);
            CreateSuitTiles(SuitType.Bamboo, tiles);
            CreateSuitTiles(SuitType.Character, tiles);

            // 创建风牌（16张）
            CreateWindTiles(tiles);

            // 创建将牌（12张）
            CreateDragonTiles(tiles);

            return tiles;
        }

        /// <summary>
        /// 创建一套麻将牌（饼、万、条）
        /// </summary>
        /// <param name="suit">牌的花色类型</param>
        /// <param name="tiles">存储所有牌的列表</param>
        private void CreateSuitTiles(SuitType suit, List<MahjongTile> tiles)
        {
            TileType[] dotTypes = { TileType.Dot1, TileType.Dot2, TileType.Dot3, TileType.Dot4, TileType.Dot5, TileType.Dot6, TileType.Dot7, TileType.Dot8, TileType.Dot9 };
            TileType[] bambooTypes = { TileType.Bamboo1, TileType.Bamboo2, TileType.Bamboo3, TileType.Bamboo4, TileType.Bamboo5, TileType.Bamboo6, TileType.Bamboo7, TileType.Bamboo8, TileType.Bamboo9 };
            TileType[] characterTypes = { TileType.Character1, TileType.Character2, TileType.Character3, TileType.Character4, TileType.Character5, TileType.Character6, TileType.Character7, TileType.Character8, TileType.Character9 };

            TileType[] types = null;
            switch (suit)
            {
                case SuitType.Dot:
                    types = dotTypes;
                    break;
                case SuitType.Bamboo:
                    types = bambooTypes;
                    break;
                case SuitType.Character:
                    types = characterTypes;
                    break;
            }

            if (types != null)
            {
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        tiles.Add(new MahjongTile(types[i], suit, i + 1));
                    }
                }
            }
        }

        /// <summary>
        /// 创建风牌
        /// </summary>
        /// <param name="tiles">存储所有牌的列表</param>
        private void CreateWindTiles(List<MahjongTile> tiles)
        {
            TileType[] windTypes = { TileType.EastWind, TileType.SouthWind, TileType.WestWind, TileType.NorthWind };
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    tiles.Add(new MahjongTile(windTypes[i], SuitType.Wind, i + 1));
                }
            }
        }

        /// <summary>
        /// 创建将牌
        /// </summary>
        /// <param name="tiles">存储所有牌的列表</param>
        private void CreateDragonTiles(List<MahjongTile> tiles)
        {
            TileType[] dragonTypes = { TileType.RedDragon, TileType.GreenDragon, TileType.WhiteDragon };
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    tiles.Add(new MahjongTile(dragonTypes[i], SuitType.Dragon, i + 1));
                }
            }
        }
    }
    #endregion
}