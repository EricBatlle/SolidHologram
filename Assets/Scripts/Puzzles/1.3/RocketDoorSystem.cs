﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RocketDoorSystem : MonoBehaviour {
    [SerializeField] private MoveTowards rocketGate;
    [SerializeField] private nextLevel nextLevelObject = null;
    [SerializeField] private LeftSystem[] systems = null;
    private Action SystemEnabled;
    
    private bool allSystemsEnabled = true;

    private void OnEnable()
    {
        if (systems != null)
        {
            foreach (LeftSystem system in systems)
            {
                system.OnStateUpdated += CheckAllSystems;
            }
        }

        SystemEnabled += EnableNextLevel;
    }

    private void OnDisable()
    {
        if (systems != null)
        {
            foreach (LeftSystem system in systems)
            {
                system.OnStateUpdated -= CheckAllSystems;
            }
        }
        SystemEnabled -= EnableNextLevel;
    }

    private void CheckAllSystems()
    {
        allSystemsEnabled = true;
        foreach (LeftSystem system in systems)
        {
            if (system.isSystemActive == false)
                allSystemsEnabled = false;
        }

        if (allSystemsEnabled)
            SystemEnabled();
    }

    private void EnableNextLevel()
    {
        //Open rocket Door
        rocketGate.StartMove();
        //Active the trigger to nextLevel
        nextLevelObject.gameObject.SetActive(true);
    }
}