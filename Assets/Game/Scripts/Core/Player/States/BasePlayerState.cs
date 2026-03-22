using System;

namespace Mahjong
{
    #region 玩家状态基类
    /// <summary>
    /// 玩家状态基类
    /// </summary>
    public abstract class BasePlayerState : IPlayerState
    {
        protected Player Player { get; }

        public abstract PlayerState StateType { get; }

        protected BasePlayerState(Player player)
        {
            Player = player ?? throw new ArgumentNullException(nameof(player));
        }

        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void Update() { }
        public abstract bool CanTransitionTo(PlayerState nextState);
    }
    #endregion
}
