namespace Mahjong
{
    public class DealingState : BaseGameState
    {
        public override GameState StateType => GameState.Dealing;

        public DealingState(GameFlowController controller) : base(controller) { }

        public override void Enter()
        {
            // TODO: 执行发牌逻辑
            // TODO: 切换到游戏进行状态
        }

        private void DealTiles()
        {
            // TODO: 实现发牌逻辑
        }

        public override bool CanTransitionTo(GameState nextState)
        {
            throw new global::System.NotImplementedException();
        }

    }
}
