using System;

namespace Mahjong.GameControl.States
{
    #region 游戏结束状态
    /// <summary>
    /// 游戏结束状态
    /// </summary>
    public class EndedState : BaseGameState
    {
        public override GameState StateType => GameState.Ended;

        public EndedState(GameFlowController controller) : base(controller) { }

        public override void Enter()
        {
            // TODO: 显示游戏结果，准备下一局
        }

        public override bool CanTransitionTo(GameState nextState) => nextState == GameState.LobbyWaiting;

        private void ShowGameResults()
        {
            // TODO: 实现结果显示逻辑
        }
    }
    #endregion
}