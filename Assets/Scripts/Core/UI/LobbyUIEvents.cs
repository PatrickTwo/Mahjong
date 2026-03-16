// 点击游戏设置按钮事件
using Mahjong.System.TypeEventSystem;

public struct GameSettingButtonClickEvent : IEvent { }

// 麦克风切换状态改变事件
public struct MicToggleValueChangedEvent : IEvent
{
    public bool IsOn;
}

// 点击对局玩法设置按钮事件
public struct PlaySettingButtonClickEvent : IEvent { }

// 扬声器切换状态改变事件
public struct SpeakerToggleValueChangedEvent : IEvent
{
    public bool IsOn;
}
