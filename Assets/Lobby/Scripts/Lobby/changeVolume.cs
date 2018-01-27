using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class changeVolume : MonoBehaviour {

	public Sprite musicOff;
    public Sprite musicOn;

    public Button button;


    void Start() {
        Button btn = button.GetComponent<Button>();
        btn.onClick.AddListener(changeImg);
    }

    public void changeImg()
    {
        if (button.image.sprite == musicOn)
        {
            button.image.sprite = musicOff;
            AudioListener.volume = 0;
        }
        else
        {
            button.image.sprite = musicOn;
            AudioListener.volume = 1;
        }
    }
}
