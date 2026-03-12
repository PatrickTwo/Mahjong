using Mahjong;



/// <summary>
/// UI请求处理基类
/// UI -> UIRequest -> Logic
/// 不同的UI请求处理类需要继承自这个基类
/// </summary>
public abstract class UIRequestHandler
{
    protected MahjongGameManager GameManager => MahjongGameManager.Instance;
}