using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PuzzleButton : NetworkBehaviour, IInteractable {
    
    [SerializeField] private Material[] materials;
    private int materialsCount = 0;

    public Material currMaterial;
    public event Action OnColorChange;
   

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
        if (materialsCount >= materials.Length)
        {
            materialsCount = 0;
        }
        this.GetComponent<MeshRenderer>().material = materials[materialsCount];
        currMaterial = materials[materialsCount];
        materialsCount++;

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
