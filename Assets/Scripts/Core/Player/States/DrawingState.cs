using System;

namespace Mahjong
{
    #region 摸牌状态
    /// <summary>
    /// 摸牌状态
    /// </summary>
    public class DrawingState : BasePlayerState
    {
        public override PlayerState StateType => PlayerState.Drawing;

        public DrawingState(Player player) : base(player) { }

        public override void Enter()
        {
        }

        public override void Exit()
        {
        }

        public override void Update()
        {
        }

        public override bool CanTransitionTo(PlayerState nextState) => true;
    }
    #endregion
}
