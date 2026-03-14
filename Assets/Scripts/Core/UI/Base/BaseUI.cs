using UnityEngine;

/// <summary>
/// UI面板基类
/// 继承结构：BaseUI → BasePageUI / BasePanelUI / BaseViewUI
/// 继承此类创建具体的UI面板，需重写以下方法：
/// - FindUIComponents: 查找UI组件引用
/// - RegisterUIEvents: 注册UI事件
/// - InitializeField: 初始化字段
/// ------------------------
/// 模版方法模式：InitializeUI() 方法定义了UI初始化的 算法骨架，父类控制流程，子类提供细节
/// </summary>
public class BaseUI : MonoBehaviour
{
    protected virtual void Awake()
    {
        InitializeUI();
    }

    protected virtual void Start()
    {

    }
    /// <summary>
    /// 初始化UI，依次调用查找组件和注册事件
    /// 子类无需重写此方法，只需重写FindUIComponents和RegisterUIEvents
    /// </summary>
    private void InitializeUI()
    {
        FindUIComponents();
        InitializeField();
        RegisterUIEvents();
    }
    /// <summary>
    /// 初始化字段<br/>
    /// 子类重写此方法实现字段初始化
    /// </summary>
    protected virtual void InitializeField() { }
    /// <summary>
    /// 查找并缓存UI组件引用<br/>
    /// 子类重写此方法实现组件查找
    /// </summary>
    protected virtual void FindUIComponents() { }
    /// <summary>
    /// 注册UI事件监听<br/>
    /// 子类重写此方法实现事件注册
    /// </summary>
    protected virtual void RegisterUIEvents() { }
}