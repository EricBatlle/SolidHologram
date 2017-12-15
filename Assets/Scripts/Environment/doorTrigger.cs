using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorTrigger : MonoBehaviour {


    public bool startMoving = false;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
       
        if (startMoving)
        {
            enabled = false;
        }
        
	}

    //Start moving when Player touch the trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            startMoving = true;
        }

    }
}
