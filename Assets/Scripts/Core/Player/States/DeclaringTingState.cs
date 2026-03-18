using System;

namespace Mahjong
{
    #region 听牌状态
    /// <summary>
    /// 听牌状态
    /// </summary>
    public class DeclaringTingState : BasePlayerState
    {
        public override PlayerState StateType => PlayerState.DeclaringTing;

        public DeclaringTingState(Player player) : base(player) { }

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
