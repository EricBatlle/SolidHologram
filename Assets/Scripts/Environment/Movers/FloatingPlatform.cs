using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPlatform : MonoBehaviour {

    [SerializeField] private Mover mover;
    [SerializeField] private Sprite[] sprites;
    private int spritesCount = 0;

    public float moveEvery = 2.0f;

    void Start()
    {
        InvokeRepeating("movePlatform", 2.0f, moveEvery);
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
