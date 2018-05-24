using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class PuzzleColorSystemV2 : MonoBehaviour
{

    [SerializeField] private PuzzleButton[] puzzleButtons;
    [SerializeField] private Sprite spriteSolution;
    [SerializeField] private Mover[] puzzleDoorMovers;
    [SerializeField] private Mover platform;

    public event Action OnPuzzleSolved;
    public event Action OnPuzzleNotSolved;

    private bool puzzleSolved = false;
    private bool puzzleHasBeenSolvedOnce = false;

    #region setPlayersReference
    private PlatformerCharacter2D box;
    private bool findBoxReference = false;
    #endregion

    //Used to set players reference
    private void LateUpdate()
    {
        //Set Players References       
        if (!findBoxReference)
        {
            GameObject boxGO = GameObject.FindGameObjectWithTag("Player");
            if (boxGO != null)
            {
                box = boxGO.GetComponent<PlatformerCharacter2D>();
                findBoxReference = true;
                box.OnPlayerDies += resetPuzzle;
            }
        }
    }

    private void OnEnable()
    {
        foreach (PuzzleButton puzzleButton in puzzleButtons)
        {
            puzzleButton.OnColorChange += checkSolution;
        }
        foreach (Mover mover in puzzleDoorMovers)
        {
            this.OnPuzzleSolved += mover.Open;
            this.OnPuzzleNotSolved += mover.Close;
        }        
    }

    private void OnDisable()
    {
        foreach (PuzzleButton puzzleButton in puzzleButtons)
        {
            puzzleButton.OnColorChange -= checkSolution;
        }
        foreach (Mover mover in puzzleDoorMovers)
        {
            this.OnPuzzleSolved -= mover.Open;
            this.OnPuzzleNotSolved -= mover.Close;
        }
        if (box != null)
            box.OnPlayerDies -= resetPuzzle;
    }

    private void resetPuzzle()
    {
        //Restart boolean variables
        puzzleSolved = false;
        puzzleHasBeenSolvedOnce = false;
        //Set buttons to original state
        foreach (PuzzleButton puzzleButton in puzzleButtons)
        {
            puzzleButton.resetButton();
        }
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
        else if (puzzleHasBeenSolvedOnce)
        {
            platform.Close();
            StartCoroutine(WaitPlatform());
        }
    }

    private IEnumerator WaitPlatform()
    {
        while (platform.moving)
        {
            yield return new WaitForEndOfFrame();
        }
        OnPuzzleNotSolved();
    }

}
