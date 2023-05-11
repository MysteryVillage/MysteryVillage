using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class NetworkManager : Mirror.NetworkManager
{

    public GameObject rock;
    private InventoryManager _inventoryManager;
    
    public override void OnStartClient()
    {
        print("NetworkManager:StartClient");
        base.OnStartServer();
        
        // Test item
        if (NetworkServer.active) {
            var newSceneObject = Instantiate(rock, Vector3.one, Quaternion.Euler(0,0,0));
            NetworkServer.Spawn(newSceneObject);
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
            print(identity.netId);
            if (identity.GetComponent<Inventory>())
            {
                networkIdentifier = identity.netId;
            }
        }
        _inventoryManager.RegisterInventory(networkIdentifier);
    }

    void ScanForInventoryManager()
    {
        var inventoryManagerGo = GameObject.Find("InventoryManager");
        if (inventoryManagerGo) {
            _inventoryManager = inventoryManagerGo.GetComponent<InventoryManager>();
        }
        else
        {
            Debug.Log("No inventory manager found yet!");
        }
    }
}
