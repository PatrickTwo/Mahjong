
using System;
using Mahjong;

/// <summary>
/// 大厅UI请求处理类
/// </summary>
public class LobbyUIRequestHandler : UIRequestHandler
{
    /// <summary>
    /// 1.验证玩家人数
    /// 2.验证玩家是否准备
    /// 3.如果所有玩家都准备好，开始游戏
    /// </summary>
    public void OnStartGameButtonClick()
    {
        // TODO 验证

        // 进入游戏流程
        GameManager.FlowController.TransitionToState(GameState.StartGame);
    }
}
