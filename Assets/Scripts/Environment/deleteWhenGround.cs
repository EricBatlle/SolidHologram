using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deleteWhenGround : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Kill
            //collision.gameObject.SetActive(false);
            //Respawn
            collision.transform.position = GameObject.FindGameObjectWithTag("Spawn").transform.position;
            ;
        }
        if ((collision.gameObject.CompareTag("line")) || (collision.gameObject.CompareTag("Wall")))
        {
            //Auto-Destroy
            Destroy(gameObject);
        }

    }
}
