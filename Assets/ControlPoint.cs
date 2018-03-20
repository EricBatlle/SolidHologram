using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPoint : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Move SpawnBox to ControlPoint
            updateSpawnBoxPosition();
        }
    }

    private void updateSpawnBoxPosition()
    {
        GameObject.FindGameObjectWithTag("Spawn").transform.position = this.transform.position;
    }
}
