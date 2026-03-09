using System;

namespace Mahjong.GameControl.States
{
    #region 初始化状态
    /// <summary>
    /// 初始化状态
    /// </summary>
    public class InitializingState : BaseGameState
    {
        public override GameState StateType => GameState.Initializing;

        public InitializingState(GameFlowController controller) : base(controller) { }

        public override void Enter()
        {
            // TODO: 初始化游戏组件
            // TODO: 转换到发牌状态
        }

        public override bool CanTransitionTo(GameState nextState) => nextState == GameState.Dealing;
    }
    #endregion
}