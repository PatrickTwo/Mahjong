using System.Collections.Generic;
using Mahjong;
using Mahjong.System.TypeEventSystem;

public class BankerSelectionState : BaseGameState
{
    public override GameState StateType => GameState.BankerSelection;
    public bool IsSelectionComplete => isSelectionComplete;

    private BankerSelectionService bankerSelectionService;
    private bool isSelectionComplete;

    public BankerSelectionState(GameFlowController controller) : base(controller) { }

    public override void Enter()
    {
        base.Enter();
        HLogger.LogSuccess($"进入 {StateType} 状态");

        GameSceneManager.LoadGameScene(SceneNameConst.GameScene);
        EventSystemManager.Instance.UIControlEventSystem.RemoveListener<RequestRollDiceEvent>(OnRequestRollDice);

        List<Player> players = PlayerManager.Instance.GetPlayers();
        bankerSelectionService = new BankerSelectionService(players);
        bankerSelectionService.StartSelection();
    }

    public override void Exit()
    {
        base.Exit();
        EventSystemManager.Instance.UIControlEventSystem.AddListener<RequestRollDiceEvent>(OnRequestRollDice);
    }

    private void OnRequestRollDice(RequestRollDiceEvent e)
    {
        HLogger.Log($"收到玩家 {e.Player.Info.PlayerName} 的掷骰请求");
        bankerSelectionService.RollDiceForPlayer(e.Player);
    }
}
