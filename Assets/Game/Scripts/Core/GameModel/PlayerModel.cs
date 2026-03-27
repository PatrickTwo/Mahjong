using System.Collections.Generic;
using Mahjong.System.TypeEventSystem;

namespace Mahjong
{
    /// <summary>
    /// 存储玩家数据
    /// </summary>
    public class PlayerModel
    {
        private const int MAX_PLAYER_COUNT = GameSettingModel.RequiredPlayerCount;
        private readonly List<Player> players = new();
        public List<Player> Players => players;
        private readonly IEventSystem modelEventSystem;

        public int PlayerCount => players.Count;

        public PlayerModel(IEventSystem modelEventSystem)
        {
            this.modelEventSystem = modelEventSystem;
        }
        /// <summary>
        /// 尝试添加玩家
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public bool TryAddPlayer(Player player)
        {
            if (players.Contains(player))
            {
                HLogger.LogFail($"玩家{player.Info.PlayerId}已存在，不可重复添加");
                return false;
            }
            if (players.Count >= MAX_PLAYER_COUNT)
            {
                HLogger.LogFail($"玩家{player.Info.PlayerId}已存在，不可重复添加");
                return false;
            }
            players.Add(player);
            modelEventSystem.Send(new AddPlayerEvent(player));
            return true;
        }
        public bool TryRemovePlayer(int playerId)
        {
            if (!TryGetPlayer(playerId, out Player player))
            {
                HLogger.Log($"玩家 {playerId} 不存在");
                return false;
            }
            return TryRemovePlayer(player);
        }
        /// <summary>
        /// 尝试移除玩家
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public bool TryRemovePlayer(Player player)
        {
            if (!players.Contains(player))
            {
                HLogger.LogFail($"玩家{player.Info.PlayerId}不存在，不可移除");
                return false;
            }
            if (players.Count <= 0)
            {
                HLogger.LogFail($"玩家{player.Info.PlayerId}不存在，不可移除");
                return false;
            }
            players.Remove(player);
            modelEventSystem.Send(new RemovePlayerEvent(player));
            return true;
        }
        /// <summary>
        /// 尝试获取玩家
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public bool TryGetPlayer(int playerId, out Player player)
        {
            player = players.Find(player => player.Info.PlayerId == playerId);
            return player != null;
        }

    }
}