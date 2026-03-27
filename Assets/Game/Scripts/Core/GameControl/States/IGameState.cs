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
        // 事件系统
        protected readonly IEventBusService eventBusService;

        public abstract GameState StateType { get; }

        protected BaseGameState(GameFlowController controller, IEventBusService eventBusService)
        {
            Controller = controller ?? throw new ArgumentNullException(nameof(controller));
            this.eventBusService = eventBusService ?? throw new ArgumentNullException(nameof(eventBusService));
        }

        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void Update() { }

        public abstract bool CanTransitionTo(GameState nextState);
    }
}
