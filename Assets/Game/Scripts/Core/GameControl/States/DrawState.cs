namespace Mahjong
{
    public class DrawState : BaseGameState
    {
        public override GameState StateType => GameState.Draw;

        public DrawState(GameFlowController controller, IEventBusService eventBusService) : base(controller, eventBusService) { }

        public override void Enter()
        {
            // TODO: 处理流局逻辑
            // TODO: 切换到结束状态
        }

        private void HandleDraw()
        {
            // TODO: 实现流局处理逻辑
        }

        public override bool CanTransitionTo(GameState nextState)
        {
            throw new global::System.NotImplementedException();
        }

    }
}
