using System.Collections.Generic;
using Inventory;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Network
{
    public class NetworkManager : Mirror.NetworkManager
    {
        private InventoryManager _inventoryManager;
    
        public override void OnStartClient()
        {
            base.OnStartServer();
        
            // get InventoryManager & spawn test items
            if (ScanForInventoryManager()) _inventoryManager.GetComponent<ItemManager>().SpawnItems();
        }

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            // @TODO: Remove hack to fix player input devices
            playerPrefab.GetComponent<PlayerInput>().neverAutoSwitchControlSchemes = true;
            if (conn.connectionId != 0)
            {
                playerPrefab.GetComponent<PlayerInput>().defaultControlScheme = "Gamepad";
                playerPrefab.GetComponent<Player.GeometryController>().character = "Boy";
            }
            else
            {
                playerPrefab.GetComponent<PlayerInput>().defaultControlScheme = "KeyboardMouse";
                playerPrefab.GetComponent<Player.GeometryController>().character = "Girl";
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

        private void CheckScene(int sceneId)
        {
            var id = SceneManager.GetActiveScene().handle;
            if (id != sceneId)
            {
                SceneManager.LoadScene(sceneId);
            }
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
    }
}
