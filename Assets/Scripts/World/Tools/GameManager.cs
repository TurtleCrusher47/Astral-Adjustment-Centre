using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public string nextSceneName;
    public int floorNum = 1;

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene("LoadingScene");
        // SceneManager.LoadScene(sceneName);

        nextSceneName = sceneName;

        Debug.Log("Next Scene : " + nextSceneName);
    }

    public GameObject FindChildWithTag(GameObject parent, string tag)
    {
        GameObject child = null;
        
        foreach(Transform transform in parent.transform)
        {
            if(transform.CompareTag(tag))
            {
                child = transform.gameObject;
                break;
            }
        }
 
        return child;
    }
}
