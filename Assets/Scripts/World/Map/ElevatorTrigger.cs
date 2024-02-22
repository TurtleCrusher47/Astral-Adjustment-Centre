using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class ElevatorTrigger : MonoBehaviour
{
    [SerializeField] private Generator3D mapGenerator;
    public void LoadNextLevel()
    {
        if (GameManager.Instance.floorNum == 1)
        {
            // game end
            GameManager.Instance.timerActive = false;
            PlayFabManager.runsCompleted += 1;

            GameManager.Instance.GetComponent<JSONManager>().SendJSON(PlayFabManager.runsCompleted);

            GameManager.Instance.GetComponent<JSONManager>().SendLeaderboard("HighScore");
            GameManager.Instance.GetComponent<JSONManager>().SendLeaderboard("HighScoreDaily");
            GameManager.Instance.GetComponent<JSONManager>().SendLeaderboard("HighScoreWeekly");
            GameManager.Instance.GetComponent<JSONManager>().SendLeaderboard("HighScoreMonthly");

            GameManager.Instance.ChangeScene("MenuScene");
        }
        else
        {
            // load next level
            Debug.Log("Next Level");
            StartCoroutine(mapGenerator.LoadNextLevel());
        }
    }
}
