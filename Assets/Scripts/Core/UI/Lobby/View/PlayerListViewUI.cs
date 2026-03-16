using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 首页玩家列表UI组件
/// 显示在首页玩家列表中
/// </summary>
public class PlayerListViewUI : BaseViewUI
{
    private readonly List<PlayerCardUI> playerCardUIs = new();
    private Transform playerCardContainer; // 玩家卡片容器

    protected override void FindReference()
    {
        playerCardContainer = transform.FindCompInChild<Transform>("PlayerCardContainer");
    }
    protected override void InitializeField()
    {
        for (int i = 0; i < playerCardContainer.childCount; i++)
        {
            playerCardUIs.Add(playerCardContainer.GetChild(i).GetComponent<PlayerCardUI>());
        }
    }
}
