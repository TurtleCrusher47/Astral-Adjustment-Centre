using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This code is just to point the canvas towards the camera
public class Billboard : MonoBehaviour
{
    public static Transform MainCameraTransform; // Static variable to store the main camera reference

    private void Start()
    {
        if (MainCameraTransform == null)
        {
            // Find the main camera in the current scene and store the reference
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                MainCameraTransform = mainCamera.transform;
            }
        }
    }

    private void LateUpdate()
    {
        if (MainCameraTransform != null)
        {
            transform.LookAt(transform.position + MainCameraTransform.forward);
        }
    }
}
