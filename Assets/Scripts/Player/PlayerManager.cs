using System;
using System.Collections.Generic;

namespace Mahjong
{
    #region 玩家管理器
    /// <summary>
    /// 玩家管理器
    /// </summary>
    public class PlayerManager
    {
        private List<Player> players = new List<Player>();

        public void InitializePlayers()
        {
            // 初始化4个玩家
            players.Clear();
            for (int i = 0; i < 4; i++)
            {
                players.Add(new Player(i, $"玩家{i + 1}"));
            }
        }

        public Player GetPlayer(int index)
        {
            if (index >= 0 && index < players.Count)
            {
                return players[index];
            }
            return null;
        }
    }
    #endregion
}
