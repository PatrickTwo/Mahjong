using System.Collections.Generic;
using Mahjong;
using UnityEngine;

/// <summary>
/// 首页玩家列表视图。
/// </summary>
public class PlayerListViewUI : BaseViewUI
{
    #region 字段

    private readonly List<PlayerCardUI> playerCardUIs = new List<PlayerCardUI>();
    [SerializeField] private Transform playerCardContainer; // 玩家卡片容器

    #endregion

    #region 生命周期

    protected override void Awake()
    {
        base.Awake();
        LobbyPresenter.Instance.Initialize();

        for (int i = 0; i < playerCardContainer.childCount; i++)
        {
            playerCardUIs.Add(playerCardContainer.GetChild(i).GetComponent<PlayerCardUI>());
        }
    }

    private void OnEnable()
    {
        LobbyPresenter.Instance.ReadModelChanged += OnLobbyReadModelChanged;
        ApplyReadModel(LobbyPresenter.Instance.CurrentReadModel);
    }

    private void OnDisable()
    {
        LobbyPresenter.Instance.ReadModelChanged -= OnLobbyReadModelChanged;
    }

    #endregion

    #region 视图刷新

    /// <summary>
    /// 响应大厅只读模型变化。
    /// </summary>
    /// <param name="readModel">大厅只读模型。</param>
    private void OnLobbyReadModelChanged(LobbyReadModel readModel)
    {
        ApplyReadModel(readModel);
    }

    /// <summary>
    /// 根据大厅只读模型刷新玩家卡片。
    /// </summary>
    /// <param name="readModel">大厅只读模型。</param>
    private void ApplyReadModel(LobbyReadModel readModel)
    {
        if (readModel == null)
        {
            return;
        }

        int cardCount = playerCardUIs.Count;
        int viewDataCount = readModel.PlayerCards.Count;
        int loopCount = cardCount < viewDataCount ? cardCount : viewDataCount;

        for (int i = 0; i < loopCount; i++)
        {
            LobbyPlayerCardViewData viewData = readModel.PlayerCards[i];
            if (viewData.IsOccupied)
            {
                playerCardUIs[i].SetViewData(viewData);
                continue;
            }

            playerCardUIs[i].Release();
        }
    }

    #endregion
}
