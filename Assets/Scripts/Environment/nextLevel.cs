using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class nextLevel : NetworkBehaviour {

    public Prototype.NetworkLobby.LobbyManager lobbyManager;
    public string nextSceneName;    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            RpcChangeScene();            
        }
    }
    
    [ClientRpc]
    public void RpcChangeScene()
    {
        lobbyManager.ServerChangeScene(nextSceneName);
    }
}
