using System.Collections;
using System.Collections.Generic;
using Mahjong;

public class BankerSelectionState : BaseGameState
{
    public override GameState StateType => GameState.BankerSelection;

    private BankerSelectionService bankerSelectionService;
    private bool isSelectionComplete = false; // 记录庄家选择是否完成

    public BankerSelectionState(GameFlowController controller) : base(controller) { }

    public override void Enter()
    {
        HLogger.LogSuccess($"进入了{StateType}状态");

        // 加载游戏场景
        // TODO 改为异步加载
        GameSceneManager.LoadGameScene(SceneNameConst.GameScene);

        

    }


    public override bool CanTransitionTo(GameState targetState)
    {
        return targetState switch
        {
            GameState.Dealing => isSelectionComplete,
            _ => false,
        };
    }

    #region 庄家选择逻辑
    private IEnumerator RunBankerSelection()
    {
        yield return null;
    }
    /// <summary>
    /// 初始化庄家选择流程
    /// </summary>
    private void InitializeBankerSelection()
    {
        // 获取所有玩家
        List<Player> players = PlayerManager.Instance.GetPlayers();

        // 创建庄家选择服务
        bankerSelectionService = new BankerSelectionService(players);

        // 注册事件
        bankerSelectionService.OnPlayerRolledDice += OnPlayerRolledDice;
        bankerSelectionService.OnBankerSelected += OnBankerSelected;

        // 开始庄家选择
        bankerSelectionService.StartSelection();
    }


    /// <summary>
    /// 玩家掷骰子事件处理
    /// </summary>
    /// <param name="player">玩家</param>
    /// <param name="diceResult">骰子点数</param>
    private void OnPlayerRolledDice(Player player, int diceResult)
    {
        // 可以在这里添加UI更新逻辑，显示玩家掷骰子的结果
        HLogger.Log($"玩家 {player.Info.PlayerName} 掷出点数: {diceResult}");

        // 触发UI事件，显示掷骰子动画和结果
        // ViewEventSystem.Send(new PlayerDiceRolledEvent(player, diceResult));
    }

    /// <summary>
    /// 庄家选择完成事件处理
    /// </summary>
    /// <param name="banker">选中的庄家</param>
    private void OnBankerSelected(Player banker)
    {
        HLogger.LogSuccess($"庄家选择完成: {banker.Info.PlayerName} 成为庄家");

        // 设置庄家选择完成标志
        isSelectionComplete = true;

        // 触发UI事件，显示庄家选择结果
        // ViewEventSystem.Send(new BankerSelectedEvent(banker));

        // 可以在这里添加一些延迟，让玩家看到结果后再转换状态
        // 例如：StartCoroutine(DelayStateTransition());
    }

    /// <summary>
    /// 延迟状态转换（用于显示结果动画）
    /// </summary>
    // private IEnumerator DelayStateTransition()
    // {
    //     yield return new WaitForSeconds(2f); // 延迟2秒
    //     isSelectionComplete = true;
    // }

    #endregion
}