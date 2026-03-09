using System;

namespace Mahjong
{
    #region 麻将牌类
    /// <summary>
    /// 麻将牌类
    /// </summary>
    public class MahjongTile
    {
        public TileType Type { get; private set; }
        public SuitType Suit { get; private set; } // 花色
        public int Value { get; private set; } // 值
        public bool IsWindTile => Suit == SuitType.Wind;
        public bool IsDragonTile => Suit == SuitType.Dragon;
        public bool IsHonorTile => IsWindTile || IsDragonTile;

        public MahjongTile(TileType type, SuitType suit, int value)
        {
            Type = type;
            Suit = suit;
            Value = value;
        }

        public override bool Equals(object obj)
        {
            return obj is MahjongTile tile && tile.Type == Type;
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode();
        }
        /// <summary>
        /// 调试用，输出对应的排面值
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public override string ToString()
        {
            if (Suit == SuitType.Wind || Suit == SuitType.Dragon)
            {
                return Suit switch
                {
                    SuitType.Wind => Value switch
                    {
                        1 => "东风",
                        2 => "南风",
                        3 => "西风",
                        4 => "北风",
                        _ => $"未知风牌{Value}"
                    },
                    SuitType.Dragon => Value switch
                    {
                        1 => "红中",
                        2 => "发财",
                        3 => "白板",
                        _ => $"未知箭牌{Value}"
                    },
                    _ => $"{Suit}"
                };
            }
            else
            {
                // 普通花色的正确映射
                string type = Suit switch
                {
                    SuitType.Character => "万",  // 万子
                    SuitType.Bamboo => "条",     // 条子
                    SuitType.Dot => "饼",       // 饼子
                    _ => throw new ArgumentOutOfRangeException(nameof(Suit), Suit, null),
                };
                string valueStr = Value switch
                {
                    1 => "一",
                    2 => "二",
                    3 => "三",
                    4 => "四",
                    5 => "五",
                    6 => "六",
                    7 => "七",
                    8 => "八",
                    9 => "九",
                    _ => $"未知牌值{Value}"
                };
                return $"{valueStr}{type}";
            }
        }
    }
    #endregion
}
