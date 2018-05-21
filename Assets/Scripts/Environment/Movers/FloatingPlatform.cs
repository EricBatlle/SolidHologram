using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPlatform : MonoBehaviour {

    [SerializeField] protected Mover mover;
    [Tooltip("First sprite should be the Up sprite, second down")]
    [SerializeField] protected Sprite[] sprites;
    [SerializeField] private CustomTrigger startTrigger;

    private int spritesCount = 0;
    private bool moving = false;
    public float moveEvery = 2.0f;

    private void OnEnable()
    {
        if(startTrigger != null)
            startTrigger.OnEnter = StartMovingPlatform;
    }
    private void OnDisable()
    {
        if (startTrigger != null)
            startTrigger.OnEnter = null;
    }
    
    void StartMovingPlatform()
    {
        if (!moving)        
            InvokeRepeating("movePlatform", 2.0f, moveEvery);
        
        moving = true;
    }

    void movePlatform()
    {
        mover.Move();
        setNextSprite();
    }

    void setNextSprite()
    {
        if (spritesCount >= sprites.Length)
        {
            spritesCount = 0;
        }
        this.GetComponent<SpriteRenderer>().sprite = sprites[spritesCount];
        spritesCount++;
    }
}
