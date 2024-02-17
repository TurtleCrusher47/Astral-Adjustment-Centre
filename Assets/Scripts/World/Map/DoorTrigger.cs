using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private GameObject door;

    public void ToggleDoor()
    {
        
    }

    public void ToggleDoor(bool toggle)
    {
        door.SetActive(toggle);
    }

    private void Update()
    {

    }
}
