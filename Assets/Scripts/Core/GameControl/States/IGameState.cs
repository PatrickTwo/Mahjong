using System;

namespace Mahjong.GameControl.States
{
    #region 游戏状态接口
    /// <summary>
    /// 游戏状态接口
    /// </summary>
    public interface IGameState
    {
        GameState StateType { get; }
        void Enter();
        void Exit();
        void Update();
        bool CanTransitionTo(GameState nextState);
    }
    #endregion

    #region 玩家操作处理接口
    /// <summary>
    /// 玩家操作处理接口
    /// </summary>
    public interface IPlayerActionHandler
    {
        void HandlePlayerAction(Player player, PlayerAction action, MahjongTile tile);
    }
    #endregion

    #region 状态基类
    /// <summary>
    /// 状态基类
    /// </summary>
    public abstract class BaseGameState : IGameState
    {
        protected GameFlowController Controller { get; }
        
        public abstract GameState StateType { get; }

        protected BaseGameState(GameFlowController controller)
        {
            Controller = controller ?? throw new ArgumentNullException(nameof(controller));
        }

        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void Update() { }
        public abstract bool CanTransitionTo(GameState nextState);
    }
    #endregion
}