using UnityEngine;

public abstract class BasePageUI<T> : MonoBehaviour where T : UIRequestHandler
{
    protected T requestHandler; // 具体的请求处理器

    protected virtual void Awake() { }
}
