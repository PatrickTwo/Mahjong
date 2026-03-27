using System.Collections.Generic;
using UnityEngine;

namespace Mahjong
{
    /// <summary>
    /// 大厅服务。
    /// </summary>
    public class LobbyService : ILobbyService
    {
        #region Constants

        private const int RequiredPlayerCount = GameSettingModel.RequiredPlayerCount;
        private const string DefaultRoomId = "LOCAL-0001";
        private const string ReadyStatusText = "\u672A\u51C6\u5907";

        #endregion

        #region Fields

        private string currentRoomId = DefaultRoomId;

        #endregion

        #region Read Model

        /// <summary>
        /// 从当前玩家状态构建大厅读模型。
        /// </summary>
        /// <param name="isMicEnabled">是否启用麦克风。</param>
        /// <param name="isSpeakerEnabled">是否启用扬声器。</param>
        /// <returns>大厅读模型。</returns>
        public LobbyReadModel BuildReadModel(bool isMicEnabled, bool isSpeakerEnabled)
        {
            List<Player> players = PlayerManager.Instance.GetPlayers();
            List<LobbyPlayerCardViewData> playerCards = CreateEmptyPlayerCards();

            foreach (Player player in players)
            {
                int lobbyCardIndex = player.LobbyCardIndex;
                if (lobbyCardIndex < 0 || lobbyCardIndex >= RequiredPlayerCount)
                {
                    Debug.LogError($"u73A9\u5BB6\u5361\u7247\u7D22\u5F15\u8D8A\u754C\uFF0C\u73A9\u5BB6\uFF1A{player.Info.PlayerName}\uFF0C\u7D22\u5F15\uFF1A{lobbyCardIndex}");
                    continue;
                }

                if (playerCards[lobbyCardIndex].IsOccupied)
                {
                    Debug.LogError($"u68C0\u6D4B\u5230\u91CD\u590D\u7684\u5927\u5385\u5361\u7247\u7D22\u5F15\uFF0C\u7D22\u5F15\uFF1A{lobbyCardIndex}\uFF0C\u540E\u5199\u5165\u73A9\u5BB6\uFF1A{player.Info.PlayerName}");
                    continue;
                }

                LobbyPlayerCardViewData playerCard = new LobbyPlayerCardViewData(
                    true,
                    player.Info.PlayerId,
                    player.Info.PlayerName,
                    ReadyStatusText,
                    player is AIPlayer,
                    lobbyCardIndex);
                playerCards[lobbyCardIndex] = playerCard;
            }

            bool canStartGame = players.Count >= RequiredPlayerCount;
            return new LobbyReadModel(currentRoomId, canStartGame, isMicEnabled, isSpeakerEnabled, playerCards);
        }

        /// <summary>
        /// 为所有大厅槽位创建空卡片数据。
        /// </summary>
        /// <returns>初始化后的空玩家卡片列表。</returns>
        private static List<LobbyPlayerCardViewData> CreateEmptyPlayerCards()
        {
            List<LobbyPlayerCardViewData> playerCards = new List<LobbyPlayerCardViewData>(RequiredPlayerCount);
            for (int i = 0; i < RequiredPlayerCount; i++)
            {
                playerCards.Add(LobbyPlayerCardViewData.Empty);
            }

            return playerCards;
        }

        #endregion

        #region Intents

        /// <summary>
        /// 尝试开始游戏。
        /// </summary>
        /// <param name="message">结果消息。</param>
        /// <returns>操作是否成功。</returns>
        public bool TryStartGame(out string message)
        {
            List<Player> players = PlayerManager.Instance.GetPlayers();
            if (players.Count < RequiredPlayerCount)
            {
                message = "\u5F53\u524D\u73A9\u5BB6\u6570\u91CF\u4E0D\u8DB3\uFF0C\u65E0\u6CD5\u5F00\u59CB\u6E38\u620F\u3002";
                return false;
            }

            MahjongGameManager.Instance.FlowController.TransitionToState(GameState.BankerSelection);
            message = "\u5F00\u59CB\u6E38\u620F\u8BF7\u6C42\u5DF2\u63D0\u4EA4\u3002";
            return true;
        }

        /// <summary>
        /// 尝试加入房间。
        /// </summary>
        /// <param name="roomId">房间ID。</param>
        /// <param name="message">结果消息。</param>
        /// <returns>操作是否成功。</returns>
        public bool TryJoinRoom(string roomId, out string message)
        {
            if (string.IsNullOrWhiteSpace(roomId))
            {
                message = "\u623F\u95F4\u53F7\u4E0D\u80FD\u4E3A\u7A7A\u3002";
                return false;
            }

            currentRoomId = roomId.Trim();
            message = $"\u5DF2\u5207\u6362\u5230\u623F\u95F4\uFF1A{currentRoomId}";
            return true;
        }

        /// <summary>
        /// 尝试添加AI玩家。
        /// </summary>
        /// <param name="difficulty">AI难度。</param>
        /// <param name="cardIndex">目标卡片索引。</param>
        /// <param name="message">结果消息。</param>
        /// <returns>操作是否成功。</returns>
        public bool TryAddAIPlayer(AIDifficulty difficulty, int cardIndex, out string message)
        {
            bool success = PlayerManager.Instance.TryAddAIPlayer(cardIndex, difficulty);
            message = success ? "\u5DF2\u6DFB\u52A0 AI \u73A9\u5BB6\u3002" : "\u6DFB\u52A0 AI \u73A9\u5BB6\u5931\u8D25\u3002";
            return success;
        }

        /// <summary>
        /// 尝试从大厅移除玩家。
        /// </summary>
        /// <param name="playerId">玩家ID。</param>
        /// <param name="message">结果消息。</param>
        /// <returns>操作是否成功。</returns>
        public bool TryKickPlayer(int playerId, out string message)
        {
            bool success = PlayerManager.Instance.TryRemovePlayer(playerId);
            message = success ? $"\u5DF2\u8E22\u51FA\u73A9\u5BB6\uFF1A{playerId}" : $"\u8E22\u51FA\u73A9\u5BB6\u5931\u8D25\uFF1A{playerId}";
            return success;
        }

        #endregion
    }
}
