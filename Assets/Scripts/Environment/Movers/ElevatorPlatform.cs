using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorPlatform : FloatingPlatform
{
	
	// Update is called once per frame
	void Update () {
        if (mover.open)
        {
            this.GetComponent<SpriteRenderer>().sprite = sprites[0];
        }
        else
        {
            this.GetComponent<SpriteRenderer>().sprite = sprites[1];
        }
    }
}
