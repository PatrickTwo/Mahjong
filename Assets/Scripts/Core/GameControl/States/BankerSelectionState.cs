using Mahjong;

public class BankerSelectionState : BaseGameState
{
    public override GameState StateType => GameState.BankerSelection;

    public BankerSelectionState(GameFlowController controller) : base(controller) { }

    public override void Enter()
    {
        HLogger.LogSuccess($"进入了{StateType}状态");
        // 加载游戏场景
        GameSceneManager.LoadGameScene(SceneNameConst.GameScene);
        // TODO 四名玩家依次掷骰子，骰子点数大者为庄家
    }

    public override bool CanTransitionTo(GameState targetState)
    {
        return targetState switch
        {
            GameState.Dealing => true,
            _ => false,
        };
    }
}