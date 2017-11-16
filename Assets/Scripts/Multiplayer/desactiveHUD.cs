using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class desactiveHUD : NetworkBehaviour {

    GameObject server;
    NetworkManagerHUD hudManager;

    // Use this for initialization
    void Start () {
        server = GameObject.Find("Network Manager");
        hudManager = server.GetComponent<NetworkManagerHUD>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q)) { 
            print("desactive hud");
            hudManager.showGUI = false;
        }
    }
}
