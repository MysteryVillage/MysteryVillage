using Inventory;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Network
{
    public class NetworkManager : Mirror.NetworkManager
    {

    
        [Header("Only for Testing")]
        public GameObject rock;
        public Transform[] stoneSpawns;
        public GameObject stick;
        public Transform[] stickSpawns;
    
        private InventoryManager _inventoryManager;
    
        public override void OnStartClient()
        {
            print("NetworkManager:StartClient");
            base.OnStartServer();
        
            // Test items
            if (NetworkServer.active) {
                foreach (Transform spawn in stoneSpawns)
                {
                    var newSceneObject = Instantiate(rock, spawn.position, Quaternion.Euler(0,0,0));
                    NetworkServer.Spawn(newSceneObject);                
                }
                foreach (Transform spawn in stickSpawns)
                {
                    var newSceneObject = Instantiate(stick, spawn.position, Quaternion.Euler(0,0,0));
                    NetworkServer.Spawn(newSceneObject);                
                }
            }
        
            // get InventoryManager
            ScanForInventoryManager();
        }

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            print("NetworkManager:ServerConnect");
            
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
            
            // check if scenes match
            CheckScene(SceneManager.GetActiveScene().handle);
        
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

        void ScanForInventoryManager()
        {
            var inventoryManagerGo = GameObject.Find("ItemManager");
            if (inventoryManagerGo) {
                _inventoryManager = inventoryManagerGo.GetComponent<InventoryManager>();
            }
            else
            {
                Debug.Log("No inventory manager found yet!");
            }
        }

        public override void OnStopHost()
        {
            ScanForInventoryManager();
            if (NetworkServer.active) _inventoryManager.ClearInventories();
        }
    }
}
