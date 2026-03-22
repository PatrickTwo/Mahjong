using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatBoxViewUI : BaseViewUI
{
    [SerializeField] private TextMeshProUGUI chatContentText; // 聊天内容展示文本
    [SerializeField] private Button sendBtn; // 发送按钮
    [SerializeField] private TMP_InputField messageInput; // 输入框


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
