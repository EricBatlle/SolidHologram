using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeColor : MonoBehaviour {

	public Material[] materials;

	private Material material;
	private int i= 0;

	// Use this for initialization
	void Start () {

	}

	private void nextColor(){
		if (i >= materials.Length) {
			i = 0;
		}
		this.GetComponent<MeshRenderer> ().material = materials [i];
		i++;
	}

	public void OnTriggerEnter2D(Collider2D collision){
		if (collision.gameObject.CompareTag ("Player")) {
			nextColor ();
		}
	}


}
