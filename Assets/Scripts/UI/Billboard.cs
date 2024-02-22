using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This code is just to point the canvas towards the camera
public class Billboard : MonoBehaviour
{
    [SerializeField]
    private Transform cam;

    private void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
