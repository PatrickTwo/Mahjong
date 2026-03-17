using System;

namespace Mahjong
{
    #region 胡牌状态
    /// <summary>
    /// 胡牌状态
    /// </summary>
    public class WinState : BaseGameState
    {
        public override GameState StateType => GameState.Win;

        public WinState(GameFlowController controller) : base(controller) { }

        public override void Enter()
        {
            // TODO: 计算分数并显示结果
            // TODO: 转换到结束状态
        }

        public override bool CanTransitionTo(GameState nextState) => nextState == GameState.Ended;

        private void CalculateScores()
        {
            // TODO: 实现分数计算逻辑
        }
    }
    #endregion
}