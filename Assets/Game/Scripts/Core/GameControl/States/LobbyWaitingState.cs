using UnityEngine.SceneManagement;

namespace Mahjong
{
    public class LobbyWaitingState : BaseGameState
    {
        public override GameState StateType => GameState.LobbyWaiting;

        public LobbyWaitingState(GameFlowController controller) : base(controller) { }

        public override void Enter()
        {
            HLogger.LogSuccess($"进入 {StateType} 状态");
            // GameSceneManager.LoadGameScene(GameSceneManager.LobbyScene, LoadSceneMode.Single);
        }
    }
}
