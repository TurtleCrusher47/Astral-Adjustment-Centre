using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorTrigger : MonoBehaviour
{
    public void LoadNextLevel()
    {
        // load next level
        Debug.Log("Next Level");
        GameManager.Instance.ChangeScene("LevelScene");
    }
}
