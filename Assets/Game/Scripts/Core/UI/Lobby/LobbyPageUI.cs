using System;
using Mahjong;
using Mahjong.Core.UI;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 大厅页面UI类
/// </summary>
public class LobbyPageUI : BasePageUI
{
    [Header("Left Up")]
    [SerializeField] private Button startGameBtn; // 开始游戏按钮
    [SerializeField] private Button playSettingBtn; // 游戏玩法设置按钮
    [Header("Right Up")]
    [SerializeField] private Button gameSettingBtn; // 游戏设置按钮
    [SerializeField] private TextMeshProUGUI roomIDText; // 房间号显示文本
    [SerializeField] private Button joinRoomBtn; // 加入房间按钮
    [Header("Left Down")]
    [SerializeField] private ChatBoxViewUI chatBoxView; // 聊天框视图
    [SerializeField] private PlayerListViewUI playerListView; // 玩家列表视图
    [Header("Right Down")]
    [SerializeField] private Toggle micTog; // 麦克风开关
    [SerializeField] private Toggle speakerTog; // 扬声器开关

    protected override void Awake()
    {
        base.Awake();
        EventSystemManager.Instance.ModelEventSystem.AddListener<EnterStateEvent>((e) => OnReceiveEnterStateEvent(e.State))
            .RemoveListenerWhenGameObjectDestroyed(gameObject);
    }

    /// <summary>
    /// 处理状态变更事件
    /// </summary>
    /// <param name="state"></param>
    private void OnReceiveEnterStateEvent(GameState state)
    {
        if (state != GameState.LobbyWaiting) return;
    }


    #region 初始化
    /// <summary>
    /// 初始化UI组件
    /// </summary>
    protected override void SetupUIEvents()
    {
        base.SetupUIEvents();
        BindUIEvent(startGameBtn.onClick, UIRequestEventSystem.Send<OnStartGameButtonClick>);
        BindUIEvent(playSettingBtn.onClick, UIRequestEventSystem.Send<OnPlaySettingButtonClick>);
        BindUIEvent(gameSettingBtn.onClick, UIRequestEventSystem.Send<OnGameSettingButtonClick>);
        BindUIEvent(joinRoomBtn.onClick, UIRequestEventSystem.Send<OnJoinRoomButtonClick>);
        BindUIEvent(micTog.onValueChanged, (isOn) => UIRequestEventSystem.Send(new OnMicToggleValueChanged(isOn)));
        BindUIEvent(speakerTog.onValueChanged, (isOn) => UIRequestEventSystem.Send(new OnSpeakerToggleValueChanged(isOn)));
    }
    #endregion
}
