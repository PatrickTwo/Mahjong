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
        public int LobbyCardIndex { get; set; } // 大厅卡片索引
        public PlayerHand handTiles;// 玩家手牌
        public int Score { get; private set; }
        public bool IsDealer { get; set; } // 是否是庄家
        public PlayerPosition Position { get; set; } // 玩家位置

        private IPlayerState currentState;

        public void Init(PlayerInfo info, int cardIndex)
        {
            this.info = info;
            LobbyCardIndex = cardIndex;
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
        /// <summary>
        /// 相等运算符
        /// </summary>
        public static bool operator ==(Player a, Player b)
        {
            // 使用 ReferenceEquals 处理 null 情况
            // ReferenceEquals 比较的是引用地址，不是对象内容
            // 不会调用任何重载的 == 操作符（防止无限递归）
            // 对于值类型会装箱，结果总是 false
            if (ReferenceEquals(a, null))
            {
                return ReferenceEquals(b, null);
            }
            if (ReferenceEquals(b, null))
            {
                return false;
            }
            return a.info.PlayerId == b.info.PlayerId;
        }
        /// <summary>
        /// 不相等运算符
        /// </summary>
        public static bool operator !=(Player a, Player b)
        {
            return !(a == b);
        }
        /// <summary>
        /// 重写 Equals 方法
        /// 用于方法调用（如 player1.Equals(player2) ）
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj is Player other)
            {
                return this == other;
            }
            return false;
        }

        /// <summary>
        /// 重写 GetHashCode 方法
        /// 用于哈希表（Dictionary、HashSet 等）的键查找
        /// </summary>
        public override int GetHashCode()
        {
            // 防御性检查，避免 info 为 null 时出错
            return info?.PlayerId.GetHashCode() ?? 0;
        }
        #endregion
    }
}
