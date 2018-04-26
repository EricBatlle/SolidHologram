using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VCManager : MonoBehaviour {

    [SerializeField] protected Cinemachine.CinemachineVirtualCamera vc;
    [SerializeField] protected bool affectBox = false;
    [SerializeField] protected bool affectBentley = true;

    [SerializeField] private CustomTrigger start;
    [SerializeField] private CustomTrigger end;

    private void Start()
    {
        start.OnEnter += OnStart;
        end.OnEnter += OnEnd;
    }
    private void OnDisable()
    {
        start.OnEnter -= OnStart;
        end.OnEnter -= OnEnd;
    }

    public virtual void OnStart()
    {
        if (affectBox)
        {
            if (affectBentley)
            {
                //Change bouth cameras
                CameraStartAction();
            }
            else
            {
                //Change only Box camera
                GameObject box = GameObject.FindGameObjectWithTag("Player");
                if (box != null)
                {
                    PlayerAuthorityAssignator boxAuthority = box.GetComponent<PlayerAuthorityAssignator>();
                    if (boxAuthority.isLocal)
                        CameraStartAction();
                }
            }
        }
        else
        {
            if (affectBentley)
            {
                //Change only Bentley camera
                GameObject bentley = GameObject.FindGameObjectWithTag("Bentley");
                if(bentley != null)
                {
                    PlayerAuthorityAssignator bentleyAuthority = bentley.GetComponent<PlayerAuthorityAssignator>();
                    if (bentleyAuthority.isLocal)
                        CameraStartAction();
                }                
            }
        }
    }
    public virtual void OnEnd()
    {
        CameraEndAction();
    }

    public virtual void CameraStartAction()
    {
        vc.Priority = 15;
    }
    public virtual void CameraEndAction()
    {
        vc.Priority = 0;
    }
}
