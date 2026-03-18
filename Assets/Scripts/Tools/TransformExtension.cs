
using System;
using UnityEngine;

public static class TransformExtension
{
    /// <summary>
    /// 在子物体中递归查找指定ID的游戏物体上的组件
    /// 编辑器中的GameObject命名规则为ID=xxx，其中xxx作为查找Id，传入函数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parent"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static T FindCompInChild<T>(this Transform parent, string id, bool logFailWhenNotFound = true) where T : Component
    {
        Transform child = parent._FindChild(id);
        if (child != null)
        {
            if (child.TryGetComponent<T>(out T component))
            {
                return component;
            }
            else
            {
                HLogger.LogFail($"在{parent.name}下ID为{id}的子物体上未找到组件{typeof(T).Name}");
            }
        }
        else if (logFailWhenNotFound)
        {
            HLogger.LogFail($"在{parent.name}下未找到ID为{id}的子物体");
        }
        return null;
    }
    /// <summary>
    /// 在子物体中递归查找指定ID的游戏物体上的组件
    /// 编辑器中的GameObject命名规则为ID=xxx，其中xxx作为查找Id，传入函数
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="type"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static Component FindCompInChild(this Transform parent, Type type, string id)
    {
        Transform child = parent._FindChild(id);
        if (child != null)
        {
            if (child.TryGetComponent(type, out Component component))
            {
                return component;
            }
            else
            {
                HLogger.LogFail($"在{parent.name}下ID为{id}的子物体上未找到组件{type.Name}");
            }
        }
        else
        {
            HLogger.LogFail($"在{parent.name}下未找到ID为{id}的子物体");
        }
        return null;
    }
    /// <summary>
    /// 在子物体中递归查找指定ID的游戏物体
    /// 编辑器中的GameObject命名规则为ID=xxx，其中xxx作为查找Id，传入函数
    /// </summary>
    /// <param name="id"></param>
    /// <param name="logFailWhenNotFound"></param>
    /// <returns></returns>
    public  static GameObject FindChildGo(this Transform parent, string id, bool logFailWhenNotFound = true)
    {
        Transform child = parent._FindChild(id);
        if (child != null)
        {
            return child.gameObject;
        }
        else if (logFailWhenNotFound)
        {
            HLogger.LogFail($"在{parent.name}下未找到ID为{id}的子物体");
        }
        return null;
    }
    /// <summary>
    /// 内部函数，用于递归查找指定id子物体
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private static Transform _FindChild(this Transform parent, string id)
    {
        foreach (Transform child in parent)
        {
            // 不区分大小写的查找
            if (child.name.Replace(" ", "").Equals($"ID={id}", StringComparison.OrdinalIgnoreCase))
            {
                return child;
            }
            Transform result = child._FindChild(id);
            if (result != null)
            {
                return result;
            }
        }
        return null;
    }
}