using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    public TMP_Text loadingBarText;
    public TMP_Text loadingText;
    public Slider loadingBar;

    public void Awake()
    {
        // Made a Temp Button to test it, can change to automatically switch scene
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return null;

        //Begin to load the Scene you specify
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(GameManager.Instance.nextSceneName);
        //Don't let the Scene activate until you allow it to
        asyncOperation.allowSceneActivation = false;

        Debug.Log("Progress : " + asyncOperation.progress);

        //When the load is still in progress, output the Text and progress bar
        while (!asyncOperation.isDone)
        {
            //Output the current progress
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);

            loadingBarText.text = Mathf.Round(progress * 100) + "%";

            loadingBar.value = progress * 100;

            //Debug.Log(progress * 100 + "%");

            loadingText.text = "Loading ";

            int numDots =(int)(Time.time % 3) + 1;
            for(int i = 0; i < numDots; i++)
            {
                loadingText.text += ".";
            }

            // Check if the load has finished
            if (asyncOperation.progress >= 0.9f)
            {
                //Change the Text to show the Scene is ready
                //loadingText.text = "Continue";
                //Wait to you press the space key to activate the Scene
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
