using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VCManager : MonoBehaviour {

    [SerializeField] private Cinemachine.CinemachineVirtualCamera vc;
    [SerializeField] private bool affectBox = false;
    [SerializeField] private bool affectBentley = true;

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

    private void OnStart()
    {
        if (affectBox)
        {
            if (affectBentley)
            {
                //Change bouth cameras
                vc.Priority = 15;
            }
            else
            {
                //Change only Box camera
                GameObject box = GameObject.FindGameObjectWithTag("Player");
                if (box != null)
                {
                    PlayerAuthorityAssignator boxAuthority = box.GetComponent<PlayerAuthorityAssignator>();
                    if (boxAuthority.isLocal)
                        vc.Priority = 15;
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
                        vc.Priority = 15;
                }                
            }
        }
    }
    private void OnEnd()
    {
        vc.Priority = 0;
    }
}
