using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] GameObject playerInventory;
    [SerializeField] GameObject cameraHolder;
    public string currSceneName;
    public string nextSceneName;
    public int floorNum = 1;
    public float timer = 0;
    public int seconds = 0;
    public bool timerActive = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else if (Instance != null)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "MenuScene")
        {
            playerInventory.SetActive(false);
            cameraHolder.SetActive(false);
        }
    }

    public void ChangeScene(string sceneName)
    {
        currSceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "LevelScene")
        {
            playerInventory.SetActive(true);       
            cameraHolder.SetActive(true);
        }
        else
        {
            playerInventory.SetActive(false);
            cameraHolder.SetActive(false);
        }

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
