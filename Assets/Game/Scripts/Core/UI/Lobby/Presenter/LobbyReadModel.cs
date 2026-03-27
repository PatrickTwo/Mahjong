using System.Collections.Generic;

namespace Mahjong
{
    /// <summary>
    /// 大厅页面只读模型
    /// 提供给视图层使用，避免直接依赖模型层。
    /// </summary>
    public sealed class LobbyReadModel
    {
        #region 构造函数

        /// <summary>
        /// 构造大厅只读模型。
        /// </summary>
        /// <param name="roomDisplayText">房间显示文本。</param>
        /// <param name="canStartGame">是否允许开始游戏。</param>
        /// <param name="isMicEnabled">麦克风是否开启。</param>
        /// <param name="isSpeakerEnabled">扬声器是否开启。</param>
        /// <param name="playerCards">玩家卡片视图数据。</param>
        public LobbyReadModel(
            string roomDisplayText,
            bool canStartGame,
            bool isMicEnabled,
            bool isSpeakerEnabled,
            IReadOnlyList<LobbyPlayerCardViewData> playerCards)
        {
            RoomDisplayText = roomDisplayText;
            CanStartGame = canStartGame;
            IsMicEnabled = isMicEnabled;
            IsSpeakerEnabled = isSpeakerEnabled;
            PlayerCards = playerCards;
        }

        #endregion

        #region 属性

        public string RoomDisplayText { get; } // 房间显示文本
        public bool CanStartGame { get; } // 是否允许开始游戏
        public bool IsMicEnabled { get; } // 麦克风是否开启
        public bool IsSpeakerEnabled { get; } // 扬声器是否开启
        public IReadOnlyList<LobbyPlayerCardViewData> PlayerCards { get; } // 根据玩家数据构建的玩家卡片视图数据

        #endregion
    }

    /// <summary>
    /// 大厅玩家卡片视图数据。
    /// </summary>
    public sealed class LobbyPlayerCardViewData
    {
        #region 静态属性
        // 空玩家卡片视图数据
        public static LobbyPlayerCardViewData Empty { get; } = new LobbyPlayerCardViewData(false, -1, string.Empty, string.Empty, false, -1);

        #endregion

        #region 构造函数

        public LobbyPlayerCardViewData(bool isOccupied, int playerId, string playerName, string readyStatusText, bool isAIPlayer, int lobbyCardIndex)
        {
            IsOccupied = isOccupied;
            PlayerId = playerId;
            PlayerName = playerName;
            ReadyStatusText = readyStatusText;
            IsAIPlayer = isAIPlayer;
            LobbyCardIndex = lobbyCardIndex;
        }

        #endregion

        #region 属性

        public bool IsOccupied { get; } // 是否已被玩家占用
        public int PlayerId { get; } // 玩家 ID
        public string PlayerName { get; } // 玩家名称
        public string ReadyStatusText { get; } // 准备状态文本
        public bool IsAIPlayer { get; } // 是否为 AI 玩家
        public int LobbyCardIndex { get; } // 大厅卡片索引

        #endregion
    }
}
