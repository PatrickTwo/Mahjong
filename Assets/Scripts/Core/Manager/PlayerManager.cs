using System.Collections;
using System.Collections.Generic;
using Mahjong.System.TypeEventSystem;
using UnityEngine;

namespace Mahjong
{
    /// <summary>
    /// 玩家管理器
    /// </summary>
    public class PlayerManager : MonoSingleton<PlayerManager>
    {
        // 暂时使用inspector面板拖拽，后期接入资源打包系统
        public Transform pfPlayer;
        public Transform pfAIPlayer; // AI玩家预制体

        private IEventSystem ModelEventSystem => EventSystemManager.Instance.ModelEventSystem;
        private PlayerModel playerModel; // 存储玩家数据

        // AI玩家名称列表
        private readonly string[] aiPlayerNames = new string[]
        {
            "麻将大师", "牌场老手", "智能AI", "自动玩家"
        };

        private void Awake()
        {
            // 依赖注入事件系统
            playerModel = new PlayerModel(ModelEventSystem);
        }


        private void Start()
        {
            // 程序启动时加入本机玩家
            // 暂时用实例化代替
            Player player = Instantiate(pfPlayer, transform).GetComponent<Player>();
            player.Init(new PlayerInfo(1, "Player 1"));
            TryAddPlayer(player);
        }

        /// <summary>
        /// 设置当前回合的玩家
        /// </summary>
        /// <param name="player"></param>
        public void SetCurrentPlayerTurn(Player player)
        {
        }

        #region 玩家管理
        public bool TryAddPlayer(Player player)
        {
            return playerModel.TryAddPlayer(player);
        }

        public bool TryRemovePlayer(Player player)
        {
            return playerModel.TryRemovePlayer(player);
        }

        /// <summary>
        /// 添加AI玩家
        /// </summary>
        /// <param name="difficulty">AI难度</param>
        /// <returns>是否添加成功</returns>
        public bool TryAddAIPlayer(AIDifficulty difficulty = AIDifficulty.Normal)
        {
            if (pfAIPlayer == null)
            {
                HLogger.Log("AI玩家预制体未设置");
                return false;
            }

            // 生成AI玩家ID和名称
            int aiPlayerId = playerModel.PlayerCount + 1;
            string aiPlayerName = GetRandomAIPlayerName();

            // 创建AI玩家实例
            AIPlayer aiPlayer = Instantiate(pfAIPlayer, transform).GetComponent<AIPlayer>();
            aiPlayer.Init(new PlayerInfo(aiPlayerId, aiPlayerName), difficulty);

            // 添加到玩家模型
            bool success = playerModel.TryAddPlayer(aiPlayer);

            if (success)
            {
                HLogger.LogSuccess($"成功添加AI玩家: {aiPlayerName} (难度: {difficulty})");
            }
            else
            {
                HLogger.Log($"添加AI玩家失败: {aiPlayerName}");
                Destroy(aiPlayer.gameObject);
            }

            return success;
        }

        /// <summary>
        /// 获取随机的AI玩家名称
        /// </summary>
        /// <returns>AI玩家名称</returns>
        private string GetRandomAIPlayerName()
        {
            int index = UnityEngine.Random.Range(0, aiPlayerNames.Length);
            return aiPlayerNames[index];
        }
        #endregion
    }
}
