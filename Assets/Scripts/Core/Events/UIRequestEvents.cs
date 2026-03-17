using Mahjong.System.TypeEventSystem;

namespace Mahjong
{
    #region UI请求事件
    
    /// <summary>
    /// 添加AI玩家请求事件
    /// </summary>
    public struct AddAIPlayerRequestEvent : IEvent
    {
        /// <summary>
        /// AI玩家难度
        /// </summary>
        public AIDifficulty Difficulty { get; }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="difficulty">AI难度</param>
        public AddAIPlayerRequestEvent(AIDifficulty difficulty)
        {
            Difficulty = difficulty;
        }
    }
    
    #endregion
}