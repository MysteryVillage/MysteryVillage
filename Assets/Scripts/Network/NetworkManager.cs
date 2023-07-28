using System.Collections.Generic;
using Inventory;
using Mirror;
using Player;
using Quests;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Network
{
    public class NetworkManager : Mirror.NetworkManager
    {
        [Header("Loading Screen")] 
        public Camera loadingCam;
        public GameObject loadingScreen;

        private InventoryManager _inventoryManager;

        public GameObject debugCam;
        
        // Overrides the base singleton so we don't
        // have to cast to this type everywhere.
        public static new NetworkManager singleton { get; private set; }

        /// <summary>
        /// Runs on both Server and Client
        /// Networking is NOT initialized when this fires
        /// </summary>
        public override void Awake()
        {
            base.Awake();
            singleton = this;
        }
        
        public override void OnClientChangeScene(string newSceneName, SceneOperation sceneOperation, bool customHandling)
        {
            Debug.Log("OnClientChangeScene");
            base.OnClientChangeScene(newSceneName, sceneOperation, customHandling);
            loadingCam.gameObject.SetActive(true);
            loadingScreen.SetActive(true);
        }

        public void HideLoadingScreen()
        {
            loadingCam.gameObject.SetActive(false);
            loadingScreen.SetActive(false);
        }
    
        public override void OnStartClient()
        {
            base.OnStartClient();
        
            // get InventoryManager & spawn test items
            if (ScanForInventoryManager()) _inventoryManager.GetComponent<ItemManager>().SpawnItems();
            
            // Hand out first quest
            QuestManager.Current.AddQuest(QuestManager.Current.startQuest);
            QuestManager.Current.SelectQuest(QuestManager.Current.startQuest);
            
            RemoveDebugCam();
        }

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            // @TODO: Remove hack to fix player input devices
            playerPrefab.GetComponent<PlayerInput>().neverAutoSwitchControlSchemes = true;
            if (conn.connectionId != 0)
            {
                playerPrefab.GetComponent<PlayerInput>().defaultControlScheme = "Gamepad";
                playerPrefab.GetComponent<Player.GeometryController>().character = "Boy";
                playerPrefab.GetComponent<PlayerController>().isBoy = true;
            }
            else
            {
                playerPrefab.GetComponent<PlayerInput>().defaultControlScheme = "KeyboardMouse";
                playerPrefab.GetComponent<Player.GeometryController>().character = "Girl";
                playerPrefab.GetComponent<PlayerController>().isBoy = false;
            }

            base.OnServerAddPlayer(conn);

            // Register player inventory
            ScanForInventoryManager();
            uint networkIdentifier = 0;
            foreach (var identity in conn.owned)
            {
                if (identity.GetComponent<PlayerInventory>())
                {
                    networkIdentifier = identity.netId;
                }
            }
            _inventoryManager.RegisterInventory(networkIdentifier);
        }

        bool ScanForInventoryManager()
        {
            var inventoryManagerGo = GameObject.Find("ItemManager");
            if (inventoryManagerGo) {
                _inventoryManager = inventoryManagerGo.GetComponent<InventoryManager>();
            }
            else
            {
                Debug.Log("No inventory manager found yet!");
            }

            return (inventoryManagerGo);
        }

        public override void OnStopHost()
        {
            ScanForInventoryManager();
            if (NetworkServer.active) _inventoryManager.ClearInventories();
        }

        public void RemoveDebugCam()
        {
            // disable debug cam
            if (debugCam == null)
            {
                debugCam = GameObject.Find("DebugCam");
                
            }
            if (debugCam != null)
            {
                debugCam.SetActive(false);
            }
        }
    }
}
