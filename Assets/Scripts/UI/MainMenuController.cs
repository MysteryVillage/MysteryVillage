using System;
using Mirror;
using Network;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NetworkManager = Network.NetworkManager;

namespace UI
{
    public class MainMenuController : MonoBehaviour
    {
        public NetworkManager networkManager;
        
        [Header("Connect Window")]
        public TMP_InputField ipAddress;
        public GameObject connectWindow;
        
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
    }
}
