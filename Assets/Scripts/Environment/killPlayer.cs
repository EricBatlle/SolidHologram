using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killPlayer : MonoBehaviour {

	private Vector3 initialPos;
    [SerializeField]private bool debugMode = false;
	// Use this for initialization
	void Start () {
        if (debugMode) Destroy(this);
		initialPos = this.transform.position;
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
            collision.transform.position = GameObject.FindGameObjectWithTag("Spawn").transform.position;
			this.transform.position = initialPos; //just for the BouncyCapsule?
		}
    }
}
