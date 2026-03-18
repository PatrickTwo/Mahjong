using System;

namespace Mahjong
{
    #region 胡牌状态
    /// <summary>
    /// 胡牌状态
    /// </summary>
    public class DeclaringWinState : BasePlayerState
    {
        public override PlayerState StateType => PlayerState.DeclaringWin;

        public DeclaringWinState(Player player) : base(player) { }

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
