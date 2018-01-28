using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nextLevel : MonoBehaviour {

    public Prototype.NetworkLobby.LobbyManager lobbyManager;
    public string nextSceneName;    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            lobbyManager.ServerChangeScene(nextSceneName);
            collision.transform.position = GameObject.FindGameObjectWithTag("Spawn").transform.position;

        }
    }
}
