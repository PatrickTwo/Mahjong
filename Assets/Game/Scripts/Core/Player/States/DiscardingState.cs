using System;

namespace Mahjong
{
    #region 切牌状态
    /// <summary>
    /// 切牌（打牌）状态
    /// </summary>
    public class DiscardingState : BasePlayerState
    {
        public override PlayerState StateType => PlayerState.Discarding;

        public DiscardingState(Player player) : base(player) { }

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
