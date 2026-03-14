
using System;
using Mahjong;

/// <summary>
/// 大厅UI请求处理类
/// </summary>
public class LobbyUIRequestHandler : BasePageUIRequestHandler
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

    internal void OnGameSettingButtonClick()
    {
        HLogger.Log("点击了游戏设置按钮");
    }

    internal void OnMicToggleValueChanged(bool arg0)
    {
        HLogger.Log("麦克风切换状态：" + arg0);
    }

    internal void OnPlaySettingButtonClick()
    {
        HLogger.Log("点击了播放设置按钮");
    }

    internal void OnSpeakerToggleValueChanged(bool arg0)
    {
        HLogger.Log("扬声器切换状态：" + arg0);
    }
}
