using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class respawnPlayer : NetworkBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

 

    //[ClientRpc]
    //void RpcRespawn(Collider2D collision)
    //{
    //    if (isLocalPlayer)
    //    {
    //        print("transform");
    //        // move back to zero location
    //        collision.gameObject.transform.position = Vector3.zero;
    //    }
    //}
}
