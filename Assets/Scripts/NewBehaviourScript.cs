using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NewBehaviourScript : NetworkBehaviour {

	public string sceneName = "PostPrototype_v2";

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//only the server can swap scene
		if (!isServer)
			return;
		
		if (Input.GetMouseButtonDown (0)) {
			NetworkManager.singleton.ServerChangeScene (sceneName);
		}
	}
}
