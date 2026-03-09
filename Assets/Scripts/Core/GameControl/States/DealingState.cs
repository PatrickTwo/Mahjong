using System;

namespace Mahjong.GameControl.States
{
    #region 发牌状态
    /// <summary>
    /// 发牌状态
    /// </summary>
    public class DealingState : BaseGameState
    {
        public override GameState StateType => GameState.Dealing;

        public DealingState(GameFlowController controller) : base(controller) { }

        public override void Enter()
        {
            // TODO: 执行发牌逻辑
            // TODO: 转换到游戏进行状态
        }

        public override bool CanTransitionTo(GameState nextState) => nextState == GameState.Playing;

        private void DealTiles()
        {
            // TODO: 实现发牌逻辑
        }
    }
    #endregion
}