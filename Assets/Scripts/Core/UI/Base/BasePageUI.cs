using Mahjong.Core.UI;

public abstract class BasePageUI<T> : BaseUI where T : BasePageUIRequestHandler, new()
{
    protected T requestHandler = new();
}
