using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class NetworkManager : Mirror.NetworkManager
{

    
    [Header("Only for Testing")]
    public GameObject rock;
    public Transform[] stoneSpawns;
    public GameObject stick;
    public Transform[] stickSpawns;
    
    private ServerInventory _inventoryManager;
    
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
        base.OnServerAddPlayer(conn);
        ScanForInventoryManager();
        uint networkIdentifier = 0;
        foreach (var identity in conn.owned)
        {
            if (identity.GetComponent<ClientInventory>())
            {
                networkIdentifier = identity.netId;
            }
        }
        _inventoryManager.RegisterInventory(networkIdentifier);
    }

    void ScanForInventoryManager()
    {
        var inventoryManagerGo = GameObject.Find("ItemManager");
        if (inventoryManagerGo) {
            _inventoryManager = inventoryManagerGo.GetComponent<ServerInventory>();
        }
        else
        {
            Debug.Log("No inventory manager found yet!");
        }
    }
}
