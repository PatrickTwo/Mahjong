using Mahjong;
using Mahjong.System.TypeEventSystem;
using UnityEngine;

public abstract class BaseUIRequestHandler : MonoBehaviour
{
    // 事件系统引用
    protected IEventSystem UIEventSystem => EventSystemManager.Instance.UIControlEventSystem;
    protected IEventSystem UIRequestEventSystem => EventSystemManager.Instance.UIRequestEventSystem;
    // 管理器引用
    protected MahjongGameManager GameManager => MahjongGameManager.Instance;
    // 游戏流程控制器引用
    protected GameFlowController FlowController => MahjongGameManager.Instance.FlowController;
}
