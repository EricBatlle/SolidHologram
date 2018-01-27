using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour {

	//public GameObject loadingScreen;
	//public Slider slider;
	//public Text progressText;

    [SerializeField]
    private Image Bar;

    private float fillAmount = 1;

    [SerializeField]
    private float units;


    void Update()
    {
        updateBar();
    }

    void Start()
    {
        StartCoroutine(BuildUnits());
    }

    private void updateBar()
    {
        Bar.fillAmount = fillAmount;
    }

    public IEnumerator BuildUnits()
    {
        for (int i = 0; i <= units; i++)
        {
            fillAmount = i / units;
            yield return null;
        }

        //Done loading 
        //var operation = SceneManager.LoadSceneAsync(1);
    }

    //public void loadLevel(int sceneIndex){
    //StartCoroutine (loadAsynchronously (sceneIndex));
    //}

    //IEnumerator loadAsynchronously (int sceneIndex){
    //	AsyncOperation opertation = SceneManager.LoadSceneAsync (sceneIndex);

    //	loadingScreen.SetActive (true);

    //	while (!opertation.isDone) {
    //		float progress = Mathf.Clamp01 (opertation.progress / .9f);

    //		slider.value = progress;
    //		progressText.text = progress * 100f + "%";
    //		yield return null;
    //	}
    //}
}
