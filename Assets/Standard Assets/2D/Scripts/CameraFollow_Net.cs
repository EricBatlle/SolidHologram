using System;
using UnityEngine;

public class CameraFollow_Net : MonoBehaviour
{
    public Transform playerTransform;
    public int depth = -20;
    public int horizontalDisplacement = 0;
    public int verticalDisplacement = 0;

    // Update is called once per frame
    void Update()
    {
        if (playerTransform != null)
        {
            transform.position = playerTransform.position + new Vector3(horizontalDisplacement, verticalDisplacement, depth);
        }
    }

    public void setTarget(Transform target)
    {
        playerTransform = target;
    }
}
