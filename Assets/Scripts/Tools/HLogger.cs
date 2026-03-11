using UnityEditor;
using UnityEngine;
/// <summary>
/// 封装日志输出类
/// </summary>
public static class HLogger
{
    /// <summary>
    /// 输出带有调用实例的日志信息
    /// </summary>
    /// <param name="go">调用该方法的实例</param>
    public static void LogWithInstance(Transform go, string message, Color color)
    {
        Debug.Log($"-----<color={color}>[{go.name}] : {message}</color>-----");
    }
    /// <summary>
    /// 输出不同颜色的日志信息
    /// </summary>
    public static void Log(string message, Color color = default)
    {
        string hexColor = ColorUtility.ToHtmlStringRGB(color);
        Debug.Log($"-----<color=#{hexColor}>{message}</color>-----");
    }
    /// <summary>
    /// 输出绿色的成功信息
    /// </summary>
    /// <param name="message">信息内容</param>
    public static void LogSuccess(string message)
    {
        Log(message, Color.green);
    }
    /// <summary>
    /// 输出红色的失败信息
    /// </summary>
    /// <param name="message">信息内容</param>
    public static void LogFail(string message)
    {
        Log(message, Color.red);
    }
}