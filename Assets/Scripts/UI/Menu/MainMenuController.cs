using System;
using Mirror;
using Network;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NetworkManager = Network.NetworkManager;
using UnityEngine.EventSystems;
using EventSystem = Events.EventSystem;

namespace UI
{
    public class MainMenuController : MonoBehaviour
    {
        public NetworkManager networkManager;
        
        [Header("Connect Window")]
        public TMP_InputField ipAddress;
        public GameObject connectWindow;

        [Header("MenuControls")]
        public GameObject optionsScreen;
        public GameObject optionsFirstButton,
        optionsClosedButton;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.None;
        }

        public void QuitApplication()
        {
            Application.Quit();
        }

        public void JoinGame()
        {
            var builder = new UriBuilder("kcp", ipAddress.text);
            networkManager.StartClient(builder.Uri);
        }

        public void HostGame()
        {
            networkManager.StartHost();
        }

        public void ToggleConnectWindow()
        {
            connectWindow.SetActive(!connectWindow.activeInHierarchy);
        }

        public void OpenOption()
        {
            optionsScreen.SetActive(true);

            //clear selected object
            EventSystem.current.SetSelectedGameObject(null);
            //set new selected object
            EventSystem.current.SetSelectedGameObject(optionsFirstButton);
        }

        public void CloseOption()
        {
            optionsScreen.SetActive(false);

            //clear selected object
            EventSystem.current.SetSelectedGameObject(null);
            //set new selected object
            EventSystem.current.SetSelectedGameObject(optionsClosedButton);
        }
    }
}
