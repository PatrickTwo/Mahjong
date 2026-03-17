using System;

namespace Mahjong
{
    #region 骰子类
    /// <summary>
    /// 骰子类
    /// 用于生成随机点数
    /// </summary>
    public class Dice
    {
        private static readonly Random random = new Random();
        
        /// <summary>
        /// 骰子面数（麻将骰子通常为6面）
        /// </summary>
        public const int Sides = 6;
        
        /// <summary>
        /// 掷骰子
        /// </summary>
        /// <returns>骰子点数（1-6）</returns>
        public static int Roll()
        {
            return random.Next(1, Sides + 1);
        }
        
        /// <summary>
        /// 掷多个骰子
        /// </summary>
        /// <param name="count">骰子数量</param>
        /// <returns>骰子点数总和</returns>
        public static int RollMultiple(int count)
        {
            int total = 0;
            for (int i = 0; i < count; i++)
            {
                total += Roll();
            }
            return total;
        }
    }
    #endregion
}