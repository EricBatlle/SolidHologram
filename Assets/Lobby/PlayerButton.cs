using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerButton : Button {
    //[SerializeField] public Sprite startSprite;
    //[SerializeField] public Sprite selectedSprite;
    [SerializeField] public Sprite currSprite;
    [SerializeField] public Sprite pressedSprite;

    [SerializeField] public bool pressed = false;

    private void OnGUI()
    {
        this.GetComponentInParent<Image>().sprite = this.currSprite;
    }
}
