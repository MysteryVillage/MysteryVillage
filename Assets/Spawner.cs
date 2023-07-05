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
        Instantiate(item, spawnPosition.transform.position, transform.rotation, transform);
    }
}
