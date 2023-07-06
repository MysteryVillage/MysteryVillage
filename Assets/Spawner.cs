using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

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
