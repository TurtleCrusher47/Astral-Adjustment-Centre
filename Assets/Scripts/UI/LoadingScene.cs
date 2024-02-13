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

    public void LoadSceneButton()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return null;

        //Begin to load the Scene you specify
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("MenuScene");
        //Don't let the Scene activate until you allow it to
        asyncOperation.allowSceneActivation = false;
        Debug.Log("Pro :" + asyncOperation.progress);
        //When the load is still in progress, output the Text and progress bar
        while (!asyncOperation.isDone)
        {
            //Output the current progress
            float progress = Mathf.Clamp(asyncOperation.progress / 0.9f);

            loadingBar.value = progress;

            loadingBarText.text = Mathf.Round(progress * 100) + "%";

            loadingText.text = "Loading ";

            int dotsCount = Mathf.FloorToInt(Time.time % 4);
            for(int i = 0; i < dotsCount; i++)
            {
                loadingText.text += ".";
            }

            // Check if the load has finished
            if (asyncOperation.progress >= 0.9f)
            {
                //Change the Text to show the Scene is ready
                //loadingText.text = "Press the space bar to continue";
                //Wait to you press the space key to activate the Scene
                if (Input.GetKeyDown(KeyCode.Space))
                    //Activate the Scene
                    asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
