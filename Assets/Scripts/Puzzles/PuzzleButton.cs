using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PuzzleButton : NetworkBehaviour {
    
    [SerializeField] private Sprite[] sprites;
    private int spritesCount = 0;

    public Sprite currSprite;
    private Sprite startSprite;
    public event Action OnColorChange;

    private void Start()
    {
        currSprite = this.GetComponent<SpriteRenderer>().sprite;
        startSprite = currSprite;
    }

    //Change color when enter player or draw
    private void OnTriggerEnter2D(Collider2D collision){
		if (collision.gameObject.CompareTag ("Player") || collision.gameObject.CompareTag ("line")) {
            nextColor();
		}
	}

    //Set Next Color
    #region nextColor
    private void nextColor()
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

    [Command]
    private void CmdNextColor()
    {
        RpcNextColor(); 
    }
    [ClientRpc]
    private void RpcNextColor()
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
    #endregion

    //Reset Button
    #region resetButton
    public void resetButton()
    {
        if (isServer)
        {
            RpcResetButton();
        }
        else
        {
            //RpcResetButton();
            currSprite = startSprite;
            this.GetComponent<SpriteRenderer>().sprite = currSprite;
            spritesCount = 0;
        }
    }

    [Command]
    void CmdResetButton()
    {
        RpcResetButton();
    }

    [ClientRpc]
    void RpcResetButton()
    {
        currSprite = startSprite;
        this.GetComponent<SpriteRenderer>().sprite = currSprite;
        spritesCount = 0;
    }
    #endregion
}
