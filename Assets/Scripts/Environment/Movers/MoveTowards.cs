using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowards : MonoBehaviour {

    // The target marker.
    [SerializeField] private GameObject objectToMove;
    // The target marker.
    [SerializeField] private Transform endTarget;
    // Speed in units per sec.
    [SerializeField] private float speed = 2.0f;

    [SerializeField] private bool isMoving = false;

    public void StartMove()
    {
        isMoving = true;
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        while (isMoving)
        {
            // The step size is equal to speed times frame time.
            float step = speed * Time.deltaTime;
            // Move our position a step closer to the target.
            objectToMove.transform.position = Vector3.MoveTowards(objectToMove.transform.position, endTarget.position, step);
            yield return new WaitForEndOfFrame();
        }        
    }
}
