using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuGlobeRotation : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 dragStartPosition;

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartDragging();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopDragging();
        }

        if (isDragging)
        {
            RotateSphere();
        }
    }

    void StartDragging()
    {
        isDragging = true;
        dragStartPosition = Input.mousePosition;
    }

    void StopDragging()
    {
        isDragging = false;
    }

    void RotateSphere()
    {
        Vector3 dragCurrentPosition = Input.mousePosition;
        float deltaX = dragCurrentPosition.x - dragStartPosition.x;
        float deltaY = dragCurrentPosition.y - dragStartPosition.y;

        float rotationSpeed = 0.1f;
        float rotationX = deltaY * rotationSpeed;
        float rotationY = -deltaX * rotationSpeed;

        transform.Rotate(Vector3.up, rotationY, Space.World);
        transform.Rotate(Vector3.right, rotationX, Space.World);

        dragStartPosition = dragCurrentPosition;
    }
}
