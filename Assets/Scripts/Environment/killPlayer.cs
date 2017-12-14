using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killPlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    //killPlayer behaviour
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Kill
            //collision.gameObject.SetActive(false);
            //Respawn
            collision.transform.position = GameObject.FindGameObjectWithTag("Spawn").transform.position;
            
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            print(this);
            
            collision.transform.position = GameObject.FindGameObjectWithTag("Spawn").transform.position;
        }
    }
}
