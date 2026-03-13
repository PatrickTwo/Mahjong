using System;
using UnityEngine.SceneManagement;

namespace Mahjong.GameControl.States
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
            GameSceneManager.LoadGameScene(GameSceneManager.LobbyScene, LoadSceneMode.Single);
            // TODO: 初始化游戏组件
            // TODO: 转换到发牌状态
        }

        public override bool CanTransitionTo(GameState nextState) => nextState == GameState.Dealing;
    }
    #endregion
}