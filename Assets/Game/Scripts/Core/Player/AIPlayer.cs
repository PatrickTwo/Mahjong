using System;
using UnityEngine;

namespace Mahjong
{
    #region AI玩家类
    /// <summary>
    /// AI玩家类
    /// 继承自Player类，用于表示AI控制的玩家
    /// </summary>
    public class AIPlayer : Player
    {
        /// <summary>
        /// AI难度等级
        /// </summary>
        public AIDifficulty Difficulty { get; private set; }
        
        /// <summary>
        /// AI玩家初始化
        /// </summary>
        /// <param name="info">玩家信息</param>
        /// <param name="difficulty">AI难度</param>
        public void Init(PlayerInfo info, int cardIndex, AIDifficulty difficulty = AIDifficulty.Normal)
        {
            base.Init(info, cardIndex);
            Difficulty = difficulty;
            // 设置AI玩家的特殊标识
            Info.PlayerName = $"{info.PlayerName} (AI)";
        }
    }
    
    #region AI难度枚举
    /// <summary>
    /// AI难度枚举
    /// </summary>
    public enum AIDifficulty
    {
        Easy,       // 简单
        Normal,     // 普通
        Hard,       // 困难
        Expert      // 专家
    }
    #endregion
    #endregion
}