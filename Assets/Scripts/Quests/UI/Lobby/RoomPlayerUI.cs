using System;
using Mirror;
using Network;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Quests.UI.Lobby
{
    public class RoomPlayerUI : NetworkBehaviour
    {
        private Network.NetworkManager _networkManager;
        private GameSettings _settings;
        public RoomPlayerItemUI[] playerSlots;
        
        [Header("UI Elements")]
        public GameObject settingsPanel;
        public TMP_Dropdown gamemodeDropdown;
        public Button readyButton;

        private void Awake()
        {
            _networkManager = Network.NetworkManager.singleton;
            _settings = GameSettings.Get();
        }

        private void Start()
        {
            if (!isServer)
            {
                settingsPanel.SetActive(false);
            }
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

        public void OnChangeGamemode()
        {
            _settings.isLocalGame = gamemodeDropdown.value != 1;
        }

        public void StartGame()
        {
            // start game ..
        }

        public bool PlayerNamesAreDifferent()
        {
            if (playerSlots[0].roomPlayer != null && playerSlots[1].roomPlayer != null)
            {
                return playerSlots[0].roomPlayer.playerName != playerSlots[1].roomPlayer.playerName;
            }

            return true;
        }

        public bool CharacterChoiceIsValid()
        {
            if (playerSlots[0].roomPlayer != null && playerSlots[1].roomPlayer != null)
            {
                return (playerSlots[0].roomPlayer.character == "Collin" && playerSlots[1].roomPlayer.character == "Alina") || (playerSlots[1].roomPlayer.character == "Collin" && playerSlots[1].roomPlayer.character == "Alina");
            }

            return true;
        }
    }
}