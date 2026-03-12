using System;
using UnityEngine;
using UnityEngine.UI;


public class LobbyPageUI : BasePageUI
{
    // UI请求处理器
    private LobbyUIRequestHandler requestHandler;
    // UI组件引用
    private Button startGameButton;
    protected override void Awake()
    {
        base.Awake();
        requestHandler = new LobbyUIRequestHandler();
        FindUIComponents();
        // 注册UI事件
        RegisterUIEvents();
    }
    /// <summary>
    /// 注册UI事件到请求处理器
    /// </summary>
    private void RegisterUIEvents()
    {
        startGameButton.onClick.AddListener(requestHandler.OnStartGameButtonClick);
    }

    /// <summary>
    /// 查找UI组件引用
    /// </summary>
    private void FindUIComponents()
    {
        startGameButton = transform.FindCompInChild<Button>("StartGameButton");
    }
}
