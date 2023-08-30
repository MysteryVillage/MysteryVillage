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
        public TMP_Text errorMessage;

        public TMP_Text playerNameDisplay;
        public TMP_Text characterDisplay;
        public TMP_Text readyDisplay;

        public TMP_Text localHeadline;
        public TMP_Text networkHeadline;

        [Header("Appearance Types")] 
        public GameObject localPlayer;
        public GameObject networkPlayer;

        private RoomPlayerUI _roomPlayerUI;

        private void Start()
        {
            _roomPlayerUI = GetComponentInParent<RoomPlayerUI>();
            
            // Hide UI before players join
            localPlayer.SetActive(false);
            networkPlayer.SetActive(false);

            string headline;

            if (transform.GetSiblingIndex() != 0)
            {
                headline = "Spieler 2";
            }
            else
            {
                headline = "Spieler 1 (Host)";
            }

            localHeadline.text = headline;
            networkHeadline.text = headline;
        }

        private void Update()
        {
            if (roomPlayer == null)
            {
                localPlayer.SetActive(false);
                networkPlayer.SetActive(false);
            }
            else
            {
                if (!roomPlayer.isLocalPlayer)
                {
                    playerNameDisplay.text = roomPlayer.playerName;
                    characterDisplay.text = roomPlayer.character;
                    readyDisplay.text = roomPlayer.readyToBegin ? "Bereit" : "";
                }
                else
                {
                    ValidateForm();
                }
            }
        }

        private void ValidateForm()
        {
            if (!_roomPlayerUI.PlayerNamesAreDifferent())
            {
                errorMessage.text = "Die Spielernamen müssen sich unterscheiden.";
                if (roomPlayer.readyToBegin) ToggleReady();
            } else if (!_roomPlayerUI.CharacterChoiceIsValid()) {
                errorMessage.text = "Ihr müsst zwei unterschiedliche Charaktere wählen.";
                if (roomPlayer.readyToBegin) ToggleReady();
            } else if (!roomPlayer.HasValidCharacter() || roomPlayer.playerName == "")
            {
                errorMessage.text = "Bitte wähle einen Charakter aus und gebe dir einen Namen.";
            }
            else
            {
                errorMessage.text = "";
            }
        }

        public void ToggleReady()
        {
            var ready = !roomPlayer.readyToBegin;

            if (!roomPlayer.HasValidCharacter() || roomPlayer.playerName == "")
            {
                ready = false;
            }
            else
            {
                errorMessage.text = "";
            }
            
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