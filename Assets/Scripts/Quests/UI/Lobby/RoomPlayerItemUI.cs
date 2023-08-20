using System;
using Player;
using TMPro;
using UnityEngine;

namespace Quests.UI.Lobby
{
    public class RoomPlayerItemUI : MonoBehaviour
    {
        [Header("Configuration")]
        public RoomPlayer roomPlayer;
        
        [Header("UI Elements")]
        public TMP_Text readyButton;
        public TMP_InputField nameInput;
        public TMP_Dropdown characterSelect;

        [Header("Appearance Types")] 
        public GameObject localPlayer;
        public GameObject networkPlayer;

        private void Start()
        {
            // Hide UI before players join
            localPlayer.SetActive(false);
            networkPlayer.SetActive(false);
        }

        public void ToggleReady()
        {
            var ready = !roomPlayer.readyToBegin;
            roomPlayer.CmdChangeReadyState(ready);

            if (ready)
            {
                readyButton.text = "Nicht bereit";
                
                nameInput.interactable = false;
                characterSelect.interactable = false;
            }
            else
            {
                readyButton.text = "Bereit";
                
                nameInput.interactable = true;
                characterSelect.interactable = true;
            }
        }

        public void UpdatePlayerName()
        {
            var value = nameInput.text;
            roomPlayer.SetPlayerName(value);
        }

        public void UpdateCharacterChoice()
        {
            var value = characterSelect.captionText.text;
            roomPlayer.SetCharacter(value);
        }

        public void UpdateAppearance()
        {
            if (roomPlayer.isLocalPlayer)
            {
                networkPlayer.SetActive(false);
                localPlayer.SetActive(true);
            }
            else
            {
                networkPlayer.SetActive(true);
                localPlayer.SetActive(false);
            }
        }
    }
}