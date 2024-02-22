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
        if (GameManager.Instance.floorNum == 4)
        {
            // game end
            GameManager.Instance.timerActive = false;
            PlayFabManager.runsCompleted += 1;

            JSONManager.Instance.SendJSON(PlayFabManager.runsCompleted);

            JSONManager.Instance.SendLeaderboard("HighScore");
            JSONManager.Instance.SendLeaderboard("HighScoreDaily");
            JSONManager.Instance.SendLeaderboard("HighScoreWeekly");
            JSONManager.Instance.SendLeaderboard("HighScoreMonthly");

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
