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
        private static readonly Random random = new();

        /// <summary>
        /// 骰子面数（麻将骰子通常为6面）
        /// </summary>
        public const int SIDES = 6;

        /// <summary>
        /// 掷骰子
        /// </summary>
        /// <returns>骰子点数（1-6）</returns>
        public static int RollSingle()
        {
            return random.Next(1, SIDES + 1);
        }

        /// <summary>
        /// 掷两个骰子
        /// </summary>
        public static int[] RollCouple()
        {
            int[] results = new int[2];
            for (int i = 0; i < 2; i++)
            {
                results[i] = RollSingle();
            }
            return results;
        }
    }
    #endregion
}