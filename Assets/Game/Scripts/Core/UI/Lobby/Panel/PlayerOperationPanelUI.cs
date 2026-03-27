using Mahjong;
using Mahjong.Core.UI;

/// <summary>
/// 玩家操作面板。
/// </summary>
public class PlayerOperationPanelUI : BasePanelUI
{
    #region 字段

    protected override string PanelID => PanelIDConst.PlayerOperationPanel;

    #endregion

    #region 生命周期

    protected override void Awake()
    {
        base.Awake();
        GameHudPresenter.Instance.Initialize();
    }

    private void OnEnable()
    {
        GameHudPresenter.Instance.ReadModelChanged += OnGameHudReadModelChanged;
        ApplyReadModel(GameHudPresenter.Instance.CurrentReadModel);
    }

    private void OnDisable()
    {
        GameHudPresenter.Instance.ReadModelChanged -= OnGameHudReadModelChanged;
    }

    #endregion

    #region 界面刷新

    /// <summary>
    /// 响应对局 HUD 数据变化。
    /// </summary>
    /// <param name="readModel">对局 HUD 只读模型。</param>
    private void OnGameHudReadModelChanged(GameHudReadModel readModel)
    {
        ApplyReadModel(readModel);
    }

    /// <summary>
    /// 根据读模型刷新玩家操作面板显隐。
    /// </summary>
    /// <param name="readModel">对局 HUD 只读模型。</param>
    private void ApplyReadModel(GameHudReadModel readModel)
    {
        if (readModel == null)
        {
            return;
        }

        if (readModel.ShouldShowPlayerOperationPanel)
        {
            Show();
            return;
        }

        Hide();
    }

    #endregion
}
