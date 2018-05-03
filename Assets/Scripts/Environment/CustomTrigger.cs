using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTrigger : MonoBehaviour {

    public Action OnEnter = null;
    public Action OnExit = null;

    //start behaviour
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(OnEnter != null)                            
                OnEnter();                            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(OnExit != null)
                OnExit();
        }
    }
}
