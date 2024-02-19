using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject subtitlePanel;
    public string nextSceneName;

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene("LoadingScene");
        // SceneManager.LoadScene(sceneName);

        nextSceneName = sceneName;

        Debug.Log("Next Scene : " + nextSceneName);

        StartCoroutine(DelayHidePanel());
    }

    private IEnumerator DelayHidePanel()
    {
        yield return null;

        Debug.Log("Test");

        subtitlePanel.SetActive(false);
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
