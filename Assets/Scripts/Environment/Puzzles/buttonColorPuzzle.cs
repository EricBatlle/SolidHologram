using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonColorPuzzle : doorTrigger {

	private bool allEquals = true;
	public Material component;
	// Use this for initialization
	void Start () {
		//if all childs...
		components = transform.GetComponentsInChildren<changeColor>();
	}
	
	// Update is called once per frame
	void Update () {
		allEquals = true;
		//...have the same color
		foreach (changeColor ccScript in components) {
			if(ccScript.gameObject.GetComponent<MeshRenderer> ().sharedMaterial != component){
				allEquals = false;
			}
		}
		//startMoving
		if(allEquals){
			startMoving = true;
		}
	}

	public override void OnTriggerEnter2D(Collider2D collision){
	
	}
}
