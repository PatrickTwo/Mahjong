namespace Mahjong
{
    public class EndedState : BaseGameState
    {
        public override GameState StateType => GameState.Ended;

        public EndedState(GameFlowController controller) : base(controller) { }

        public override void Enter()
        {
            // TODO: 显示游戏结果，准备下一局
        }

        private void ShowGameResults()
        {
            // TODO: 实现结果显示逻辑
        }

        public override bool CanTransitionTo(GameState nextState)
        {
            throw new global::System.NotImplementedException();
        }

    }
}
