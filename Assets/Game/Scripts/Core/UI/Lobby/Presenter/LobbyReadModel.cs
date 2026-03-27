using System.Collections.Generic;

namespace Mahjong
{
    /// <summary>
    /// 大厅页面只读模型。
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

        public string RoomDisplayText { get; }
        public bool CanStartGame { get; }
        public bool IsMicEnabled { get; }
        public bool IsSpeakerEnabled { get; }
        public IReadOnlyList<LobbyPlayerCardViewData> PlayerCards { get; }

        #endregion
    }

    /// <summary>
    /// 大厅玩家卡片视图数据。
    /// </summary>
    public sealed class LobbyPlayerCardViewData
    {
        #region 静态属性

        public static LobbyPlayerCardViewData Empty { get; } = new LobbyPlayerCardViewData(false, -1, string.Empty, string.Empty, false);

        #endregion

        #region 构造函数

        public LobbyPlayerCardViewData(bool isOccupied, int playerId, string playerName, string readyStatusText, bool isAIPlayer)
        {
            IsOccupied = isOccupied;
            PlayerId = playerId;
            PlayerName = playerName;
            ReadyStatusText = readyStatusText;
            IsAIPlayer = isAIPlayer;
        }

        #endregion

        #region 属性

        public bool IsOccupied { get; }
        public int PlayerId { get; }
        public string PlayerName { get; }
        public string ReadyStatusText { get; }
        public bool IsAIPlayer { get; }

        #endregion
    }
}
