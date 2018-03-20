using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drip : MonoBehaviour {

    public GameObject dropPrefab;
	public bool random_0_dropEvery = false;
    public float dropEvery = 2.0f;
    public float startDropping = 0.0f;

	// Use this for initialization
	void Start () {
        //Every 2 seconds, spawn a drop
		if (random_0_dropEvery == true) {
			dropEvery = Random.Range (0.5f,dropEvery);
			InvokeRepeating("dripDrop", startDropping, dropEvery);

		} else {
			InvokeRepeating("dripDrop", startDropping, dropEvery);
		}

    }
	
	// Update is called once per frame
	void Update () {
        
	}

    public void dripDrop()
    {
        var drop = (GameObject)Instantiate(dropPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation);

		drop.transform.SetParent(transform);
	}
}
