using System;
using Mahjong;
using Mahjong.Core.UI;

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
        HLogger.Log("点击了开始游戏按钮");
    }
    // 点击游戏设置按钮
    public void OnGameSettingButtonClick()
    {
        UIEventSystem.Send(new ShowPanelEvent(PanelIDConst.GameSettingPanelID));
    }
    // 点击加入房间按钮
    public void OnJoinRoomButtonClick()
    {
        UIEventSystem.Send(new ShowPanelEvent(PanelIDConst.JoinRoomPanelID));
    }

    // 麦克风切换状态改变事件
    internal void OnMicToggleValueChanged(bool arg0)
    {
        HLogger.Log("麦克风切换状态：" + arg0);
    }
    // 扬声器切换状态改变事件
    internal void OnSpeakerToggleValueChanged(bool arg0)
    {
        HLogger.Log("扬声器切换状态：" + arg0);
    }
    // 点击对局玩法设置按钮
    public void OnPlaySettingButtonClick()
    {
        UIEventSystem.Send(new ShowPanelEvent(PanelIDConst.PlaySettingPanelID));
    }
}
