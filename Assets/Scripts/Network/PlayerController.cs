﻿//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Networking;

//public class PlayerController : NetworkBehaviour
//{

//    private void Start()
//    {
//        if (isLocalPlayer)
//            this.gameObject.tag = "LocalPlayer";
//    }

//    [Command]
//    public void CmdSetAuth(NetworkInstanceId objectId, NetworkIdentity player)
//    {
//        GameObject iObject = NetworkServer.FindLocalObject(objectId);
//        NetworkIdentity networkIdentity = iObject.GetComponent<NetworkIdentity>();

//        //Checks if anyone else has authority and removes it and lastly gives the authority to the player who interacts with object
//        NetworkConnection otherOwner = networkIdentity.clientAuthorityOwner;
//        if (otherOwner == player.connectionToClient)
//        {
//            return;
//        }
//        else
//        {
//            if (otherOwner != null)
//            {
//                networkIdentity.RemoveClientAuthority(otherOwner);
//            }
//            networkIdentity.AssignClientAuthority(player.connectionToClient);
//        }

//        networkIdentity.AssignClientAuthority(player.connectionToClient);
//    }

//}