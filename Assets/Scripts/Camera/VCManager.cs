using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VCManager : MonoBehaviour {

    [SerializeField] private Cinemachine.CinemachineVirtualCamera vc;
    [SerializeField] private CustomTrigger start;
    [SerializeField] private CustomTrigger end;


    private void OnEnable()
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
        vc.Priority = 15;
    }
    private void OnEnd()
    {
        vc.Priority = 0;
    }
}
