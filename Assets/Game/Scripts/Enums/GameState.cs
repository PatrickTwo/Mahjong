using System;

namespace Mahjong
{
    #region 游戏状态枚举
    /// <summary>
    /// 游戏状态枚举
    /// </summary>
    public enum GameState
    {
        LobbyWaiting,          // 大厅等待
        // 开始游戏---------------------------------
        BankerSelection, // 庄家选择
        Dealing,        // 发牌中
        Playing,        // 进行中
        TingDeclared,   // 已听牌
        Win,            // 胡牌
        Draw,           // 流局
        Ended           // 结束
    }
    #endregion
}
