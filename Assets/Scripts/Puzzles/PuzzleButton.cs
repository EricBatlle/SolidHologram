using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PuzzleButton : NetworkBehaviour, IInteractable {
    
    [SerializeField] private Sprite[] sprites;
    private int spritesCount = 0;

    public Sprite currSprite;
    public event Action OnColorChange;

    private void Start()
    {
        currSprite = this.GetComponent<SpriteRenderer>().sprite;
    }

    private void OnTriggerEnter2D(Collider2D collision){
		if (collision.gameObject.CompareTag ("Player") || collision.gameObject.CompareTag ("line")) {
            PlayerInteraction();
		}
	}
    
    public void PlayerInteraction()
    {
        if (isServer)
        {
            RpcNextColor();
        }
        else
        {
            CmdNextColor(); 
        }
    }

    //Set Next Color
    private void nextColor()
    {
        if (spritesCount >= sprites.Length)
        {
            spritesCount = 0;
        }
        this.GetComponent<SpriteRenderer>().sprite = sprites[spritesCount];
        currSprite = sprites[spritesCount];
        spritesCount++;

        OnColorChange();
    }

    [Command]
    private void CmdNextColor()
    {
        RpcNextColor(); 
    }
    [ClientRpc]
    private void RpcNextColor()
    {
        nextColor();
    }
}
