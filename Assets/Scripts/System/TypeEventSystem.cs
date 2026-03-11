/*
    基于类型的全局事件系统
    实现IEvent接口的类或结构体（可以含有成员作为参数）可以作为事件发送
    支持自动事件注销：在注册事件时通过调用扩展方法实现自动注销功能
*/
using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

// 所有事件的实现接口
public interface IEvent { }

#region 自动注销
// 用于实现事件的自动注销，防止因忘记注销事件导致的内存泄漏
/// <summary>
/// 每注册一个事件返回一个移除器对象，调用<see cref="RemoveListenerWhenGameObjectDestroyed"/>方法即可实现自动注销功能
/// 一个ListenerAutoRemover包含一个监听者
/// </summary>
/// <typeparam name="T"></typeparam>
public class ListenerAutoRemover<T> where T : IEvent
{
    public Action<T> OnEvent { get; set; } // 要移除的事件

    public void RemoveListenerWhenGameObjectDestroyed(GameObject gameObject)
    {
        var trigger = gameObject.GetComponent<RemoveListenerOnDestroyComp>();
        if (!trigger)
        {
            trigger = gameObject.AddComponent<RemoveListenerOnDestroyComp>();
        }
        trigger.AddToRemove(this);
    }
}

#endregion
#region 自动注销组件
/// <summary>
/// 自动注销事件组件，该组件会记录当前GameObject上注册的所有事件类型及其对应的监听者
/// </summary>
public class RemoveListenerOnDestroyComp : MonoBehaviour
{
    // 存储事件监听器的字典，键为事件类型，值为该类型的ListenerAutoRemover列表（一个事件可能有多个观察者）
    // List<object> -> List<ListenerAutoRemover<T>>，使用List<object>是为了存储泛型T
    private readonly Dictionary<Type, List<object>> eventListeners = new();

    public void AddToRemove<T>(ListenerAutoRemover<T> autoRemoveListener) where T : IEvent
    {
        Type eventType = typeof(T);
        if (!eventListeners.ContainsKey(eventType))
        {
            eventListeners.Add(eventType, new List<object>());
        }
        eventListeners[eventType].Add(autoRemoveListener);
    }

    private void OnDestroy()
    {
        // 遍历所有事件类型
        foreach (KeyValuePair<Type, List<object>> kvp in eventListeners)
        {
            var eventType = kvp.Key;
            var listeners = kvp.Value;

            RemoveListenersForType(eventType, listeners);
        }
        eventListeners.Clear();
    }

    // 使用反射来移除特定类型的事件监听器
    private void RemoveListenersForType(Type eventType, List<object> listeners)
    {
        // 通过反射从事件系统中获取移除事件的方法
        MethodInfo removeMethod = typeof(TypeEventSystem).GetMethod("RemoveListener");

        foreach (object listener in listeners)
        {
            // 获取AutoRemoveListener<T>中的OnEvent委托
            PropertyInfo onEventProperty = listener.GetType().GetProperty("OnEvent");
            object onEventDelegate = onEventProperty.GetValue(listener);

            // 将RemoveListener方法转换为泛型方法，其中T类型被替换为eventType
            MethodInfo genericRemoveMethod = removeMethod.MakeGenericMethod(eventType);
            genericRemoveMethod.Invoke(null, new[] { onEventDelegate });
        }
    }
}
#endregion
#region 事件系统主要实现
public static class TypeEventSystem
{
    /// <summary>
    // 事件集合类，类型为T的事件监听器集合
    /// </summary>
    /// <typeparam name="T">事件类型</typeparam>
    class ListenersSet<T> where T : IEvent
    {
        public Action<T> OnEvent = obj => { };
    }

    // 按类型存储事件监听者集合
    // 字典值为ListenersSet<T>类型
    private static readonly Dictionary<Type, object> eventListenerSet = new();
    #region 发送
    // 发送无参事件
    public static void Send<T>() where T : IEvent => Send<T>(default);
    // 发送有参事件
    public static void Send<T>(T e = default) where T : IEvent
    {
        Type type = typeof(T);

        if (eventListenerSet.TryGetValue(type, out object listenersSetObj))
        {
            if (listenersSetObj is ListenersSet<T> listenersSet)
            {
                HLogger.Log(String.Format("发送事件：{0}", e.GetType().Name));
                listenersSet.OnEvent.Invoke(e);
            }
        }
    }
    #endregion
    #region 注册
    /// <summary>
    /// 事件注册，根据类型添加事件
    /// <para>调用该方法返回值的成员方法可以实现自动注销功能</para>
    /// </summary>
    // 无参事件注册
    public static ListenerAutoRemover<T> AddListener<T>(Action onEvent) where T : IEvent
    {
        return AddListener<T>(e => onEvent());
    }
    // 带参事件注册
    public static ListenerAutoRemover<T> AddListener<T>(Action<T> onEvent) where T : IEvent
    {
        Type type = typeof(T);
        ListenersSet<T> listenersSet;

        if (eventListenerSet.TryGetValue(type, out object listenersSetObj))
        {
            listenersSet = listenersSetObj as ListenersSet<T>;
        }
        else
        {
            listenersSet = new ListenersSet<T>();
            eventListenerSet.Add(type, listenersSet);
        }

        listenersSet.OnEvent += onEvent; // 向委托注册监听者

        return new ListenerAutoRemover<T>()
        {
            OnEvent = onEvent,
        };
    }
    #endregion
    #region 注销
    public static void RemoveListener<T>(Action<T> onEvent) where T : IEvent
    {
        Type type = typeof(T);

        if (eventListenerSet.TryGetValue(type, out object listenersSetObj))
        {
            var listenersSet = listenersSetObj as ListenersSet<T>;
            listenersSet.OnEvent -= onEvent;
        }
    }
    #endregion
}
#endregion