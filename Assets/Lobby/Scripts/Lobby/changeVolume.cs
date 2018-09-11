using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class changeVolume : MonoBehaviour {

	public Sprite musicOff;
    public Sprite musicOff_highlighted;

    public Sprite musicOn;
    public Sprite musicOn_highlighted;

    public Button button;
    private Button m_btn;


    void Start() {
        Button m_btn = button.GetComponent<Button>();
        m_btn.onClick.AddListener(changeImg);
    }

    public void changeImg()
    {
        SpriteState st = new SpriteState();        
        if (button.image.sprite == musicOn)
        {
            button.image.sprite = musicOff;
            st.highlightedSprite = musicOff_highlighted;            
            AudioListener.volume = 0;
        }
        else
        {
            button.image.sprite = musicOn;
            st.highlightedSprite = musicOn_highlighted;
            AudioListener.volume = 1;
        }
        button.spriteState = st;
    }
}
