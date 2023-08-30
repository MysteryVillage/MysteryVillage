using Mirror;
using UnityEngine;

namespace Items
{
    public class Spawner : NetworkBehaviour
    {
        public GameObject item;
        public Transform spawnPosition;
    
        public void SpawnItem()
        {
            var itemObject = Instantiate(item, spawnPosition.transform.position, transform.rotation);
            NetworkServer.Spawn(itemObject);
        }
    }
}
