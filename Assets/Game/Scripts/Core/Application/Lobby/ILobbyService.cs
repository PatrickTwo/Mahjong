namespace Mahjong
{
    /// <summary>
    /// 大厅业务服务接口。
    /// </summary>
    public interface ILobbyService
    {
        #region 只读模型

        /// <summary>
        /// 构建大厅只读模型。
        /// </summary>
        /// <param name="isMicEnabled">麦克风是否开启。</param>
        /// <param name="isSpeakerEnabled">扬声器是否开启。</param>
        /// <returns>大厅只读模型。</returns>
        LobbyReadModel BuildReadModel(bool isMicEnabled, bool isSpeakerEnabled);

        #endregion

        #region 用户意图

        /// <summary>
        /// 尝试开始游戏。
        /// </summary>
        /// <param name="message">执行结果消息。</param>
        /// <returns>是否成功。</returns>
        bool TryStartGame(out string message);

        /// <summary>
        /// 尝试加入房间。
        /// </summary>
        /// <param name="roomId">房间号。</param>
        /// <param name="message">执行结果消息。</param>
        /// <returns>是否成功。</returns>
        bool TryJoinRoom(string roomId, out string message);

        /// <summary>
        /// 尝试添加 AI 玩家。
        /// </summary>
        /// <param name="difficulty">AI 难度。</param>
        /// <param name="message">执行结果消息。</param>
        /// <returns>是否成功。</returns>
        bool TryAddAIPlayer(AIDifficulty difficulty, int cardIndex, out string message);
        /// <summary>
        /// 尝试踢出玩家。
        /// </summary>
        /// <param name="playerId">玩家 ID。</param>
        /// <param name="message">执行结果消息。</param>
        /// <returns>是否成功。</returns>
        bool TryKickPlayer(int playerId, out string message);

        #endregion
    }
}
