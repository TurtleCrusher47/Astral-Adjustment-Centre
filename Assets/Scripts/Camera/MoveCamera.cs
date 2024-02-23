using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveCamera : MonoBehaviour
{
    private Transform cameraPosition;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void SetCameraPosition()
    {
        if (SceneManager.GetActiveScene().name == "LevelScene")
        {
            cameraPosition = GameObject.FindGameObjectWithTag("CameraPosition").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraPosition != null)
        {
            transform.position = cameraPosition.position;
        }
    }
}
