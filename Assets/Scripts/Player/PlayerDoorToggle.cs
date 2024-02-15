using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDoorToggle : MonoBehaviour
{
    [SerializeField] private LayerMask targetableLayer;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 1000, targetableLayer))
            {
                Debug.Log("Door Hit");
                hit.collider.GetComponent<DoorTrigger>().ToggleDoor();
            }
        }
    }
}
