using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 协程工具类
/// 1. 为非 MonoBehaviour 类提供协程启动接口
/// 2. 提供延迟执行工具：按时间或按帧数延迟回调
/// 3. 为每个协程分配唯一 id，便于追踪
/// 4. 可通过协程 id 停止指定协程
/// </summary>
public static class CoroutineTools
{
    /// <summary>
    /// 内部运行协程的挂载节点
    /// </summary>
    private class CoroutineRunner : MonoBehaviour
    {
    }

    private static CoroutineRunner _runner;
    private static readonly Dictionary<string, Coroutine> _coroutines = new Dictionary<string, Coroutine>();

    /// <summary>
    /// 确保用于运行协程的 GameObject 存在
    /// </summary>
    private static void EnsureRunner()
    {
        if (_runner != null)
        {
            return;
        }

        var go = new GameObject("CoroutineToolsRunner");
        UnityEngine.Object.DontDestroyOnLoad(go);
        _runner = go.AddComponent<CoroutineRunner>();
    }
    #region 启动协程
    /// <summary>
    /// 为非 MonoBehaviour 类启动协程，返回唯一协程 id
    /// </summary>
    /// <param name="routine"></param>
    /// <returns>唯一协程 id</returns>
    public static string StartCoroutine(IEnumerator routine)
    {
        if (routine == null)
        {
            throw new ArgumentNullException(nameof(routine));
        }

        EnsureRunner();

        string key = $"Coroutine_{System.Guid.NewGuid():N}";
        Coroutine coroutine = _runner.StartCoroutine(WrapCoroutine(key, routine));
        _coroutines[key] = coroutine;
        return key;
    }
    /// <summary>
    /// 包装协程，结束时自动移除记录
    /// </summary>
    private static IEnumerator WrapCoroutine(string key, IEnumerator routine)
    {
        yield return routine;
        _coroutines.Remove(key);
    }
    #endregion
    #region 停止协程
    /// <summary>
    /// 按协程 id 停止协程
    /// </summary>
    /// <param name="id">协程 id</param>
    /// <returns>是否成功停止</returns>
    public static bool StopCoroutine(string key)
    {
        if (_runner == null)
        {
            return false;
        }

        if (_coroutines.TryGetValue(key, out Coroutine coroutine))
        {
            _runner.StopCoroutine(coroutine);
            _coroutines.Remove(key);
            return true;
        }

        return false;
    }
    #endregion
    #region 延迟执行
    /// <summary>
    /// 在指定秒数后执行回调
    /// </summary>
    /// <param name="seconds">延迟秒数</param>
    /// <param name="callback">回调</param>
    /// <returns>协程 id</returns>
    public static string DelayTime(float seconds, Action callback)
    {
        return StartCoroutine(DelayTimeRoutine(seconds, callback));
    }

    private static IEnumerator DelayTimeRoutine(float seconds, Action callback)
    {
        if (seconds > 0f)
        {
            yield return new WaitForSeconds(seconds);
        }
        else
        {
            yield return null;
        }

        callback?.Invoke();
    }

    /// <summary>
    /// 在指定帧数后执行回调
    /// </summary>
    /// <param name="frameCount">需要等待的帧数（>=1）</param>
    /// <param name="callback">回调</param>
    /// <returns>协程 id</returns>
    public static string DelayFrame(int frameCount, Action callback)
    {
        return StartCoroutine(DelayFrameRoutine(frameCount, callback));
    }

    private static IEnumerator DelayFrameRoutine(int frameCount, Action callback)
    {
        if (frameCount < 1)
        {
            frameCount = 1;
        }

        for (int i = 0; i < frameCount; i++)
        {
            yield return null;
        }

        callback?.Invoke();
    }
    #endregion
}

