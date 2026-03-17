using System;
using Mahjong;
using Mahjong.Core.UI;

/// <summary>
/// 大厅UI请求处理类
/// </summary>
public class LobbyUIRequestHandler : BaseUIRequestHandler
{
    private void Awake()
    {
        // 注册事件
        UIRequestEventSystem.AddListener<OnJoinRoomButtonClick>(OnJoinRoomButtonClick);
        UIRequestEventSystem.AddListener<OnStartGameButtonClick>(OnStartGameButtonClick);
        UIRequestEventSystem.AddListener<OnGameSettingButtonClick>(OnGameSettingButtonClick);
        UIRequestEventSystem.AddListener<OnPlaySettingButtonClick>(OnPlaySettingButtonClick);
        UIRequestEventSystem.AddListener<OnMicToggleValueChanged>((e)=>OnMicToggleValueChanged(e.isOn));
        UIRequestEventSystem.AddListener<OnSpeakerToggleValueChanged>((e)=>OnSpeakerToggleValueChanged(e.isOn));
        UIRequestEventSystem.AddListener<OnJoinButtonClick>(OnJoinButtonClick);
    }
    /// <summary>
    /// 1.验证玩家人数
    /// 2.验证玩家是否准备
    /// 3.如果所有玩家都准备好，开始游戏
    /// </summary>
    private void OnStartGameButtonClick()
    {
        // TODO 验证

        // 进入游戏流程
        HLogger.Log("点击了开始游戏按钮");
        FlowController.TransitionToState(GameState.BankerSelection);

    }
    // 点击游戏设置按钮
    private void OnGameSettingButtonClick()
    {
        UIEventSystem.Send(new ShowPanelEvent(PanelIDConst.GameSettingPanelID));
    }
    // 点击加入房间按钮
    private void OnJoinRoomButtonClick()
    {
        UIEventSystem.Send(new ShowPanelEvent(PanelIDConst.JoinRoomPanelID));
    }

    // 麦克风切换状态改变事件
    private void OnMicToggleValueChanged(bool arg0)
    {
        HLogger.Log("麦克风切换状态：" + arg0);
    }
    // 扬声器切换状态改变事件
    private void OnSpeakerToggleValueChanged(bool arg0)
    {
        HLogger.Log("扬声器切换状态：" + arg0);
    }
    // 点击对局玩法设置按钮
    private void OnPlaySettingButtonClick()
    {
        UIEventSystem.Send(new ShowPanelEvent(PanelIDConst.PlaySettingPanelID));
    }
    // 加入房间面板中点击 加入 按钮事件
    private void OnJoinButtonClick(OnJoinButtonClick e)
    {
        HLogger.Log("点击了加入房间按钮，房间ID：" + e.RoomID);
    }
}
