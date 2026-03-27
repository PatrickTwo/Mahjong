using Mahjong.Core.UI;
using UnityEngine.SceneManagement;

namespace Mahjong
{
    public class LobbyWaitingState : BaseGameState
    {
        public override GameState StateType => GameState.LobbyWaiting;

        public LobbyWaitingState(GameFlowController controller, IEventBusService eventBusService) : base(controller, eventBusService) { }

        public override void Enter()
        {
            HLogger.LogSuccess($"进入 {StateType} 状态");
            // GameSceneManager.LoadGameScene(GameSceneManager.LobbyScene, LoadSceneMode.Single);
            // 进入时隐藏该页面所有面板
            eventBusService.UIControlEventSystem.Send(new HidePanelEvent(PanelIDConst.JoinRoomPanel));
            eventBusService.UIControlEventSystem.Send(new HidePanelEvent(PanelIDConst.PlayerOperationPanel));
            eventBusService.UIControlEventSystem.Send(new HidePanelEvent(PanelIDConst.PromptPanel));
            eventBusService.UIControlEventSystem.Send(new HidePanelEvent(PanelIDConst.GameSettingPanel));
            eventBusService.UIControlEventSystem.Send(new HidePanelEvent(PanelIDConst.PlaySettingPanel));
            eventBusService.UIControlEventSystem.Send(new HidePanelEvent(PanelIDConst.PlayerInfoPanel));
        }

        public override bool CanTransitionTo(GameState nextState)
        {
            return nextState == GameState.BankerSelection;
        }

    }
}
