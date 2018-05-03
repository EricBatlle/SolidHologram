using System;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkInteractiveObject : NetworkBehaviour
{
    private GameObject player = null;
    public Action OnInteraction;

    //ToDo: Test if it's better to do that, or just find the player every time in setLocalAuthority
    private void FixedUpdate()
    {
        if(player == null)
        {
            player = findLocalPlayer();
        }
    }

    public void setLocalAuthority()
    {
        //find player object and assign authority
        //GameObject player = GameObject.FindGameObjectWithTag("LocalPlayer");
        NetworkIdentity playerID = player.GetComponent<NetworkIdentity>();
        player.GetComponent<PlayerAuthorityAssignator>().CmdSetAuth(netId, playerID);
    }

    public override void OnStartAuthority()
    {
        if (!isServer)
        {
            if (OnInteraction != null)
            {
                OnInteraction();
                OnInteraction = null;
            }
        }
    }

    public GameObject findLocalPlayer()
    {
        GameObject box = GameObject.FindWithTag("Player");
        GameObject bentley = GameObject.FindWithTag("Bentley");

        if (box != null)
        {
            if (box.GetComponent<PlayerAuthorityAssignator>().isLocal)
            {
                return box;
            }
        }
        if (bentley != null)
        {
            if (bentley.GetComponent<PlayerAuthorityAssignator>().isLocal)
            {
                return bentley;
            }
        } 
        //Debug log
        //print("There is no local player?");

        return null;
    }
}
