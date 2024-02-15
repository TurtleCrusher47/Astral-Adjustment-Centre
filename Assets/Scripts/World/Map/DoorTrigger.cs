using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    private bool isOpen = false;
    private float targetAngle = 0;

    public void ToggleDoor()
    {
        isOpen = !isOpen;
        if (!isOpen)
        {
            targetAngle = 0;
        }
        else if (isOpen)
        {
            targetAngle = 90;
        }
    }

    private void Update()
    {
        if (transform.localEulerAngles.y <= targetAngle - 1.5f || transform.localEulerAngles.y >= targetAngle + 1.5f)
        {
            GetComponent<MeshCollider>().enabled = false;
            transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, new Vector3(0, targetAngle, 0), 0.15f);
        }
        else
        {
            GetComponent<MeshCollider>().enabled = true;
            transform.localEulerAngles = new Vector3(0, targetAngle, 0);
        }
    }
}
