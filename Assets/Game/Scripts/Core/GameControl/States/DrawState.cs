namespace Mahjong
{
    public class DrawState : BaseGameState
    {
        public override GameState StateType => GameState.Draw;

        public DrawState(GameFlowController controller) : base(controller) { }

        public override void Enter()
        {
            // TODO: 处理流局逻辑
            // TODO: 切换到结束状态
        }

        private void HandleDraw()
        {
            // TODO: 实现流局处理逻辑
        }
    }
}
