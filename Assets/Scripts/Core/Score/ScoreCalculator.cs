using System;


namespace Mahjong
{
    #region 计分器
    /// <summary>
    /// 计分器
    /// </summary>
    public class ScoreCalculator
    {
        private const int BaseScore = 1; // 底分

        public ScoreResult CalculateScore(WinPattern pattern, WinMethod method, Player winner, Player payer)
        {
            int multiplier = CalculateMultiplier(pattern, method);
            int totalScore = BaseScore * multiplier;

            return new ScoreResult
            {
                Winner = winner,
                Payer = payer,
                BaseScore = BaseScore,
                Multiplier = multiplier,
                TotalScore = totalScore
            };
        }

        private int CalculateMultiplier(WinPattern pattern, WinMethod method)
        {
            int multiplier = 1;

            // 清一色加倍
            if (pattern.IsPureSuit) multiplier *= 2;

            // 杠上开花加倍
            if (method == WinMethod.KongDraw) multiplier *= 2;

            // 自摸加倍
            if (method == WinMethod.SelfDraw) multiplier *= 2;

            // 风头/将头组合加倍
            if (pattern.HasWindHead) multiplier *= 2;
            if (pattern.HasDragonHead) multiplier *= 2;

            // 断风将加倍
            if (pattern.SpecialCount == 0) multiplier *= 2;

            return multiplier;
        }
    }
    #endregion
}
