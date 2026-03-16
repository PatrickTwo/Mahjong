using System;

namespace Mahjong.Core.UI
{
    /// <summary>
    /// 面板ID常量类
    /// <para>功能：集中管理所有面板的标识符常量</para>
    /// <para>用法：在UI事件系统中使用这些常量来标识不同的面板</para>
    /// <para>示例：ShowPanelEvent("PlayerOperationPanel") 或 PanelID = PanelIDConst.PlayerOperationPanelID</para>
    /// </summary>
    public static class PanelIDConst
    {
        /// <summary>
        /// 玩家操作面板ID
        /// </summary>
        public const string PlayerOperationPanelID = "PlayerOperationPanel";
        /// <summary>
        /// 提示面板ID
        /// </summary>
        public const string PromptPanelID = "PromptPanel";
        /// <summary>
        /// 加入房间面板ID
        /// </summary>
        public const string JoinRoomPanelID = "JoinRoomPanel";
        /// <summary>
        /// 游戏设置面板ID
        /// </summary>
        public const string GameSettingPanelID = "GameSettingPanel";
        /// <summary>
        /// 对局玩法设置面板ID
        /// </summary>
        public const string PlaySettingPanelID = "PlaySettingPanel";
        /// <summary>
        /// 玩家信息面板ID
        /// </summary>
        public const string PlayerInfoPanelID = "PlayerInfoPanel";
    }
}