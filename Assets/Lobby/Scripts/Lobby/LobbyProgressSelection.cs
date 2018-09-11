using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyProgressSelection : MonoBehaviour {

    [SerializeField] private Button[] buttons;
    [SerializeField] private ProgressType progressType;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private Sprite lockImage;

    private enum ProgressType
    {
        Chapter,
        Level
    }   

    public void SetValidProgress()
    {
        SetButtonsInitialValues();
        int maxProgress = CheckPlayerProgress();
        for(int i = maxProgress; i < buttons.Length; i++)
        {
            buttons[i].image.sprite = lockImage;
            buttons[i].interactable = false;
        }
    }

    private void SetButtonsInitialValues()
    {
        for(int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = true;
            buttons[i].image.sprite = sprites[i];
        }
    }

    private int CheckPlayerProgress()
    {        
        switch (progressType)
        {
            case ProgressType.Chapter:
                return PlayerInfoController.s_Singleton.GetLastChapterCompleted();
                
            case ProgressType.Level:
                return PlayerInfoController.s_Singleton.GetLastLevelCompleted();
                
            default:
                print("error CheckPlayerProgress Type");
                break;
        }
        return 0;
    }
}
