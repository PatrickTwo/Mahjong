using System;

namespace Mahjong.GameControl.States
{
    #region 游戏状态接口
    /// <summary>
    /// 游戏状态接口
    /// </summary>
    public interface IGameState
    {
        /// <summary>
        /// 该状态的枚举
        /// </summary>
        GameState StateType { get; }
        void Enter();
        void Exit();
        void Update();
        /// <summary>
        /// 判断是否可以转换到下一个状态
        /// </summary>
        /// <param name="nextState">下一个状态</param>
        /// <returns>如果可以转换则返回true，否则返回false</returns>
        bool CanTransitionTo(GameState nextState);
    }
    #endregion

    #region 玩家操作处理接口
    /// <summary>
    /// 定义了处理玩家操作的能力
    /// 实现该接口的类可以处理玩家的输入操作，如出牌、摸牌、交牌等
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