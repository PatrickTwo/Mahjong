using System;
using System.Collections.Generic;
using System.Linq;

namespace Mahjong
{
    /// <summary>
    /// 庄家选择服务。
    /// 流程如下：
    /// 1. 调用 StartSelection 开始庄家选择流程。
    /// 2. 按玩家顺序通知当前轮到的玩家掷骰子。
    /// 3. 玩家完成掷骰后，服务记录该玩家的掷骰结果，并通知外部本次结果。
    /// 4. 若仍有未掷骰的玩家，则继续通知下一位玩家。
    /// 5. 当所有玩家都完成掷骰后，比较点数并筛选最高点数玩家。
    /// 6. 若最高点数唯一，则确定该玩家为庄家。
    /// 7. 若最高点数出现平局，则仅对平局玩家重新执行庄家选择流程，直到产生唯一庄家。
    /// 8. 庄家确定后，写入庄家状态并通知外部庄家选择完成。
    /// </summary>
    public class BankerSelectionService
    {
        #region 字段
        private readonly List<Player> allPlayers;
        private readonly Dictionary<Player, int> playerDiceResults = new Dictionary<Player, int>();
        private readonly Action<Player> onPlayerTurnToRoll;
        private readonly Action<Player, int> onPlayerDiceRolled;
        private readonly Action<Player> onBankerSelected;

        private List<Player> currentRoundPlayers;
        private int currentRollingPlayerIndex;
        private Player currentRollingPlayer;
        #endregion

        #region 构造函数
        public BankerSelectionService(
            List<Player> players,
            Action<Player> onPlayerTurnToRoll,
            Action<Player, int> onPlayerDiceRolled,
            Action<Player> onBankerSelected)
        {
            if (players == null)
            {
                throw new ArgumentNullException(nameof(players));
            }

            if (players.Count == 0)
            {
                throw new ArgumentException("players cannot be empty.", nameof(players));
            }

            allPlayers = players.ToList();
            currentRoundPlayers = allPlayers.ToList();
            this.onPlayerTurnToRoll = onPlayerTurnToRoll;
            this.onPlayerDiceRolled = onPlayerDiceRolled;
            this.onBankerSelected = onBankerSelected;
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 开始庄家选择流程。
        /// </summary>
        public void StartSelection()
        {
            HLogger.Log("Start banker selection.");

            ResetDealerFlag();
            StartRound(allPlayers);
        }

        /// <summary>
        /// 处理指定玩家的掷骰请求。
        /// </summary>
        /// <param name="player">请求掷骰的玩家。</param>
        public void RollDiceForPlayer(Player player)
        {
            if (player == null)
            {
                HLogger.LogFail("掷骰子失败: 玩家为 null");
                return;
            }

            if (currentRollingPlayer == null)
            {
                HLogger.LogFail("掷骰子失败: 当前轮到的玩家为空");
                return;
            }

            if (player != currentRollingPlayer)
            {
                HLogger.LogFail($"掷骰子失败: 期望 {currentRollingPlayer.Info.PlayerName}, 实际 {player.Info.PlayerName}");
                return;
            }

            if (playerDiceResults.ContainsKey(player))
            {
                HLogger.LogFail($"掷骰子失败: {player.Info.PlayerName} 已掷骰");
                return;
            }

            int diceResult = Dice.RollSingle();
            playerDiceResults[player] = diceResult;

            HLogger.Log($"玩家 {player.Info.PlayerName} 掷骰 {diceResult}");
            onPlayerDiceRolled?.Invoke(player, diceResult);

            currentRollingPlayerIndex++;
            currentRollingPlayer = null;

            NotifyCurrentPlayerToRoll();
        }
        #endregion

        #region 回合流程
        /// <summary>
        /// 开始一轮候选玩家的庄家选择流程。
        /// </summary>
        /// <param name="roundPlayers">当前轮次的候选玩家。</param>
        private void StartRound(List<Player> roundPlayers)
        {
            currentRoundPlayers = roundPlayers.ToList();
            playerDiceResults.Clear();
            currentRollingPlayerIndex = 0;
            currentRollingPlayer = null;

            NotifyCurrentPlayerToRoll();
        }

        /// <summary>
        /// 通知当前轮到的玩家掷骰子。
        /// </summary>
        private void NotifyCurrentPlayerToRoll()
        {
            if (currentRollingPlayerIndex >= currentRoundPlayers.Count)
            {
                SelectBankerFromCurrentRound();
                return;
            }

            currentRollingPlayer = currentRoundPlayers[currentRollingPlayerIndex];
            HLogger.Log($"通知 {currentRollingPlayer.Info.PlayerName} 掷骰子");

            onPlayerTurnToRoll?.Invoke(currentRollingPlayer);
        }
        #endregion

        #region 庄家选择
        /// <summary>
        /// 从当前轮结果中选择庄家。
        /// </summary>
        private void SelectBankerFromCurrentRound()
        {
            if (playerDiceResults.Count == 0)
            {
                HLogger.LogFail("Banker selection failed: no dice results in current round.");
                return;
            }

            int maxDiceResult = playerDiceResults.Values.Max();
            List<Player> potentialBankers = playerDiceResults
                .Where(pair => pair.Value == maxDiceResult)
                .Select(pair => pair.Key)
                .ToList();

            if (potentialBankers.Count == 1)
            {
                FinalizeBanker(potentialBankers[0]);
                return;
            }

            HLogger.Log($"Tie detected. {potentialBankers.Count} players rolled max value {maxDiceResult}.");
            StartRound(potentialBankers);
        }

        /// <summary>
        /// 确定庄家并通知外部。
        /// </summary>
        /// <param name="banker">最终庄家。</param>
        private void FinalizeBanker(Player banker)
        {
            if (banker == null)
            {
                HLogger.LogFail("Banker selection failed: banker is null.");
                return;
            }

            banker.IsDealer = true;

            HLogger.LogSuccess($"Banker selected: {banker.Info.PlayerName}.");
            onBankerSelected?.Invoke(banker);
        }
        #endregion

        #region 辅助方法
        /// <summary>
        /// 重置所有玩家的庄家标志。
        /// </summary>
        private void ResetDealerFlag()
        {
            foreach (Player player in allPlayers)
            {
                player.IsDealer = false;
            }
        }
        #endregion
    }
}
