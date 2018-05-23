using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftSystem : MonoBehaviour {
    [SerializeField] private PuzzleButton leftPanelButton = null;
    [SerializeField] private PuzzleButton leftPanelIndicator = null;
    [SerializeField] private Mover[] gates = null;
    [SerializeField] private Mover floatingPlatform = null;

    private void OnEnable()
    {
        foreach (Mover gate in gates)
        {
            gate.OnStop += CheckPlatformMovement;
            
        }
        leftPanelButton.OnColorChange += leftPanelIndicator.nextColor;
    }
    private void OnDisable()
    {
        foreach (Mover gate in gates)
        {
            gate.OnStop -= CheckPlatformMovement;
        }
        leftPanelButton.OnColorChange -= leftPanelIndicator.nextColor;
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
   
    private void EnablePanel()
    {
        //change Panel sprite
    }
    private void EnableIndicator()
    {
        //change indicator sprite
    }
}
