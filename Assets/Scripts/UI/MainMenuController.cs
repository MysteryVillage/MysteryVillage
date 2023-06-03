using System;
using Mirror;
using Network;
using UnityEngine;
using NetworkManager = Network.NetworkManager;

namespace UI
{
    public class MainMenuController : MonoBehaviour
    {
        public NetworkManager networkManager;
        public void QuitApplication()
        {
            Application.Quit();
        }

        public void JoinGame()
        {
            // @TODO: Manually set join ip
            networkManager.StartClient();
        }

        public void HostGame()
        {
            networkManager.StartHost();
        }
    }
}
