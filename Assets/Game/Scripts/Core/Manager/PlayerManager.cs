using System.Collections.Generic;
using UnityEngine;

namespace Mahjong
{
    /// <summary>
    /// 玩家管理器。
    /// </summary>
    public class PlayerManager : MonoSingleton<PlayerManager>
    {
        #region 字段

        public Transform pfPlayer; // 玩家预制体
        public Transform pfAIPlayer; // AI 玩家预制体

        private IEventBusService eventBusService; // 事件总线服务
        private PlayerModel playerModel; // 存储玩家数据

        private readonly string[] aiPlayerNames = new string[]
        {
            "麻将大师", "牌场老手", "智能AI", "自动玩家"
        };

        #endregion

        #region 生命周期

        private void Awake()
        {
            eventBusService = EventBusService.Instance;
            playerModel = new PlayerModel(eventBusService.ModelEventSystem);
        }

        private void Start()
        {

        }

        #endregion

        #region 玩家管理

        /// <summary>
        /// 设置当前回合玩家。
        /// </summary>
        /// <param name="player">当前回合玩家。</param>
        public void SetCurrentPlayerTurn(Player player)
        {
        }

        public bool TryAddPlayer(Player player)
        {
            return playerModel.TryAddPlayer(player);
        }

        public bool TryRemovePlayer(int playerId)
        {
            return playerModel.TryRemovePlayer(playerId);
        }

        /// <summary>
        /// 尝试添加 AI 玩家。
        /// </summary>
        /// <param name="difficulty">AI 难度。</param>
        /// <returns>是否添加成功。</returns>
        public bool TryAddAIPlayer(int cardIndex, AIDifficulty difficulty = AIDifficulty.Normal)
        {
            if (pfAIPlayer == null)
            {
                HLogger.Log("AI 玩家预制体未设置");
                return false;
            }

            int aiPlayerId = playerModel.PlayerCount + 1;
            string aiPlayerName = GetRandomAIPlayerName();

            AIPlayer aiPlayer = Instantiate(pfAIPlayer, transform).GetComponent<AIPlayer>();
            aiPlayer.Init(new PlayerInfo(aiPlayerId, aiPlayerName), cardIndex, difficulty);

            bool success = playerModel.TryAddPlayer(aiPlayer);
            if (success)
            {
                HLogger.LogSuccess($"成功添加AI玩家：{aiPlayerName}（难度：{difficulty}）");
            }
            else
            {
                HLogger.Log($"添加AI玩家失败：{aiPlayerName}");
                Destroy(aiPlayer.gameObject);
            }

            return success;
        }

        /// <summary>
        /// 获取随机 AI 名称。
        /// </summary>
        /// <returns>AI 名称。</returns>
        private string GetRandomAIPlayerName()
        {
            int index = Random.Range(0, aiPlayerNames.Length);
            return aiPlayerNames[index];
        }

        #endregion

        #region 外部数据获取

        public List<Player> GetPlayers()
        {
            return playerModel.Players;
        }

        #endregion
    }
}
