using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openDoor : MonoBehaviour {

	private bool startMoving = false;
	//public float maxX = 0;
	public float maxY = 0;
	//public float maxZ = 0;

	private Vector3 destination;
	// Use this for initialization
	void Start () {
		//Vector3 auxVec = new Vector3 (maxX,maxY,maxZ);
		//destination = this.transform.position + auxVec;
		destination.y = this.transform.position.y + maxY;

	}

	// Update is called once per frame
	void Update () {
		
		if( (startMoving == true) && (this.transform.position.y <= destination.y)){
			transform.Translate(Vector3.up * Time.deltaTime);
		}
	}

	//killPlayer behaviour
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			startMoving = true;
		}

	}

}
