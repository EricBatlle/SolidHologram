using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressionButton : PuzzleButton {

    //Avoid calling the puzzlebutton ontriggerenter2d schema
    private void OnTriggerEnter2D(Collider2D collision)
    {
    }
}
