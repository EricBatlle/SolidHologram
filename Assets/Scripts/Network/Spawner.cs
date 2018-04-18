using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Spawner : NetworkBehaviour {

    [SerializeField] private GameObject cubePrefab;

    public override void OnStartServer()
    {
        base.OnStartServer();
        GameObject cube = Instantiate(cubePrefab, transform.position, Quaternion.identity) as GameObject; //SpawnWithClientAuthority WORKS JUST LIKE NetworkServer.Spawn ...THE
                                                                                                          //GAMEOBJECT MUST BE INSTANTIATED FIRST.

        //NetworkServer.SpawnWithClientAuthority(cube, gameObject); //THIS WILL SPAWN THE troop THAT WAS CREATED ABOVE AND GIVE AUTHORITY TO THIS PLAYER. THIS PLAYER (GAMEOBJECT) MUST
        NetworkServer.Spawn(cube); //THIS WILL SPAWN THE troop THAT WAS CREATED ABOVE AND GIVE AUTHORITY TO THIS PLAYER. THIS PLAYER (GAMEOBJECT) MUST
    }
}
