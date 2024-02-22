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

    [SerializeField] private Color baseColor, altColor;

    //private float realProgress = 0.0f;
    private float fakeProgress = 0f;

    public void Awake()
    {
        // Made a Temp Button to test it, can change to automatically switch scene
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return null;

        GameManager.Instance.timerActive = false;
        bool switchColor = false;
        loadingBarText.color = baseColor;

        // Begin to load the Scene you specify
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(GameManager.Instance.nextSceneName);
        // Don't let the Scene activate until you allow it to
        asyncOperation.allowSceneActivation = false;

        // When the load is still in progress, output the Text and progress bar
        while (!asyncOperation.isDone)
        {   
            // Simulate a delay for loading time
            yield return new WaitForSeconds(0.01f); // Adjust the time as needed

            // Continue incrementing fake progress even after Unity's progress reaches 0.9
            fakeProgress = Mathf.Clamp01(fakeProgress + 0.01f); // Adjust the increment as needed

            // Output the current progress
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            progress = Mathf.Lerp(progress, fakeProgress, 1f); // Interpolate between Unity's progress and fake progress

            if (progress > 0.45f && !switchColor)
            {
                switchColor = true;
                loadingBarText.color = altColor;
            }

            loadingBarText.text = (progress * 100f).ToString("F0") + "%";
            loadingBar.value = progress;

            loadingText.text = "Loading ";

            int numDots = (int)(Time.time % 3) + 1;
            for (int i = 0; i < numDots; i++)
            {
                loadingText.text += ".";
            }

            // Check if the load has finished
            if (progress >= 1.0f)
            {
                // Change the Text to show the Scene is ready
                // loadingText.text = "Continue";
                // Wait until you press the space key to activate the Scene
                asyncOperation.allowSceneActivation = true;

                if (GameManager.Instance.nextSceneName == "LevelScene")
                {
                    if (GameManager.Instance.floorNum <= 1)
                    {
                        TimelineManager.Instance.StartCoroutine(TimelineManager.Instance.PlayCutscene("PostIntro", null));
                        GameManager.Instance.timer = 0;
                        GameManager.Instance.seconds = 0;
                    }
                    
                    GameManager.Instance.timerActive = true;
                }
            }

            yield return null;
        }
    }
}
