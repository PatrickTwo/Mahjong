using System;
using System.Collections.Generic;
using System.Linq;

namespace Mahjong
{
    #region 庄家选择服务类
    /// <summary>
    /// 庄家选择服务类
    /// 负责管理庄家选择流程
    /// </summary>
    public class BankerSelectionService
    {
        private readonly List<Player> players;
        private readonly Dictionary<Player, int> playerDiceResults = new();

        /// <summary>
        /// 庄家选择完成事件
        /// </summary>
        public event Action<Player> OnBankerSelected;

        /// <summary>
        /// 玩家掷骰子事件
        /// </summary>
        public event Action<Player, int> OnPlayerRolledDice;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="players">参与庄家选择的玩家列表</param>
        public BankerSelectionService(List<Player> players)
        {
            if (players == null || players.Count == 0)
                throw new ArgumentException("玩家列表不能为空");

            this.players = players.ToList();
        }

        /// <summary>
        /// 开始庄家选择流程
        /// </summary>
        public void StartSelection()
        {
            HLogger.Log("开始庄家选择流程");
            playerDiceResults.Clear();

            // 按玩家顺序依次掷骰子
            foreach (var player in players)
            {
                RollDiceForPlayer(player);
            }

            // 选择庄家
            SelectBanker();
        }

        /// <summary>
        /// 为指定玩家掷骰子
        /// </summary>
        /// <param name="player">玩家</param>
        private void RollDiceForPlayer(Player player)
        {
            int diceResult = Dice.RollSingle();
            playerDiceResults[player] = diceResult;

            HLogger.LogSuccess($"玩家 {player.Info.PlayerName} 掷出点数: {diceResult}");
            OnPlayerRolledDice?.Invoke(player, diceResult);
        }

        /// <summary>
        /// 选择庄家
        /// </summary>
        private void SelectBanker()
        {
            // 找出点数最大的玩家
            var maxDiceResult = playerDiceResults.Values.Max();
            var potentialBankers = playerDiceResults
                .Where(kv => kv.Value == maxDiceResult)
                .Select(kv => kv.Key)
                .ToList();

            Player banker;

            if (potentialBankers.Count == 1)
            {
                // 只有一个玩家点数最大
                banker = potentialBankers[0];
            }
            else
            {
                // 多个玩家点数相同，需要重新掷骰子
                HLogger.Log($"有{potentialBankers.Count}名玩家掷出相同点数{maxDiceResult}，需要重新掷骰子");
                banker = ResolveTie(potentialBankers);
            }

            // 设置庄家
            banker.IsDealer = true;
            HLogger.LogSuccess($"庄家选择完成: {banker.Info.PlayerName} 成为庄家");
            OnBankerSelected?.Invoke(banker);
        }

        /// <summary>
        /// 处理平局情况
        /// </summary>
        /// <param name="tiedPlayers">点数相同的玩家列表</param>
        /// <returns>最终庄家</returns>
        private Player ResolveTie(List<Player> tiedPlayers)
        {
            var tieService = new BankerSelectionService(tiedPlayers);
            Player winner = null;

            // 注册事件来获取最终胜者
            tieService.OnBankerSelected += banker => winner = banker;
            tieService.StartSelection();

            return winner;
        }

        /// <summary>
        /// 获取所有玩家的掷骰子结果
        /// </summary>
        /// <returns>玩家掷骰子结果字典</returns>
        public Dictionary<Player, int> GetDiceResults()
        {
            return new Dictionary<Player, int>(playerDiceResults);
        }
    }
    #endregion
}