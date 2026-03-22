using System;

namespace Mahjong
{
    #region 杠牌状态
    /// <summary>
    /// 杠牌状态
    /// </summary>
    public class DeclaringKongState : BasePlayerState
    {
        public override PlayerState StateType => PlayerState.DeclaringKong;

        public DeclaringKongState(Player player) : base(player) { }

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
