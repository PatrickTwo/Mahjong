using System;
using System.Reflection;
using System.Threading;

namespace Mahjong
{
    /// <summary>
    /// 线程安全的延迟加载单例基类。
    /// <para>该基类仅依赖纯 C# 能力，不依赖 Unity 的组件生命周期。</para>
    /// <para>内部使用 <see cref="Lazy{T}"/> 保证首次访问时再创建实例，并保持线程安全。</para>
    /// <para>子类只需提供无参构造函数即可，构造函数可以是 private 或 protected。</para>
    /// </summary>
    /// <typeparam name="T">单例子类类型。</typeparam>
    public abstract class LazySingleton<T> where T : LazySingleton<T>
    {
        #region 单例

        private static readonly Lazy<T> LazyInstance = new Lazy<T>(CreateInstance, LazyThreadSafetyMode.ExecutionAndPublication);

        /// <summary>
        /// 获取单例实例。
        /// </summary>
        public static T Instance => LazyInstance.Value;

        #endregion

        #region 构造函数

        /// <summary>
        /// 保护构造函数，避免外部直接实例化基类。
        /// </summary>
        protected LazySingleton()
        {
        }

        #endregion

        #region 内部逻辑

        /// <summary>
        /// 创建单例实例，允许子类使用 private 或 protected 的无参构造函数。
        /// </summary>
        /// <returns>创建完成的单例实例。</returns>
        /// <exception cref="InvalidOperationException">当子类未提供无参构造函数时抛出。</exception>
        private static T CreateInstance()
        {
            ConstructorInfo constructorInfo = typeof(T).GetConstructor(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                binder: null,
                types: Type.EmptyTypes,
                modifiers: null);

            if (constructorInfo == null)
            {
                throw new InvalidOperationException(
                    $"类型 {typeof(T).FullName} 必须声明无参构造函数，且建议使用 private 或 protected 以保持单例约束。");
            }

            object instance = constructorInfo.Invoke(null);
            return (T)instance;
        }

        #endregion
    }
}
