using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCinemachine : MonoBehaviour
{
    public CinemachineVirtualCamera cm;
    public Transform playerTransform;

    void Update()
    {
        if (playerTransform != null)
        {
            //transform.position = playerTransform.position + new Vector3(horizontalDisplacement, verticalDisplacement, depth);
            cm.Follow = playerTransform;
        }
    }

    public void setTarget(Transform target)
    {
        playerTransform = target;
    }
}
