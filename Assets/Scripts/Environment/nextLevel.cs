using Prototype.NetworkLobby;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class nextLevel : NetworkInteractiveObject
{
    public string nextSceneName;
    public bool blabla;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            destroyAllLines();
            ////find any/all lines and destroy them            
            //GameObject[] toDestroy = GameObject.FindGameObjectsWithTag("line");
            //foreach (GameObject td in toDestroy)
            //{
            //    NetworkServer.Destroy(td);
            //}
            ChangeScene();            
        }
    }

    #region changeScene
    private void ChangeScene()
    {
        if (isServer)
        {
            RpcChangeScene();
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

    [ClientRpc]
    private void RpcChangeScene()
    {
        LobbyManager.s_Singleton.LoadScene(nextSceneName);
    }

    [Command]
    private void CmdChangeScene()
    {
        RpcChangeScene();
    }
    #endregion

    //Find any/all lines and destroy them :: same function that has Bentley
    #region destroyAllLines
    public void destroyAllLines()
    {
        //if (!isLocalPlayer)
        //    return;

        if (isServer)
        {
            RpcDestroyAllLines();
        }
        else
        {
            if (!hasAuthority)
            {
                this.OnInteraction += NmCmdDestroyAllLines;
                setLocalAuthority();
            }
            else
            {
                CmdDestroyAllLines();
            }            
        }
    }
    void NmCmdDestroyAllLines()
    {
        CmdDestroyAllLines();
    }
    [Command]
    void CmdDestroyAllLines()
    {
        RpcDestroyAllLines();
    }
    [ClientRpc]
    void RpcDestroyAllLines()
    {
        ////find any/all lines and destroy them
        GameObject[] toDestroy = GameObject.FindGameObjectsWithTag("line");
        foreach (GameObject td in toDestroy)
        {
            NetworkServer.Destroy(td);
        }
    }
    #endregion

}
