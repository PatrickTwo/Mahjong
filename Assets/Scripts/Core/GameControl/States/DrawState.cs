using System;

namespace Mahjong.GameControl.States
{
    #region 流局状态
    /// <summary>
    /// 流局状态
    /// </summary>
    public class DrawState : BaseGameState
    {
        public override GameState StateType => GameState.Draw;

        public DrawState(GameFlowController controller) : base(controller) { }

        public override void Enter()
        {
            // TODO: 处理流局逻辑
            // TODO: 转换到结束状态
        }

        public override bool CanTransitionTo(GameState nextState) => nextState == GameState.Ended;

        private void HandleDraw()
        {
            // TODO: 实现流局处理逻辑
        }
    }
    #endregion
}