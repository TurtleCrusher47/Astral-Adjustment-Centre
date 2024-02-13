using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    [SerializeField] Transform cameraRotation;

    // Update is called once per frame
    void Update()
    {
        transform.rotation = cameraRotation.rotation;
    }
}
