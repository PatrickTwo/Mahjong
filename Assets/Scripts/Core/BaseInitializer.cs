using System;
using System.Reflection;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

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
    /// 查找并缓存组件引用<br/>
    /// 通过反射遍历所有带 SerializeField 特性的字段，根据字段名在子物体中查找对应引用并序列化保存到预制体中<br/>
    /// 修复说明：原代码使用 FieldInfo.SetValue 直接赋值，仅运行时有效，退出预制体后引用丢失。<br/>
    ///          现改用 SerializedObject API 进行序列化，可将引用持久化保存到预制体中
    /// </summary>
    [ContextMenu("Find Reference")]
    private void FindReference()
    {
#if UNITY_EDITOR
        // 获取当前类型，遍历所有私有实例字段（用于查找带有 SerializeField 的字段）
        Type type = GetType();
        FieldInfo[] fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

        // 创建 SerializedObject，用于操作 Unity 的序列化数据
        // 区别于 FieldInfo.SetValue：SetValue 只在运行时生效，不会持久化；SerializedObject 会写入序列化系统
        SerializedObject serializedObject = new(this);

        // 遍历所有字段，找出带 SerializeField 特性的字段
        foreach (FieldInfo field in fields)
        {
            object[] serializeAttributes = field.GetCustomAttributes(typeof(SerializeField), true);
            if (serializeAttributes.Length > 0)
            {
                // 根据字段名查找对应的 SerializedProperty（序列化属性）
                // 只有通过 SerializedProperty 设置的值才会被 Unity 序列化保存
                SerializedProperty serializedProperty = serializedObject.FindProperty(field.Name);
                if (serializedProperty != null)
                {
                    // 根据字段类型分别处理：GameObject 或 Component
                    if (field.FieldType == typeof(GameObject))
                    {
                        // 通过扩展方法查找子物体
                        GameObject go = transform.FindChildGo(field.Name);
                        // 将引用写入序列化属性（关键步骤：这样引用才会保存到预制体）
                        serializedProperty.objectReferenceValue = go;
                    }
                    else
                    {
                        // 查找子物体上的指定类型组件
                        Component component = transform.FindCompInChild(field.FieldType, field.Name);
                        // 将引用写入序列化属性
                        serializedProperty.objectReferenceValue = component;
                    }
                }
            }
        }

        // 应用所有修改到序列化对象（将内存中的修改写入序列化系统）
        serializedObject.ApplyModifiedProperties();
        // 标记当前对象为"脏"（已修改），提示 Unity 保存预制体时需要写入
        EditorUtility.SetDirty(this);
#endif
    }
    /// <summary>
    /// 初始化UI，依次调用查找组件和注册事件
    /// 子类无需重写此方法，只需重写FindUIComponents和RegisterUIEvents
    /// </summary>
    private void Initialize()
    {
        InitializeField();
        RegisterFromEventSystem();
    }

    /// <summary>
    /// 初始化字段<br/>
    /// 子类重写此方法实现字段初始化
    /// </summary>
    protected virtual void InitializeField() { }
    /// <summary>
    /// 注册事件到事件系统<br/>
    /// 子类重写此方法实现事件注册
    /// </summary>
    protected virtual void RegisterFromEventSystem() { }

}