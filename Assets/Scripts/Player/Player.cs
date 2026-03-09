using System;
using System.Collections.Generic;

namespace Mahjong
{
    /// <summary>
    /// 玩家类
    /// </summary>
    public class Player
    {
        public int PlayerId { get; private set; }
        public string Name { get; private set; }
        public PlayerHand handTiles;// 玩家手牌
        public int Score { get; private set; }
        public bool IsDealer { get; set; } // 是否是庄家

        public Player(int playerId, string name)
        {
            PlayerId = playerId;
            Name = name;
            handTiles = new PlayerHand();
            Score = 0;
        }

        public void AddScore(int points)
        {
            Score += points;
        }

        public void SubtractScore(int points)
        {
            Score -= points;
        }
        #region 手牌相关
        /// <summary>
        /// 添加手牌
        /// </summary>
        /// <param name="tiles">要添加的手牌列表</param>
        public void AddTiles(List<MahjongTile> tiles)
        {
            foreach (var tile in tiles)
            {
                handTiles.AddTile(tile);
            }
        }
        /// <summary>
        /// 获取玩家手牌
        /// </summary>
        /// <returns>玩家手牌列表</returns>
        public List<MahjongTile> GetHandTiles()
        {
            return handTiles.Tiles;
        }
        #endregion
    }
}
