using System.Collections.Generic;

namespace Mahjong
{
    /// <summary>
    /// 存储玩家数据
    /// </summary>
    public class PlayerModel
    {
        private const int MAX_PLAYER_COUNT = 4;
        private readonly List<Player> Players = new();

        /// <summary>
        /// 尝试添加玩家
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public bool TryAddPlayer(Player player)
        {
            if (Players.Contains(player))
            {
                HLogger.LogFail($"玩家{player.PlayerId}已存在，不可重复添加");
                return false;
            }
            if (Players.Count >= MAX_PLAYER_COUNT)
            {
                HLogger.LogFail($"玩家{player.PlayerId}已存在，不可重复添加");
                return false;
            }
            Players.Add(player);
            return true;
        }
        /// <summary>
        /// 尝试移除玩家
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public bool TryRemovePlayer(Player player)
        {
            if (!Players.Contains(player))
            {
                HLogger.LogFail($"玩家{player.PlayerId}不存在，不可移除");
                return false;
            }
            if (Players.Count <= 0)
            {
                HLogger.LogFail($"玩家{player.PlayerId}不存在，不可移除");
                return false;
            }
            Players.Remove(player);
            return true;
        }
    }
}