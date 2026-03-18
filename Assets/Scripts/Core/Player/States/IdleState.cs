using System;

namespace Mahjong
{
    #region 空闲状态
    /// <summary>
    /// 空闲状态
    /// </summary>
    public class IdleState : BasePlayerState
    {
        public override PlayerState StateType => PlayerState.Idle;

        public IdleState(Player player) : base(player) { }

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
