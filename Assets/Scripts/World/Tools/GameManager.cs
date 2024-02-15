using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public void ChangeScene(string sceneName)
    {
        // SceneManager.LoadScene("LoadingScene");
        SceneManager.LoadScene(sceneName);

        // nextSceneName = sceneName;
    }
}
