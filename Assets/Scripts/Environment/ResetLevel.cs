using Prototype.NetworkLobby;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class ResetLevel : NetworkInteractiveObject
{
    private string nextSceneName;
    private void OnDisable()
    {
        OnInteraction -= ResetScene;
    }

    private void Start()
    {                
        nextSceneName = SceneManager.GetActiveScene().name;
    }

    #region resetScene
    public void ResetScene()
    {
        if (isServer)
        {
            RpcResetScene();
        }
        else
        {
            if (!hasAuthority)
            {
                this.OnInteraction += NmCmdResetScene;
                setLocalAuthority();
            }
            else
            {
                CmdResetScene();
            }
        }        
    }

    public void NmCmdResetScene()
    {
        CmdResetScene();
    }

    [ClientRpc]
    protected void RpcResetScene()
    {
        LobbyManager.s_Singleton.LoadScene(nextSceneName);
    }

    [Command]
    protected void CmdResetScene()
    {
        RpcResetScene();
    }
    #endregion    
}
