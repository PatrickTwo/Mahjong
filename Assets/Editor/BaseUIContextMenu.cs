using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 为 BaseUI 组件提供编辑器右键菜单扩展。
/// </summary>
public static class BaseUIContextMenu
{
    /// <summary>
    /// TMP 默认控件创建所需资源。
    /// </summary>
    private static readonly TMP_DefaultControls.Resources m_TmpResources = new TMP_DefaultControls.Resources();

    /// <summary>
    /// 标准 UI 控件创建所需资源（内置精灵），与编辑器右键菜单行为一致。
    /// </summary>
    private static readonly DefaultControls.Resources s_StandardResources = CreateStandardResources();

    /// <summary>
    /// 加载 Unity 内置 UI 精灵，构建 DefaultControls.Resources。
    /// <para>与 UnityEditor.UI.MenuOptions.GetStandardResources() 逻辑一致。</para>
    /// </summary>
    private static DefaultControls.Resources CreateStandardResources()
    {
        DefaultControls.Resources resources = new DefaultControls.Resources();
        resources.standard    = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
        resources.background  = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
        resources.inputField  = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/InputFieldBackground.psd");
        resources.knob        = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");
        resources.checkmark   = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Checkmark.psd");
        resources.dropdown    = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/DropdownArrow.psd");
        resources.mask        = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UIMask.psd");
        return resources;
    }

    #region UI创建

    /// <summary>
    /// 根据组件字段自动创建对应的 UI 子物体。
    /// </summary>
    [MenuItem("CONTEXT/BaseUI/Create UI")]
    private static void CreateUI(MenuCommand command)
    {
        MonoBehaviour target = command.context as MonoBehaviour;
        if (target == null)
        {
            Debug.LogError("[Create UI] 目标组件为空，无法创建 UI。");
            return;
        }

        Transform root = target.transform;
        Type targetType = target.GetType();
        FieldInfo[] fields = targetType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        DefaultControls.Resources resources = s_StandardResources;
        List<GameObject> createdObjects = new List<GameObject>();

        #region 按字段创建完整 UI
        foreach (FieldInfo field in fields)
        {
            if (!ShouldProcessField(field))
            {
                continue;
            }

            if (root.FindChildGo(field.Name, false) != null)
            {
                continue;
            }

            GameObject createdObject = CreateUIObjectByFieldType(root, field, resources);
            if (createdObject == null)
            {
                continue;
            }

            createdObjects.Add(createdObject);
        }
        #endregion

        FindReferenceForComponent(target);
        EditorUtility.SetDirty(target.gameObject);
        Debug.Log($"[Create UI] {target.name} ({targetType.Name})：已创建 {createdObjects.Count} 个 UI 元素。");
    }

    private static bool ShouldProcessField(FieldInfo field)
    {
        if (field == null || field.IsStatic)
        {
            return false;
        }

        bool hasSerializeField = field.GetCustomAttributes(typeof(SerializeField), true).Length > 0;
        if (!field.IsPublic && !hasSerializeField)
        {
            return false;
        }

        return IsSupportedUIFieldType(field.FieldType);
    }

    private static bool IsSupportedUIFieldType(Type fieldType)
    {
        return fieldType == typeof(GameObject)
               || fieldType == typeof(Button)
               || fieldType == typeof(Slider)
               || fieldType == typeof(InputField)
               || fieldType == typeof(Text)
               || fieldType == typeof(Image)
               || fieldType == typeof(Toggle)
               || fieldType == typeof(Scrollbar)
               || fieldType == typeof(Dropdown)
               || fieldType == typeof(ScrollRect)
               || fieldType == typeof(TextMeshProUGUI)
               || fieldType == typeof(TMP_InputField)
               || fieldType == typeof(TMP_Dropdown);
    }

    /// <summary>
    /// 根据字段类型创建结构完整的 UI 物体，并按 Find Reference 规则命名。
    /// </summary>
    private static GameObject CreateUIObjectByFieldType(Transform parent, FieldInfo field, DefaultControls.Resources resources)
    {
        Type fieldType = field.FieldType;
        string uiObjectName = $"ID={field.Name}";
        GameObject createdObject = null;

        #region 按字段类型创建默认控件
        if (fieldType == typeof(Button))
        {
            createdObject = DefaultControls.CreateButton(resources);
        }
        else if (fieldType == typeof(Slider))
        {
            createdObject = DefaultControls.CreateSlider(resources);
        }
        else if (fieldType == typeof(InputField))
        {
            createdObject = DefaultControls.CreateInputField(resources);
        }
        else if (fieldType == typeof(Text))
        {
            createdObject = DefaultControls.CreateText(resources);
        }
        else if (fieldType == typeof(Image))
        {
            createdObject = DefaultControls.CreateImage(resources);
        }
        else if (fieldType == typeof(Toggle))
        {
            createdObject = DefaultControls.CreateToggle(resources);
        }
        else if (fieldType == typeof(Scrollbar))
        {
            createdObject = DefaultControls.CreateScrollbar(resources);
        }
        else if (fieldType == typeof(Dropdown))
        {
            createdObject = DefaultControls.CreateDropdown(resources);
        }
        else if (fieldType == typeof(ScrollRect))
        {
            createdObject = DefaultControls.CreateScrollView(resources);
        }
        else if (fieldType == typeof(TextMeshProUGUI))
        {
            createdObject = TMP_DefaultControls.CreateText(m_TmpResources);
        }
        else if (fieldType == typeof(TMP_InputField))
        {
            createdObject = TMP_DefaultControls.CreateInputField(m_TmpResources);
        }
        else if (fieldType == typeof(TMP_Dropdown))
        {
            createdObject = TMP_DefaultControls.CreateDropdown(m_TmpResources);
        }
        else if (fieldType == typeof(GameObject))
        {
            createdObject = new GameObject(uiObjectName, typeof(RectTransform));
        }
        #endregion

        if (createdObject == null)
        {
            return null;
        }

        Undo.RegisterCreatedObjectUndo(createdObject, $"Create {uiObjectName}");
        createdObject.name = uiObjectName;
        createdObject.transform.SetParent(parent, false);
        return createdObject;
    }

    #endregion

    #region 引用查找

    /// <summary>
    /// 自动查找并绑定子物体引用。
    /// </summary>
    [MenuItem("CONTEXT/BaseUI/Find Reference")]
    private static void FindReference(MenuCommand command)
    {
        MonoBehaviour target = command.context as MonoBehaviour;
        if (target == null)
        {
            return;
        }

        FindReferenceForComponent(target);
    }

    private static void FindReferenceForComponent(MonoBehaviour target)
    {
        Type type = target.GetType();
        FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        SerializedObject serializedObject = new SerializedObject(target);
        int foundCount = 0;

        #region 按命名规则绑定字段引用
        foreach (FieldInfo field in fields)
        {
            if (!ShouldProcessField(field))
            {
                continue;
            }

            SerializedProperty serializedProperty = serializedObject.FindProperty(field.Name);
            if (serializedProperty == null)
            {
                continue;
            }

            if (field.FieldType == typeof(GameObject))
            {
                GameObject childObject = target.transform.FindChildGo(field.Name, false);
                if (childObject != null)
                {
                    serializedProperty.objectReferenceValue = childObject;
                    foundCount++;
                }
                else
                {
                    HLogger.LogFail($"[Find Reference] 未找到字段 {field.Name} 对应的 GameObject 子物体。");
                }
            }
            else
            {
                Component component = target.transform.FindCompInChild(field.FieldType, field.Name);
                if (component != null)
                {
                    serializedProperty.objectReferenceValue = component;
                    foundCount++;
                }
                else
                {
                    HLogger.LogFail($"[Find Reference] 未找到字段 {field.Name} ({field.FieldType.Name}) 对应的组件。");
                }
            }
        }
        #endregion

        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(target);
        Debug.Log($"[Find Reference] {target.name} ({type.Name})：已找到 {foundCount} 个引用。");
    }

    #endregion
}
