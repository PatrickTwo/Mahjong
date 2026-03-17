using System;

namespace Mahjong
{
    #region 游戏进行状态
    /// <summary>
    /// 游戏进行状态
    /// </summary>
    public class PlayingState : BaseGameState, IPlayerActionHandler
    {
        public override GameState StateType => GameState.Playing;

        public PlayingState(GameFlowController controller) : base(controller) { }

        public override void Enter()
        {
            // TODO: 开始第一个玩家的回合
        }

        public override void Update()
        {
            // TODO: 检查游戏结束条件
        }

        public override bool CanTransitionTo(GameState nextState) =>
            nextState == GameState.TingDeclared || nextState == GameState.Win ||
            nextState == GameState.Draw || nextState == GameState.Ended;

        public void HandlePlayerAction(Player player, PlayerAction action, MahjongTile tile)
        {
            // TODO: 处理玩家操作
        }

        private void StartNextPlayerTurn()
        {
            // TODO: 实现玩家回合开始逻辑
        }

        private void HandleDrawTile(Player player)
        {
            // TODO: 处理摸牌逻辑
        }

        private void HandleDiscardTile(Player player, MahjongTile tile)
        {
            // TODO: 处理打牌逻辑
        }

        private void HandleDeclareTing(Player player)
        {
            // TODO: 处理听牌逻辑
        }

        private void HandleWin(Player player, MahjongTile tile)
        {
            // TODO: 处理胡牌逻辑
        }

        private void CheckOtherPlayersReactions(Player currentPlayer, MahjongTile discardedTile)
        {
            // TODO: 实现其他玩家对打出的牌的反应逻辑
        }

        private void CheckGameEndConditions()
        {
            // TODO: 检查游戏结束条件
        }
    }
    #endregion
}