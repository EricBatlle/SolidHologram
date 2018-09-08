using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftSystem : MonoBehaviour {
    [SerializeField] private PuzzleButton leftPanelButton = null;
    [SerializeField] private PuzzleButton leftPanelIndicator = null;
    [SerializeField] private Mover[] gates = null;
    [SerializeField] private Mover floatingPlatform = null;

    public bool isSystemActive = false;

    public Action OnStateUpdated; 

    private void OnEnable()
    {
        if(gates != null)
        {
            foreach (Mover gate in gates)
            {
                gate.OnStop += CheckPlatformMovement;

            }
        }            
        leftPanelButton.OnColorChange += leftPanelIndicator.nextColor;
        leftPanelButton.OnColorChange += ActivateSystemState;
    }
    private void OnDisable()
    {
        foreach (Mover gate in gates)
        {
            gate.OnStop -= CheckPlatformMovement;
        }
        leftPanelButton.OnColorChange -= leftPanelIndicator.nextColor;
        leftPanelButton.OnColorChange -= ActivateSystemState;
    }

    private void ActivateSystemState()
    {
        isSystemActive = true;
        OnStateUpdated();
    }

    private void CheckPlatformMovement()
    {
        bool allStoped = true;
        bool allOpen = true;

        foreach (Mover gate in gates)
        {
            if (gate.moving)
            {
                allStoped = false;
            }
            if (gate.close)
            {
                allOpen = false;
            }
        }

        if (allStoped && allOpen)
        {
            floatingPlatform.Move();
        }
    }   
   
}
