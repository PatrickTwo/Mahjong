using Mahjong.System.TypeEventSystem;

namespace Mahjong
{
    // 点击开始游戏按钮事件
    public struct OnStartGameButtonClick : IEvent { }
    // 点击游戏玩法设置按钮事件
    public struct OnPlaySettingButtonClick : IEvent { }
    // 点击游戏设置按钮事件
    public struct OnGameSettingButtonClick : IEvent { }
    // 点击加入房间按钮事件
    public struct OnJoinRoomButtonClick : IEvent { }
    // 麦克风切换状态改变事件
    public struct OnMicToggleValueChanged : IEvent
    {
        public bool isOn;
        public OnMicToggleValueChanged(bool isOn)
        {
            this.isOn = isOn;
        }
    }
    // 扬声器切换状态改变事件
    public struct OnSpeakerToggleValueChanged : IEvent
    {
        public bool isOn;
        public OnSpeakerToggleValueChanged(bool isOn)
        {
            this.isOn = isOn;
        }
    }

    // 加入房间面板中点击 加入 按钮事件
    public struct OnJoinButtonClick : IEvent
    {
        public string RoomID;
        public OnJoinButtonClick(string roomID)
        {
            RoomID = roomID;
        }
    }
}
