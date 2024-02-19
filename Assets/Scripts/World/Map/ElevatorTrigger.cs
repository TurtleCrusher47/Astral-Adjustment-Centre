using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorTrigger : MonoBehaviour
{
    [SerializeField] private Generator3D mapGenerator;
    public void LoadNextLevel()
    {
        // load next level
        Debug.Log("Next Level");
        StartCoroutine(mapGenerator.LoadNextLevel());
    }
}
