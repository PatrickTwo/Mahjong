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
        public const string PlayerOperationPanel = "PlayerOperationPanel";
        /// <summary>
        /// 提示面板ID
        /// </summary>
        public const string PromptPanel = "PromptPanel";
        /// <summary>
        /// 加入房间面板ID
        /// </summary>
        public const string JoinRoomPanel = "JoinRoomPanel";
        /// <summary>
        /// 游戏设置面板ID
        /// </summary>
        public const string GameSettingPanel = "GameSettingPanel";
        /// <summary>
        /// 对局玩法设置面板ID
        /// </summary>
        public const string PlaySettingPanel = "PlaySettingPanel";
        /// <summary>
        /// 玩家信息面板ID
        /// </summary>
        public const string PlayerInfoPanel = "PlayerInfoPanel";
    }
}