using Prototype.NetworkLobby;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class nextLevel : NetworkInteractiveObject
{
    [SerializeField] private bool startDisabled = false;
    private bool disabledOnce = false;
    public string nextSceneName;    

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

    private void FixedUpdate()
    {
        //if (startDisabled && (player != null) && (!disabledOnce))
        if (startDisabled && (!disabledOnce))
        {
            disabledOnce = true;
            this.gameObject.SetActive(false);
        }
    }

    #region changeScene
    protected void ChangeScene()
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
    protected void RpcChangeScene()
    {
        PlayerInfoController.s_Singleton.UpdateProgress();
        LobbyManager.s_Singleton.LoadScene(nextSceneName);
    }

    [Command]
    protected void CmdChangeScene()
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
