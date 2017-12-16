using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorTrigger : MonoBehaviour {

	public enum Direction
	{
		Up,
		Down,
		Right,
		Left
	}
    public bool startMoving = false;
	public Direction dir;
	public Component[] components;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
       
		//if the door is moving, we need to stop the trigger startMoving or the door update will never stop recovering this info
		if(transform.GetComponentInParent<moveTo>().startMoving == true){
			startMoving = false;
			//enabled = false; -> this could improve performance but if the player go back and forward through triggers could stop the door
		}
        
	}

    //Start moving when Player touch the trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
			//Find other moveTo scripts on the parent
			components = transform.GetComponentsInParent<moveTo>();
			//Stop all of them
			foreach (moveTo mtScript in components) {
				mtScript.startMoving = false;
			}
			//Switch on the moveTo associated with this trigger
            startMoving = true;
        }

    }
}
