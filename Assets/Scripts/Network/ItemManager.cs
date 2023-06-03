using Items;
using Mirror;
using UnityEngine;

namespace Network
{
    public class ItemManager : NetworkBehaviour
    {
        [Header("Only for Testing")]
        public GameObject rock;
        public Transform[] stoneSpawns;
        public GameObject stick;
        public Transform[] stickSpawns;
        
        private static ItemManager _instance;

        private ItemManager()
        {
            _instance = this;
        }

        public static ItemManager Instance()
        {
            return _instance;
        }
    
        [Server]
        public void Spawn(int itemId, Vector3 position)
        {
            Spawn(itemId, position, Quaternion.Euler(Vector3.one * Random.value * 360.0f));
        }
    
        [Server]
        public void Spawn(int itemId, Vector3 position, Quaternion rotation)
        {
            var item = ItemData.FindById(itemId);
            Debug.Log(itemId);
            Debug.Log(position);
            var newItem = Instantiate(item.dropPrefab, position, rotation);
            NetworkServer.Spawn(newItem);
        }

        public void SpawnItems()
        {
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
        }
    }
}
