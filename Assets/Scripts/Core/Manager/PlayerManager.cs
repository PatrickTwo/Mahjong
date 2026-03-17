using System.Collections;
using System.Collections.Generic;
using Mahjong.System.TypeEventSystem;
using UnityEngine;

namespace Mahjong
{
    /// <summary>
    /// 玩家管理器
    /// </summary>
    public class PlayerManager : MonoBehaviour
    {
        private IEventSystem ModelEventSystem => EventSystemManager.Instance.ModelEventSystem;
        private readonly PlayerModel playerModel = new();

        private void Awake()
        {
            // 程序启动时加入本机玩家
        }

        public bool TryAddPlayer(Player player)
        {
            ModelEventSystem.Send(new AddPlayerEvent(player));
            return playerModel.TryAddPlayer(player);
        }
        public bool TryRemovePlayer(Player player)
        {
            ModelEventSystem.Send(new RemovePlayerEvent(player));
            return playerModel.TryRemovePlayer(player);
        }
    }
}
