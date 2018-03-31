using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleColorSystem : MonoBehaviour {

    [SerializeField] private PuzzleButton[] puzzleButtons;
    [SerializeField] private Sprite spriteSolution;
    [SerializeField] private Mover puzzleDoorMover;

    public event Action OnPuzzleSolved;
    public event Action OnPuzzleNotSolved;

    private bool puzzleSolved = false;
    private bool puzzleHasBeenSolvedOnce = false;

    private void OnEnable()
    {
        foreach (PuzzleButton puzzleButton in puzzleButtons)
        {
            puzzleButton.OnColorChange += checkSolution;
        }
        this.OnPuzzleSolved += puzzleDoorMover.Open;
        this.OnPuzzleNotSolved += puzzleDoorMover.Close;
    }

    private void OnDisable()
    {
        foreach (PuzzleButton puzzleButton in puzzleButtons)
        {
            puzzleButton.OnColorChange -= checkSolution;
        }
        this.OnPuzzleSolved -= puzzleDoorMover.Open;
        this.OnPuzzleNotSolved -= puzzleDoorMover.Close;
    }

    private void checkSolution()
    {
        puzzleSolved = true;
        foreach (PuzzleButton puzzleButton in puzzleButtons)
        {
            if (puzzleButton.currSprite != spriteSolution)
            {
                puzzleSolved = false;
            }
        }

        if (puzzleSolved == true)
        {
            puzzleHasBeenSolvedOnce = true;
            OnPuzzleSolved();            
        }
        else if(puzzleHasBeenSolvedOnce)
        {
            OnPuzzleNotSolved();
        }
    }
    
}
