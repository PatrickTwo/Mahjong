using System.Collections.Generic;

namespace Mahjong
{
    /// <summary>
    /// 大厅业务服务。
    /// </summary>
    public class LobbyService : ILobbyService
    {
        #region 常量

        private const int MaxPlayerCount = 4;
        private const int PrototypeStartPlayerCount = 1;
        private const string DefaultRoomId = "LOCAL-0001";

        #endregion

        #region 字段

        private string currentRoomId = DefaultRoomId;

        #endregion

        #region 只读模型

        /// <summary>
        /// 构建大厅只读模型。
        /// </summary>
        /// <param name="isMicEnabled">麦克风是否开启。</param>
        /// <param name="isSpeakerEnabled">扬声器是否开启。</param>
        /// <returns>大厅只读模型。</returns>
        public LobbyReadModel BuildReadModel(bool isMicEnabled, bool isSpeakerEnabled)
        {
            List<Player> players = PlayerManager.Instance.GetPlayers();
            List<LobbyPlayerCardViewData> playerCards = new List<LobbyPlayerCardViewData>(MaxPlayerCount);

            for (int i = 0; i < MaxPlayerCount; i++)
            {
                if (i < players.Count)
                {
                    Player player = players[i];
                    LobbyPlayerCardViewData playerCard = new LobbyPlayerCardViewData(
                        true,
                        player.Info.PlayerId,
                        player.Info.PlayerName,
                        "未准备",
                        player is AIPlayer);
                    playerCards.Add(playerCard);
                    continue;
                }

                playerCards.Add(LobbyPlayerCardViewData.Empty);
            }

            bool canStartGame = players.Count >= PrototypeStartPlayerCount;
            return new LobbyReadModel(currentRoomId, canStartGame, isMicEnabled, isSpeakerEnabled, playerCards);
        }

        #endregion

        #region 用户意图

        /// <summary>
        /// 尝试开始游戏。
        /// </summary>
        /// <param name="message">执行结果消息。</param>
        /// <returns>是否成功。</returns>
        public bool TryStartGame(out string message)
        {
            List<Player> players = PlayerManager.Instance.GetPlayers();
            if (players.Count < PrototypeStartPlayerCount)
            {
                message = "当前玩家数量不足，无法开始游戏。";
                return false;
            }

            MahjongGameManager.Instance.FlowController.TransitionToState(GameState.BankerSelection);
            message = "开始游戏请求已提交。";
            return true;
        }

        /// <summary>
        /// 尝试加入房间。
        /// </summary>
        /// <param name="roomId">房间号。</param>
        /// <param name="message">执行结果消息。</param>
        /// <returns>是否成功。</returns>
        public bool TryJoinRoom(string roomId, out string message)
        {
            if (string.IsNullOrWhiteSpace(roomId))
            {
                message = "房间号不能为空。";
                return false;
            }

            currentRoomId = roomId.Trim();
            message = $"已切换到房间：{currentRoomId}";
            return true;
        }

        /// <summary>
        /// 尝试添加 AI 玩家。
        /// </summary>
        /// <param name="difficulty">AI 难度。</param>
        /// <param name="message">执行结果消息。</param>
        /// <returns>是否成功。</returns>
        public bool TryAddAIPlayer(AIDifficulty difficulty, out string message)
        {
            bool success = PlayerManager.Instance.TryAddAIPlayer(difficulty);
            message = success ? "已添加 AI 玩家。" : "添加 AI 玩家失败。";
            return success;
        }

        #endregion
    }
}
