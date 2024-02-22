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

            JSONManager.SendJSON(PlayFabManager.runsCompleted);

            SendLeaderboard("HighScore");
            SendLeaderboard("HighScoreDaily");
            SendLeaderboard("HighScoreWeekly");
            SendLeaderboard("HighScoreMonthly");

            GameManager.Instance.ChangeScene("MenuScene");
        }
        else
        {
            // load next level
            Debug.Log("Next Level");
            StartCoroutine(mapGenerator.LoadNextLevel());
        }
    }

    public void SendLeaderboard(string statName)
    {
        var req = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = statName,
                    Value = GameManager.Instance.seconds
                }
            }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(req, OnLeaderboardUpdate, OnError);
    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult r)
    {
        Debug.Log("Successful Leaderboard Sent : " + r.ToString());
    }

    void OnError(PlayFabError e)
    {
        Debug.Log("Error : " + e.GenerateErrorReport());
    }
}
