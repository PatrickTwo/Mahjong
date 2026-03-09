using System;

namespace Mahjong
{
    #region 计分结果
    /// <summary>
    /// 计分结果
    /// </summary>
    public class ScoreResult
    {
        public Player Winner { get; set; }
        public Player Payer { get; set; }
        public int BaseScore { get; set; }
        public int Multiplier { get; set; }
        public int TotalScore { get; set; }
    }
    #endregion
}
