using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public string currSceneName;
    public string nextSceneName;
    public int floorNum = 1;
    public float timer = 0;
    public int seconds = 0;
    public bool timerActive = false;

    public void ChangeScene(string sceneName)
    {
        currSceneName = SceneManager.GetActiveScene().name;

        SceneManager.LoadScene("LoadingScene");

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

    void Update()
    {
        if (timerActive)
        {
            timer += Time.deltaTime;
            seconds = (int)(timer % 60);
        }
    }
}
