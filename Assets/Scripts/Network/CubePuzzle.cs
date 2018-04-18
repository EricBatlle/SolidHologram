using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubePuzzle : MonoBehaviour {

    [SerializeField] private MaterialChanger[] cubes;
    [SerializeField] private Material materialSolution;
    [SerializeField] private Text text;

    private bool puzzleSolved = false;
    public Action OnPuzzleSolved;

    private void OnEnable()
    {
        OnPuzzleSolved += changeText;

        foreach (MaterialChanger cube in cubes)
        {
            cube.OnMaterialChange += checkSolution;
        }
    }

    private void OnDisable()
    {
        OnPuzzleSolved -= changeText;
        foreach (MaterialChanger cube in cubes)
        {
            cube.OnMaterialChange -= checkSolution;
        }
    }

    private void checkSolution()
    {
        puzzleSolved = true;

        foreach (MaterialChanger cube in cubes)
        {
            if (cube.currMaterial != materialSolution)
            {
                puzzleSolved = false;
            }
        }

        if(puzzleSolved == true)
        {
            OnPuzzleSolved();
        }
    }

    private void changeText()
    {
        text.text = "Solved";
    }

}
