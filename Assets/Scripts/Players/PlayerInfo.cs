using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class PlayerInfo : NetworkBehaviour {

    [Header("IT'S NOT WORKING AT ALL!")]
    [SerializeField] public int playerType = 0; //0-> Client ; 1-> Server

    public bool isCreatedByServer()
    {        
        //return ( isServer && (playerType == 1)) ? true : false;
        return (playerType == 1) ? true : false;
    }

    public bool isCreatedByClient()
    {
        //return (!isServer && (playerType == 0)) ? true : false;
        return (playerType == 0) ? true : false;

    }

    public bool isOriginal()
    {
        return (isCreatedByClient() || isCreatedByServer());
    }

}
