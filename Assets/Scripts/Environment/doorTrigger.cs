﻿using System.Collections;
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
		//buscas puerta
		//buscas move
		//llamas a move en direccion de este trigger


        if (collision.gameObject.CompareTag("Player"))
        {
            startMoving = true;
        }

    }
}