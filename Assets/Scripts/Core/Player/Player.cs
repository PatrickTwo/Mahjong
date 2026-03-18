using System.Collections.Generic;
using UnityEngine;

namespace Mahjong
{
    /// <summary>
    /// 玩家类
    /// </summary>
    public class Player : MonoBehaviour
    {
        private PlayerInfo info;
        public PlayerInfo Info => info;
        public PlayerHand handTiles;// 玩家手牌
        public int Score { get; private set; }
        public bool IsDealer { get; set; } // 是否是庄家

        private IPlayerState currentState;

        public void Init(PlayerInfo info)
        {
            this.info = info;
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
        #region 重写操作符
        // 相等运算符
        public static bool operator ==(Player a, Player b)
        {
            return a.info.PlayerId == b.info.PlayerId;
        }
        // 不相等运算符
        public static bool operator !=(Player a, Player b)
        {
            return !(a == b);
        }
        // 重写==和!=运算符时通常需要重写下面两个方法
        // Equals()用于方法调用（如 player1.Equals(player2) ）
        //GetHashCode()用于哈希表（Dictionary、HashSet 等）的键查找
        public override bool Equals(object obj)
        {
            if (obj is Player other)
            {
                return this.info.PlayerId == other.info.PlayerId;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return info.PlayerId.GetHashCode();
        }
        #endregion
    }
}
