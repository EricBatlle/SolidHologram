using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PuzzleButton : NetworkInteractiveObject
{
    [SerializeField] private bool worksWithPressure = false;
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
		if (collision.gameObject.CompareTag ("Player") || collision.gameObject.CompareTag ("line") || (collision.gameObject.CompareTag("Pusher"))) {
            nextColor();
		}
	}

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (worksWithPressure)
        {
            if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("line") || (collision.gameObject.CompareTag("Pusher")))
            {
                nextColor();
            }
        }        
    }

    //Set Next Color
    #region nextColor
    public void nextColor()
    {
        if (isServer)
        {
            RpcNextColor();
        }
        //else
        //{
        //    if (!hasAuthority)
        //    {
        //        this.OnInteraction += NmCmdNextColor;
        //        setLocalAuthority();
        //    }
        //    else
        //    {
        //        CmdNextColor();
        //    }
        //}
    }

    private void NmCmdNextColor()
    {
        CmdNextColor();
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

        if(OnColorChange != null)
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
            if (!hasAuthority)
            {
                this.OnInteraction += NmCmdResetButton;
                setLocalAuthority();
            }
            else
            {
                CmdResetButton();
            }
        }
    }
    void NmCmdResetButton()
    {
        CmdResetButton();
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
