using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class NetworkManager : Mirror.NetworkManager
{

    public GameObject rock;
    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log("Spawn rock");
        var newSceneObject = Instantiate(rock, Vector3.one, Quaternion.Euler(0,0,0));
        NetworkServer.Spawn(newSceneObject);
    }
}
