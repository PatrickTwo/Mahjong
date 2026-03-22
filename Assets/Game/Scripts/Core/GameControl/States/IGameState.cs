using System;

namespace Mahjong
{
    public interface IGameState : IBaseState<GameState> { }

    public interface IPlayerActionHandler
    {
        void HandlePlayerAction(Player player, PlayerAction action, MahjongTile tile);
    }

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
}
