using System.Collections.Generic;
using Mahjong;
using UnityEngine;

/// <summary>
/// 大厅玩家列表组件。
/// </summary>
public class PlayerListWidgetUI : BaseWidgetUI
{
    #region Fields

    private readonly List<PlayerCardWidgetUI> playerCardUIs = new();

    [SerializeField] private Transform playerCardContainer;

    #endregion

    #region Lifecycle

    protected override void Awake()
    {
        base.Awake();
        LobbyPresenter.Instance.Initialize();

        for (int i = 0; i < playerCardContainer.childCount; i++)
        {
            playerCardUIs.Add(playerCardContainer.GetChild(i).GetComponent<PlayerCardWidgetUI>());
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

    #region View

    /// <summary>
    /// 处理大厅读模型变更事件。
    /// </summary>
    /// <param name="readModel">大厅读模型。</param>
    private void OnLobbyReadModelChanged(LobbyReadModel readModel)
    {
        ApplyReadModel(readModel);
    }

    /// <summary>
    /// 将读模型应用到玩家卡片组件。
    /// </summary>
    /// <param name="readModel">大厅读模型。</param>
    private void ApplyReadModel(LobbyReadModel readModel)
    {
        if (readModel == null)
        {
            return;
        }

        int cardCount = Mathf.Min(playerCardUIs.Count, readModel.PlayerCards.Count);
        for (int i = 0; i < cardCount; i++)
        {
            LobbyPlayerCardViewData viewData = readModel.PlayerCards[i];
            if (viewData.IsOccupied)
            {
                playerCardUIs[i].SetViewData(viewData);
                continue;
            }

            playerCardUIs[i].Release();
        }

        for (int i = cardCount; i < playerCardUIs.Count; i++)
        {
            playerCardUIs[i].Release();
        }
    }

    #endregion
}
