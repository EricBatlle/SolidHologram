using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class cameraRunnerSize : NetworkBehaviour {

	public float size = 7;

	// Use this for initialization
	void Start () {
		//Only change 
		if ((isServer))
			return;

		Camera.main.GetComponent<Camera>().orthographicSize = size;
	}

	// Update is called once per frame
	void Update () {

	}
}
