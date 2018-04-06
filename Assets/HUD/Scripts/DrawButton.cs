using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawButton : Button {

    private DrawButton[] hudButtons;

    protected override void Start()
    {
        base.Start();
        hudButtons = this.GetComponentInParent<LineDraw_Net>().hudButtons;
    }

    //Can't change the pressed img at the begining with the setPressedImg function cause hudButtons can be at this time null
    public void initiatePressed()
    {
        //Set pressed Img
        this.GetComponentInParent<Image>().sprite = this.spriteState.pressedSprite;
    }

    //Set pressed img button and disables all the pressed drawbuttons
    public void setPressedImg()
    {
        //Disable all buttons in case someone was pressed
        foreach(DrawButton drawButton in hudButtons)
        {
            drawButton.setDisableImg();
        }

        //Set pressed Img
        this.GetComponentInParent<Image>().sprite = this.spriteState.pressedSprite;
    }

    //Set disable img button
    public void setDisableImg()
    {
        //Set disable Img
        this.GetComponentInParent<Image>().sprite = this.spriteState.disabledSprite;
    }
}
