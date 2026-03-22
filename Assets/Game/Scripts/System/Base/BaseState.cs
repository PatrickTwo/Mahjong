namespace Mahjong
{
    /// <summary>
    /// 状态机基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseState<T>
    {
        /// <summary>
        /// 该状态的枚举
        /// </summary>
        T StateType { get; }
        void Enter();
        void Exit();
        void Update();
        /// <summary>
        /// 判断是否可以转换到下一个状态
        /// </summary>
        /// <param name="nextState">下一个状态</param>
        /// <returns>如果可以转换则返回true，否则返回false</returns>
        bool CanTransitionTo(T nextState);
    }
}