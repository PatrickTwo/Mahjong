using System;
using UnityEngine.SceneManagement;

namespace Mahjong
{
    #region 初始化状态
    /// <summary>
    /// 开始状态
    /// 游戏开始时进入LobbyScene
    /// </summary>
    public class LobbyWaitingState : BaseGameState
    {
        public override GameState StateType => GameState.LobbyWaiting;

        public LobbyWaitingState(GameFlowController controller) : base(controller) { }

        public override void Enter()
        {
            HLogger.LogSuccess($"进入了{StateType}状态");
            //GameSceneManager.LoadGameScene(GameSceneManager.LobbyScene, LoadSceneMode.Single);
        }

        public override bool CanTransitionTo(GameState targetState)
        {
            return targetState switch
            {
                GameState.BankerSelection => true,
                _ => false,
            };
        }
        #endregion
    }
}