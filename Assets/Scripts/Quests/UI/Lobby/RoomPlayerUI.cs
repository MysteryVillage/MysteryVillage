using Player;
using UnityEngine;

namespace Quests.UI.Lobby
{
    public class RoomPlayerUI : MonoBehaviour
    {
        public GameObject roomPlayerUiPrefab;
        private Network.NetworkManager _networkManager;
        public RoomPlayerItemUI[] playerSlots;

        private void Awake()
        {
            _networkManager = Network.NetworkManager.singleton;
        }

        private void Update()
        {
            var players = _networkManager.roomSlots;
            foreach (var player in players)
            {
                var slot = playerSlots[player.index];
                slot.roomPlayer = player as RoomPlayer;
                
                // Show either form or just player info
                slot.UpdateAppearance();
            }
        }
    }
}