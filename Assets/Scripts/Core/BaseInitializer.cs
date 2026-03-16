using System;
using UnityEngine;

/// <summary>
/// 基础初始化类
/// <para>继承结构：BaseInitializer -> BaseUI</para>
/// <para>用法：继承此类创建需要 查找引用 和 初始化字段 的组件，需重写以下方法：</para>
/// <para>- FindReference: 查找并缓存UI组件引用</para>
/// <para>- InitializeField: 初始化字段数据</para>
/// <para>模版方法模式：Initialize() 方法定义了初始化的算法骨架，父类控制流程，子类提供细节</para>
/// </summary>
public class BaseInitializer : MonoBehaviour
{
    protected virtual void Awake()
    {
        Initialize();
    }

    protected virtual void Start()
    {

    }
    protected virtual void OnDestroy()
    {

    }

    /// <summary>
    /// 初始化UI，依次调用查找组件和注册事件
    /// 子类无需重写此方法，只需重写FindUIComponents和RegisterUIEvents
    /// </summary>
    private void Initialize()
    {
        FindReference();
        InitializeField();
    }
    /// <summary>
    /// 查找并缓存组件引用<br/>
    /// 子类重写此方法实现组件查找
    /// </summary>
    protected virtual void FindReference() { }
    /// <summary>
    /// 初始化字段<br/>
    /// 子类重写此方法实现字段初始化
    /// </summary>
    protected virtual void InitializeField() { }

}