using System;

namespace Mahjong
{
    #region 胡牌判定器
    /// <summary>
    /// 胡牌判定器
    /// </summary>
    public class WinChecker
    {
        public bool CanWin(PlayerHand hand, MahjongTile winningTile)
        {
            // 检查缺门规则
            if (!CheckMissingSuit(hand))
                return false;

            // 检查基本胡牌结构
            return CheckWinPattern(hand, winningTile);
        }

        private bool CheckMissingSuit(PlayerHand hand)
        {
            // 实现缺门检查逻辑
            // 玩家手中必须只有饼、万、条中的两种花色
            return true;
        }

        private bool CheckWinPattern(PlayerHand hand, MahjongTile winningTile)
        {
            // 实现胡牌牌型判定
            // n × AAA + m × ABC + x × (风头+任意两张风牌) + y × (将头+任意两张将牌) + s × DD
            return true;
        }

        public bool CanTing(PlayerHand hand)
        {
            // 检查是否可以听牌
            // 玩家只差最后一张牌即可胡牌
            return true;
        }
    }
    #endregion
}
