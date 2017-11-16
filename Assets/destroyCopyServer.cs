using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class destroyCopyServer : NetworkBehaviour {

    // Use this for initialization
    private void Start()
    {
        if (!isLocalPlayer)
        {
            Destroy(this);
            return;
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
