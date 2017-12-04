using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager_MultiPlayer : NetworkManager
{

    [SerializeField] Vector3 player1SpawnPos;
    [SerializeField] Vector3 player2SpawnPos;
    [SerializeField] GameObject character1;
    [SerializeField] GameObject character2;
    [SerializeField] int currentCharacter = 0;

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {

        //ensure we don't add more than two players
        if (currentCharacter > 1)
            return;

        //depending on which player we add, we set variables to those set by unity UI
        GameObject chosenCharacter = character1;
        Vector3 chosenSpawnPos = player1SpawnPos;
        if (currentCharacter == 1)
        {
            chosenCharacter = character2;
            chosenSpawnPos = player2SpawnPos;
        }
		chosenSpawnPos = GameObject.FindGameObjectWithTag("Spawn").transform.position;

        //create new object and add to server
        var player = (GameObject)GameObject.Instantiate(chosenCharacter, chosenSpawnPos, Quaternion.identity);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

        //move to next character
        currentCharacter++;
    }
}
