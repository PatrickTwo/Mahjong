using System;

namespace Mahjong
{
    #region 游戏规则配置类
    /// <summary>
    /// 游戏规则配置类
    /// </summary>
    public class GameRules
    {
        public int DeadWallSize { get; set; } = 20;
        public int BaseScore { get; set; } = 1;
        public bool AllowRobKong { get; set; } = true;
        public bool AllowMultipleWinners { get; set; } = false;
        public int MaxRounds { get; set; } = 4;
    }
    #endregion

    #region 规则引擎接口
    /// <summary>
    /// 规则引擎接口
    /// </summary>
    public interface IRuleEngine
    {
        bool ValidateAction(Player player, PlayerAction action, MahjongTile tile);
        bool CheckWinConditions(PlayerHand hand, MahjongTile winningTile);
        int CalculateScore(WinPattern pattern, WinMethod method);
    }
    #endregion

    #region AI玩家接口
    /// <summary>
    /// AI玩家接口
    /// </summary>
    public interface IAIStrategy
    {
        PlayerAction DecideAction(Player player, GameState state);
        MahjongTile ChooseDiscard(PlayerHand hand);
    }
    #endregion
}
