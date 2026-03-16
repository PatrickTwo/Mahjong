using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatBoxViewUI : BaseViewUI
{
    private TextMeshProUGUI chatContentText; // 聊天内容展示文本
    private Button sendBtn; // 发送按钮
    private TMP_InputField messageInput; // 输入框

    protected override void FindReference()
    {
        base.FindReference();
        chatContentText = transform.FindCompInChild<TextMeshProUGUI>("ChatContentText");
        sendBtn = transform.FindCompInChild<Button>("SendBtn");
        messageInput = transform.FindCompInChild<TMP_InputField>("MessageInput");
    }
    protected override void AddUIListener()
    {
        base.AddUIListener();
        RegisterUIListener(sendBtn.onClick, OnSendButtonClick);
    }
    private void OnSendButtonClick()
    {
        HLogger.Log("点击了发送按钮");
    }
}
