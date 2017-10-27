using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class networkSelector : NetworkBehaviour {

	GameObject box;
	GameObject bentley;
	public GameObject camObject;


	// Use this for initialization
	void Start () {
		camObject = GameObject.Find ("Main Camera");

		foreach (Transform child in transform) {
			if (child.gameObject.CompareTag("Bentley")) {	
				bentley = child.gameObject;
			}
			if (child.gameObject.CompareTag("Player")) {	
				box = child.gameObject;
			}
		}


		//codigo para saber si soy host o no
		if(isServer){
			if (isLocalPlayer) {
				print ("I'm the server"); // and I'm the platformer
				bentley.SetActive (false);
				PlayerCamera pc = camObject.GetComponent<PlayerCamera> ();
				pc.setPlayer (box);
			} else {
				box.SetActive(false);
				bentley.SetActive (true);
			}
		}

		if(!isServer){
			if (isLocalPlayer) {
				box.SetActive (false);
				bentley.SetActive (true);
			} else {
			//	box.SetActive (true);
			//	bentley.SetActive (false);
			}
			print("I'm the client");

		}

		//if (!isLocalPlayer)
			//Destroy (gameObject);
		//bentley.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
