using System.Collections;
using System.Collections.Generic;
using Mahjong;
using Mahjong.System.TypeEventSystem;

public class BankerSelectionState : BaseGameState
{
    public override GameState StateType => GameState.BankerSelection;

    private BankerSelectionService bankerSelectionService;
    private bool isSelectionComplete = false; // 记录庄家选择是否完成

    public BankerSelectionState(GameFlowController controller) : base(controller) { }

    public override void Enter()
    {
        base.Enter();
        HLogger.LogSuccess($"进入了{StateType}状态");

        // 加载游戏场景
        // TODO 改为异步加载
        GameSceneManager.LoadGameScene(SceneNameConst.GameScene);

        // 注册事件监听
        EventSystemManager.Instance.UIControlEventSystem.RemoveListener<RequestRollDiceEvent>(OnRequestRollDice);

        // 获取所有玩家
        List<Player> players = PlayerManager.Instance.GetPlayers();

        // 创建庄家选择服务
        bankerSelectionService = new BankerSelectionService(players);
        // 开始庄家选择
        bankerSelectionService.StartSelection();
    }

    public override void Exit()
    {
        base.Exit();
        // 取消事件监听
        EventSystemManager.Instance.UIControlEventSystem.AddListener<RequestRollDiceEvent>(OnRequestRollDice);
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



    /// <summary>
    /// 处理UI请求掷骰子事件
    /// </summary>
    /// <param name="e">请求掷骰子事件</param>
    private void OnRequestRollDice(RequestRollDiceEvent e)
    {
        HLogger.Log($"收到玩家 {e.Player.Info.PlayerName} 的掷骰子请求");

        // 调用庄家选择服务的掷骰子方法
        bankerSelectionService.RollDiceForPlayer(e.Player);
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