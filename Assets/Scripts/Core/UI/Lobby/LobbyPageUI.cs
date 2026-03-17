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
    // UI组件引用
    private Button startGameBtn; // 开始游戏按钮
    private Button playSettingBtn; // 游戏玩法设置按钮
    private Button gameSettingBtn; // 游戏设置按钮
    private TextMeshProUGUI roomIDText; // 房间号显示文本
    private Button joinRoomBtn; // 加入房间按钮
    private Toggle micTog; // 麦克风开关
    private Toggle speakerTog; // 扬声器开关
    // View
    private ChatBoxViewUI chatBoxView; // 聊天框视图
    private PlayerListViewUI playerListView; // 玩家列表视图

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
        HideAllPanels();
    }

    private void HideAllPanels()
    {
        // 暂时使用手动隐藏
        EventSystemManager.Instance.UIControlEventSystem.Send(new HidePanelEvent(PanelIDConst.GameSettingPanelID));
        EventSystemManager.Instance.UIControlEventSystem.Send(new HidePanelEvent(PanelIDConst.PlaySettingPanelID));
        EventSystemManager.Instance.UIControlEventSystem.Send(new HidePanelEvent(PanelIDConst.JoinRoomPanelID));
        EventSystemManager.Instance.UIControlEventSystem.Send(new HidePanelEvent(PanelIDConst.PlayerOperationPanelID));
        EventSystemManager.Instance.UIControlEventSystem.Send(new HidePanelEvent(PanelIDConst.PromptPanelID));
    }

    #region 初始化
    /// <summary>
    /// 初始化UI组件
    /// </summary>
    protected override void AddUIListener()
    {
        base.AddUIListener();
        RegisterUIListener(startGameBtn.onClick, UIRequestEventSystem.Send<OnStartGameButtonClick>);
        RegisterUIListener(playSettingBtn.onClick, UIRequestEventSystem.Send<OnPlaySettingButtonClick>);
        RegisterUIListener(gameSettingBtn.onClick, UIRequestEventSystem.Send<OnGameSettingButtonClick>);
        RegisterUIListener(joinRoomBtn.onClick, UIRequestEventSystem.Send<OnJoinRoomButtonClick>);
        RegisterUIListener(micTog.onValueChanged, (isOn) => UIRequestEventSystem.Send(new OnMicToggleValueChanged(isOn)));
        RegisterUIListener(speakerTog.onValueChanged, (isOn) => UIRequestEventSystem.Send(new OnSpeakerToggleValueChanged(isOn)));
    }

    /// <summary>
    /// 查找UI组件引用
    /// </summary>
    protected override void FindReference()
    {
        base.FindReference();
        joinRoomBtn = transform.FindCompInChild<Button>("JoinRoomBtn"); // 加入房间按钮
        startGameBtn = transform.FindCompInChild<Button>("StartGameBtn"); // 开始游戏按钮
        playSettingBtn = transform.FindCompInChild<Button>("PlaySettingBtn"); // 游戏玩法设置按钮
        gameSettingBtn = transform.FindCompInChild<Button>("GameSettingBtn"); // 游戏设置按钮
        roomIDText = transform.FindCompInChild<TextMeshProUGUI>("RoomIDText"); // 房间号显示文本
        micTog = transform.FindCompInChild<Toggle>("MicTog"); // 麦克风开关
        speakerTog = transform.FindCompInChild<Toggle>("SpeakerTog"); // 扬声器开关
        chatBoxView = transform.FindCompInChild<ChatBoxViewUI>("ChatBoxView"); // 聊天框视图
        playerListView = transform.FindCompInChild<PlayerListViewUI>("PlayerListView"); // 玩家列表视图
    }
    #endregion
}
