namespace Mahjong
{
    public class WinState : BaseGameState
    {
        public override GameState StateType => GameState.Win;

        public WinState(GameFlowController controller) : base(controller) { }

        public override void Enter()
        {
            // TODO: 计算分数并显示结果
            // TODO: 切换到结束状态
        }

        private void CalculateScores()
        {
            // TODO: 实现分数计算逻辑
        }

        public override bool CanTransitionTo(GameState nextState)
        {
            throw new global::System.NotImplementedException();
        }

    }
}
