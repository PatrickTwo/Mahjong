using System;
using System.Collections.Generic;

/// <summary>
/// 可绑定属性
/// 用于在不同组件之间共享数据，当属性值改变时，会自动触发事件通知其他组件。
/// </summary>
/// <typeparam name="T"></typeparam>
public class BindableProperty<T>
{
    public T Value
    {
        get => mValue;
        set
        {
            // EqualityComparer<T> 泛型相等比较起
            // .Default 获取 T 类型的默认比较器实例
            // .Equals(a, b) 调用比较器的 Equals 方法 判断两个值是否相等
            if (EqualityComparer<T>.Default.Equals(mValue, value)) return;
            mValue = value;
            OnValueChanged?.Invoke(value);
        }
    }
    private T mValue;
    public event Action<T> OnValueChanged;
}