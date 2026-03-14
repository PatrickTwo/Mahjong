using System;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 大厅页面UI类
/// </summary>
public class LobbyPageUI : BasePageUI<LobbyUIRequestHandler>
{
    // UI组件引用
    private Button startGameButton; // 开始游戏按钮
    private Button playSettingButton; // 游戏玩法设置按钮
    private Button gameSettingButton; // 游戏设置按钮
    private Text roomIDText; // 房间号显示文本
    private Toggle micToggle; // 麦克风开关
    private Toggle speakerToggle; // 扬声器开关
    private ChatBoxViewUI chatBoxView; // 聊天框视图
    private PlayerListViewUI playerListView; // 玩家列表视图

    protected override void Awake()
    {
        base.Awake();
        requestHandler = new LobbyUIRequestHandler();

    }
    #region 初始化
    /// <summary>
    /// 注册UI事件到请求处理器
    /// </summary>
    protected override void RegisterUIEvents()
    {
        startGameButton.onClick.AddListener(requestHandler.OnStartGameButtonClick);
        playSettingButton.onClick.AddListener(requestHandler.OnPlaySettingButtonClick);
        gameSettingButton.onClick.AddListener(requestHandler.OnGameSettingButtonClick);
        micToggle.onValueChanged.AddListener(requestHandler.OnMicToggleValueChanged);
        speakerToggle.onValueChanged.AddListener(requestHandler.OnSpeakerToggleValueChanged);
    }

    /// <summary>
    /// 查找UI组件引用
    /// </summary>
    protected override void FindUIComponents()
    {
        startGameButton = transform.FindCompInChild<Button>("StartGameButton"); // 开始游戏按钮
        playSettingButton = transform.FindCompInChild<Button>("PlaySettingBtn"); // 游戏玩法设置按钮
        gameSettingButton = transform.FindCompInChild<Button>("GameSettingBtn"); // 游戏设置按钮
        roomIDText = transform.FindCompInChild<Text>("RoomID"); // 房间号显示文本
        micToggle = transform.FindCompInChild<Toggle>("MicTog"); // 麦克风开关
        speakerToggle = transform.FindCompInChild<Toggle>("SpeakerTog"); // 扬声器开关
        chatBoxView = transform.FindCompInChild<ChatBoxViewUI>("ChatBox"); // 聊天框视图
        playerListView = transform.FindCompInChild<PlayerListViewUI>("PlayerList"); // 玩家列表视图
    }
    #endregion
}
