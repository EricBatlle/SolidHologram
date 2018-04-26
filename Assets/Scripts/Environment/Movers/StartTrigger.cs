using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class StartTrigger : MonoBehaviour {

    [SerializeField] ActivableObject startObject;

    //start behaviour
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            startObject.StartBehaviour();
        }
    }
}
