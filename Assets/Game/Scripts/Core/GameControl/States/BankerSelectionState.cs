using System.Collections.Generic;
using Mahjong;

public class BankerSelectionState : BaseGameState
{
    #region 属性
    public override GameState StateType => GameState.BankerSelection;
    public bool IsSelectionComplete => isSelectionComplete;
    #endregion

    #region 字段
    private BankerSelectionService bankerSelectionService; // 庄家选择服务
    private bool isSelectionComplete;
    #endregion

    #region 构造函数
    public BankerSelectionState(GameFlowController controller) : base(controller) { }
    #endregion

    #region 状态生命周期
    public override void Enter()
    {
        base.Enter();
        HLogger.LogSuccess($"进入 {StateType} 状态");

        GameSceneManager.LoadGameScene(SceneNameConst.GameScene);
        EventSystemManager.Instance.UIRequestEventSystem.AddListener<RequestRollDiceEvent>(OnRequestRollDice);

        List<Player> players = PlayerManager.Instance.GetPlayers();
        bankerSelectionService = new BankerSelectionService(
            players,
            OnPlayerTurnToRoll,
            OnPlayerDiceRolled,
            OnBankerSelected);

        isSelectionComplete = false;
        bankerSelectionService.StartSelection();
    }

    public override void Exit()
    {
        base.Exit();
        EventSystemManager.Instance.UIRequestEventSystem.RemoveListener<RequestRollDiceEvent>(OnRequestRollDice);
        bankerSelectionService = null;
    }
    #endregion

    #region 事件处理
    private void OnRequestRollDice(RequestRollDiceEvent e)
    {
        HLogger.Log($"收到玩家 {e.Player.Info.PlayerName} 的掷骰请求");
        bankerSelectionService.RollDiceForPlayer(e.Player);
    }

    /// <summary>
    /// 将“轮到玩家掷骰”通知桥接到 UI 控制事件系统。
    /// </summary>
    /// <param name="player">当前轮到的玩家。</param>
    private void OnPlayerTurnToRoll(Player player)
    {
        EventSystemManager.Instance.UIControlEventSystem.Send(new PlayerTurnToRollEvent(player));
    }

    /// <summary>
    /// 将“玩家掷骰完成”通知桥接到 UI 控制事件系统。
    /// </summary>
    /// <param name="player">完成掷骰的玩家。</param>
    /// <param name="diceResult">掷骰结果。</param>
    private void OnPlayerDiceRolled(Player player, int diceResult)
    {
        EventSystemManager.Instance.UIControlEventSystem.Send(new PlayerDiceRolledEvent(player, diceResult));
    }

    /// <summary>
    /// 庄家选定后，发送完成事件并进入发牌阶段。
    /// </summary>
    /// <param name="banker">最终庄家。</param>
    private void OnBankerSelected(Player banker)
    {
        EventSystemManager.Instance.UIControlEventSystem.Send(new BankerSelectedEvent(banker));
        isSelectionComplete = true;
        Controller.TransitionToState(GameState.Dealing);
    }
    #endregion

    #region 状态转换
    public override bool CanTransitionTo(GameState nextState)
    {
        return nextState == GameState.Dealing;
    }
    #endregion
}
