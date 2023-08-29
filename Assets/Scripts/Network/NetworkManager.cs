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
    public class NetworkManager : NetworkRoomManager
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

        public override void OnRoomServerSceneChanged(string sceneName)
        {
            if (sceneName == GameplayScene)
            {
                // get InventoryManager & spawn test items
                if (ScanForInventoryManager()) _inventoryManager.GetComponent<ItemManager>().SpawnItems();
                
                // Hand out first quest
                if (QuestManager.Current != null) {
                    QuestManager.Current.AddQuest(QuestManager.Current.startQuest);
                    QuestManager.Current.SelectQuest(QuestManager.Current.startQuest);
                }

                RemoveDebugCam();
            }
            base.OnRoomServerSceneChanged(sceneName);
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
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

        public RoomPlayer GetRoomHost()
        {
            if (roomSlots.Count > 0)
            {
                return roomSlots[0] as RoomPlayer;
            }

            return null;
        }
        
        public RoomPlayer GetRoomClient()
        {
            if (roomSlots.Count > 1)
            {
                return roomSlots[1] as RoomPlayer;
            }

            return null;
        }
    }
}
