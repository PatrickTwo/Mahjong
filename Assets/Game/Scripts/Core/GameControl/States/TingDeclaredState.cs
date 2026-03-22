namespace Mahjong
{
    public class TingDeclaredState : BaseGameState, IPlayerActionHandler
    {
        public override GameState StateType => GameState.TingDeclared;

        public TingDeclaredState(GameFlowController controller) : base(controller) { }

        public override void Enter()
        {
            // TODO: 听牌状态的特殊初始化
        }

        public void HandlePlayerAction(Player player, PlayerAction action, MahjongTile tile)
        {
            // TODO: 听牌状态下的特殊处理
        }

        private void HandleTingDiscard(Player player, MahjongTile tile)
        {
            // TODO: 处理听牌状态下的打牌逻辑
        }

        private void HandleTingWin(Player player, MahjongTile tile)
        {
            // TODO: 处理听牌状态下的胡牌逻辑
        }

        private bool CanDiscardTile(Player player, MahjongTile tile)
        {
            // TODO: 检查听牌状态下是否可以打这张牌
            return true;
        }

        public override bool CanTransitionTo(GameState nextState)
        {
            throw new global::System.NotImplementedException();
        }

    }
}
