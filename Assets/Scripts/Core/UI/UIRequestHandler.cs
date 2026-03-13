using Mahjong;
using UnityEngine;

/// <summary>
/// UI请求处理基类，用于处理UI层的请求，并调用逻辑层的方法
/// UI->UIRequestHandler->Logic
/// 子类是不同的UI请求处理器，用于处理不同的UI请求
/// </summary>
public abstract class UIRequestHandler
{
    protected MahjongGameManager GameManager => MahjongGameManager.Instance;

}
