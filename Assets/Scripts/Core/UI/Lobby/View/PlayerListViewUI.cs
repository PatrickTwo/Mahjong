using System;
using System.Collections.Generic;
using System.Linq;
using Mahjong;
using UnityEngine;

/// <summary>
/// 首页玩家列表UI组件
/// 显示在首页玩家列表中
/// </summary>
public class PlayerListViewUI : BaseViewUI
{
    private readonly List<PlayerCardUI> playerCardUIs = new();
    private Transform playerCardContainer; // 玩家卡片容器

    protected override void RegisterFromEventSystem()
    {
        base.RegisterFromEventSystem();
        ModelEventSystem.AddListener<AddPlayerEvent>((e) => OnAddPlayer(e.player))
            .RemoveListenerWhenGameObjectDestroyed(gameObject);
        ModelEventSystem.AddListener<RemovePlayerEvent>((e) => OnRemovePlayer(e.player))
            .RemoveListenerWhenGameObjectDestroyed(gameObject);
    }


    private void OnAddPlayer(Player player)
    {
        // 查找第一个空位插入
        PlayerCardUI availableCard = playerCardUIs.FirstOrDefault(card => !card.IsOccupied);
        availableCard.SetPlayer(player);
    }
    private void OnRemovePlayer(Player player)
    {
        PlayerCardUI playerCard = playerCardUIs.FirstOrDefault(card => card.Player == player);
        playerCard.Release();
    }

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
