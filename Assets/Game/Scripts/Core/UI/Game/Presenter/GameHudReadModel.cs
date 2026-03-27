namespace Mahjong
{
    /// <summary>
    /// 对局 HUD 只读模型。
    /// </summary>
    public sealed class GameHudReadModel
    {
        #region 构造函数

        /// <summary>
        /// 构造对局 HUD 只读模型。
        /// </summary>
        public GameHudReadModel(
            GameState currentState,
            bool isBankerSelectionVisible,
            bool shouldShowPlayerOperationPanel,
            string promptMessage,
            string currentRollingPlayerName,
            string lastDicePlayerName,
            int lastDiceResult,
            string bankerName)
        {
            CurrentState = currentState;
            IsBankerSelectionVisible = isBankerSelectionVisible;
            ShouldShowPlayerOperationPanel = shouldShowPlayerOperationPanel;
            PromptMessage = promptMessage;
            CurrentRollingPlayerName = currentRollingPlayerName;
            LastDicePlayerName = lastDicePlayerName;
            LastDiceResult = lastDiceResult;
            BankerName = bankerName;
        }

        #endregion

        #region 属性

        public GameState CurrentState { get; }
        public bool IsBankerSelectionVisible { get; }
        public bool ShouldShowPlayerOperationPanel { get; }
        public string PromptMessage { get; }
        public string CurrentRollingPlayerName { get; }
        public string LastDicePlayerName { get; }
        public int LastDiceResult { get; }
        public string BankerName { get; }

        #endregion
    }
}
