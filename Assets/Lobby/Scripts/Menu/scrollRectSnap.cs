using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scrollRectSnap : MonoBehaviour {

    public RectTransform panel; //hold scrollpanel
    public Button[] button;
    public RectTransform center; //center to compare the distance for each button

    private float[] distance; //all buttons distance to the center
    private bool dragging = false; //will be true while we drag the panel
    private int buttonDistance; //will hold the distance between the buttons
    private int minButtonNum; //to hold the number of the button, with smallest distance to center

    void Start()
    {
        int buttonLeght = button.Length;
        distance = new float[buttonLeght];

        //get distance between buttons
        buttonDistance = (int)Mathf.Abs(button[1].GetComponent<RectTransform>().anchoredPosition.x - button[0].GetComponent<RectTransform>().anchoredPosition.x);

    }

    void Update()
    {
        for (int i = 0; i < button.Length; i++)
        {
            distance[i] = Mathf.Abs(center.transform.position.x - button[i].transform.position.x);
        }

        float minDistance = Mathf.Min(distance); //get the min distance

        for (int a = 0; a < button.Length; a++)
        {
            if (minDistance == distance[a])
            {
                minButtonNum = a;
            }
        }

        if (!dragging)
        {
            LerpToBttn(minButtonNum * -buttonDistance);
        }
    }

    void LerpToBttn(int position)
    {
        float newX = Mathf.Lerp(panel.anchoredPosition.x, position, Time.deltaTime * 10f);
        Vector2 newPosition = new Vector2(newX, panel.anchoredPosition.y);

        panel.anchoredPosition = newPosition;
    }

    public void startDrag()
    {
        dragging = true;
    }

    public void endDrag()
    {
        dragging = false;
    }
}
