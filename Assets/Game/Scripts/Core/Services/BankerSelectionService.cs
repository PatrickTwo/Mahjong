using System;
using System.Collections.Generic;
using System.Linq;
using Mahjong.System.TypeEventSystem;

namespace Mahjong
{
    /// <summary>
    /// 庄家选择服务类
    /// 负责管理庄家选择流程
    /// 服务流程如下
    /// 1.调用StartSelection方法开始庄家选择流程
    /// 2.从第一个玩家开始，通知轮到其掷骰子
    /// 3.玩家处于掷骰子状态时，弹出掷骰子按钮，点击时触发RequestRollDice事件
    /// 4.接收玩家掷骰子事件，为其生成掷骰子结果，记录并发送PlayerDiceRolledEvent事件通知
    /// 5.依次通知每个玩家掷骰子，直到所有玩家都掷完骰子
    /// 6.根据掷骰子结果，选择点数最高的玩家作为庄家
    /// 7.触发庄家选择完成事件
    /// </summary>
    public class BankerSelectionService
    {
        private readonly List<Player> players; // 参与庄家选择的玩家列表，按照这个顺序进行掷骰子
        private readonly Dictionary<Player, int> playerDiceResults = new(); // 掷骰子结果
        private int curRollingPlayerIndex; // 当前掷骰子的玩家
        private Action<Player> OnBankerSelected;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="players">参与庄家选择的玩家列表</param>
        public BankerSelectionService(List<Player> players)
        {
            this.players = players.ToList();
        }
        #region 启动
        /// <summary>
        /// 开始庄家选择流程
        /// </summary>
        public void StartSelection()
        {
            HLogger.Log("开始庄家选择流程");
            playerDiceResults.Clear();

            // 从第一个玩家开始，列表中玩家的顺序应为东南西北
            curRollingPlayerIndex = 0;
            // 开始第一个玩家掷骰子
            StartNextPlayerRoll();
        }
        #endregion
        #region 掷骰子
        /// <summary>
        /// 开始下一个玩家掷骰子
        /// </summary>
        private void StartNextPlayerRoll()
        {
            if (curRollingPlayerIndex < players.Count)
            {
                // TODO 触发UI事件，显示当前玩家掷骰子界面
                
                curRollingPlayerIndex++;
            }
            else
            {
                // 所有玩家都掷完骰子，选择庄家
                SelectBanker();
            }
        }

        /// <summary>
        /// 为指定玩家掷骰子
        /// </summary>
        /// <param name="player">玩家</param>
        public void RollDiceForPlayer(Player player)
        {
            int diceResult = Dice.RollSingle();
            playerDiceResults[player] = diceResult;

            HLogger.LogSuccess($"玩家 {player.Info.PlayerName} 掷出点数: {diceResult}");

            StartNextPlayerRoll();
        }
        #endregion
        #region 选择庄家
        /// <summary>
        /// 选择庄家
        /// </summary>
        private void SelectBanker()
        {
            // 找出点数最大的玩家
            int maxDiceResult = playerDiceResults.Values.Max();
            List<Player> potentialBankers = playerDiceResults
                .Where(kv => kv.Value == maxDiceResult)
                .Select(kv => kv.Key)
                .ToList();

            Player banker;

            if (potentialBankers.Count == 1)
            {
                // 只有一个玩家点数最大
                banker = potentialBankers[0];
                OnBankerSelected?.Invoke(banker);
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
        }
        #endregion
        #region 平局处理
        /// <summary>
        /// 处理平局情况
        /// </summary>
        /// <param name="tiedPlayers">点数相同的玩家列表</param>
        /// <returns>最终庄家</returns>
        private Player ResolveTie(List<Player> tiedPlayers)
        {
            // 递归调用，处理平局情况
            var tieService = new BankerSelectionService(tiedPlayers);
            Player winner = null;

            // 注册事件来获取最终胜者
            tieService.OnBankerSelected += banker => winner = banker;
            tieService.StartSelection();

            return winner;
        }
        #endregion
    }
}