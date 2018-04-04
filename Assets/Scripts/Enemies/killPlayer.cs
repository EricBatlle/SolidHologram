using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class killPlayer : MonoBehaviour {

    [SerializeField]private bool debugMode = false;
    public event Action OnKill;

    #region setPlayersReference variables
    private LineDraw_Net bentley;
    private PlatformerCharacter2D box;

    private bool findBoxReference = false;
    private bool findBentleyReference = false;
#endregion
    
    // Use this for initialization
    void Start () {
        if (debugMode) Destroy(this);
    }
    
    //Used to set players reference
    private void FixedUpdate()
    {
        //Set Players References       
        if(!findBoxReference)
        {
            GameObject boxGO = GameObject.FindGameObjectWithTag("Player");
            if (boxGO != null)
            {
                box = boxGO.GetComponent<PlatformerCharacter2D>();
                findBoxReference = true;
            }            
        }
        if(!findBentleyReference)
        {
            GameObject bentleyGO = GameObject.FindGameObjectWithTag("Bentley");
            if (bentleyGO != null)
            {
                bentley = bentleyGO.GetComponent<LineDraw_Net>();
                findBentleyReference = true;
            }
        }
    }

    //killPlayer behaviour
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Remove all draws
            if (bentley != null)
                bentley.destroyAllLines();
            //Call the functions suscribed to the action called when the player dies
            if (box != null)
                box.PlayerKilled();
        }        
    }    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            //Remove all draws
            if(bentley != null)
                bentley.destroyAllLines();
            //Call the functions suscribed to the action called when the player dies
            if(box != null)
                box.PlayerKilled();

            collision.transform.position = GameObject.FindGameObjectWithTag("Spawn").transform.position;
            OnKill();
        }
    }
}
