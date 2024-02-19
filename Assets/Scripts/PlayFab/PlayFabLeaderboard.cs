using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.Impl;
using Michsky.UI.Shift;
using System.Linq;

public class PlayFabLeaderboard : MonoBehaviour
{
    [System.Serializable]
    public class GetRunsCompletedResults
    {
        public List<string> RunsCompleted = new();
    }

    public enum StatisticType
    {
        OVERALL,
        DAILY,
        WEEKLY,
        MONTHLY
    }

    public enum LeaderboardType
    {
        GLOBAL,
        NEARBY,
        FRIENDS
    }

    public static List<GameObject> lbItems = new List<GameObject>();
    public List<string> playersRunCompleted;

    [SerializeField] public GameObject rowPrefab;
    [SerializeField] public GridLayoutGroup lbGroup;
    [SerializeField] public TMP_Text titleText;
    [SerializeField] public Button statTypeButton;
    [SerializeField] public Button lbTypeButton;

    private StatisticType currStatType;
    private LeaderboardType currLBType;
    private string currStatistic = "";
    private float delay = 0;

    void Awake()
    {
        currStatType = StatisticType.OVERALL;
        currLBType = LeaderboardType.GLOBAL;
    }

    public void OnButtonSwitchType()
    {
        switch (currStatType)
        {
            case StatisticType.OVERALL:
                currStatType = StatisticType.DAILY;
                statTypeButton.GetComponent<MainButton>().ChangeText("CURRENT : DAILY");
                break;
            case StatisticType.DAILY:
                currStatType = StatisticType.WEEKLY;
                statTypeButton.GetComponent<MainButton>().ChangeText("CURRENT : WEEKLY");
                break;
            case StatisticType.WEEKLY:
                currStatType = StatisticType.MONTHLY;
                statTypeButton.GetComponent<MainButton>().ChangeText("CURRENT : MONTHLY");
                break;
            case StatisticType.MONTHLY:
                currStatType = StatisticType.OVERALL;
                statTypeButton.GetComponent<MainButton>().ChangeText("CURRENT : OVERALL");
                break;
        }

        OnButtonRefreshLB();
    }

    public void OnButtonSwitchLB()
    {
        switch (currLBType)
        {
            case LeaderboardType.GLOBAL:
                currLBType = LeaderboardType.NEARBY;
                titleText.text = "NEARBY LEADERBOARD";
                lbTypeButton.GetComponent<MainButton>().ChangeText("FRIEND LEADERBOARD");
                break;
            case LeaderboardType.NEARBY:
                currLBType = LeaderboardType.FRIENDS;
                titleText.text = "FRIEND LEADERBOARD";
                lbTypeButton.GetComponent<MainButton>().ChangeText("GLOBAL LEADERBOARD");
                break;
            case LeaderboardType.FRIENDS:
                currLBType = LeaderboardType.GLOBAL;
                titleText.text = "GLOBAL LEADERBOARD";
                lbTypeButton.GetComponent<MainButton>().ChangeText("NEARBY LEADERBOARD");
                break;
        }

        OnButtonRefreshLB();
    }

    private void GetCurrStatisticName()
    {
        switch (currStatType)
        {
            case StatisticType.OVERALL:
                currStatistic = "HighScore";
                break;
            case StatisticType.DAILY:
                currStatistic = "HighScoreDaily";
                break;
            case StatisticType.WEEKLY:
                currStatistic = "HighScoreWeekly";
                break;
            case StatisticType.MONTHLY:
                currStatistic = "HighScoreMonthly";
                break;
        }
    }

    private void GetLeaderboardIcon(List<PlayerLeaderboardEntry> entries)
    {
        List<string> playfabIDs = new List<string>();

        for (int i = 0; i < entries.Count; i++)
        {
            playfabIDs.Add(entries[i].PlayFabId);
        }

        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
        {
            FunctionName = "GetPlayerDatas",
            FunctionParameter = new
            {
                PlayFabIDs = JSONManager.StringListToJSON("IDs", playfabIDs)
            }
        }, csResult=>
        {
            var jsonString = csResult.FunctionResult.ToString();
            var runsCompleted = JsonUtility.FromJson<GetRunsCompletedResults>(jsonString);

            playersRunCompleted = runsCompleted.RunsCompleted.ToList();

            for (int i = 0; i < entries.Count; i++)
            {
                UpdateRow(entries[i], i + 1);
            }
        }, OnLeaderboardError);
    }

    public void OnButtonGetLeaderboard()
    {
        currLBType = LeaderboardType.GLOBAL;

        GetCurrStatisticName();

        var lbReq = new GetLeaderboardRequest
        {
            StatisticName = currStatistic,
            StartPosition = 0,
            MaxResultsCount = 100
        };

        PlayFabClientAPI.GetLeaderboard(lbReq, OnLeaderboardGet, OnLeaderboardError);
    }

    void OnLeaderboardGet(GetLeaderboardResult r)
    {
        StopAllCoroutines();

        for (int i = 0; i < lbItems.Count; i++)
        {
            StartCoroutine(ObjectPoolManager.Instance.ReturnObjectToPool(lbItems[i]));
        }

        ResetAllRows();

        List<PlayerLeaderboardEntry> entries = r.Leaderboard;
        entries.Reverse();

        GetLeaderboardIcon(entries);
    }

    public void OnButtonGetNearbyLeaderboard()
    {
        currLBType = LeaderboardType.NEARBY;

        GetCurrStatisticName();

        var lbReq = new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = currStatistic,
            PlayFabId = PlayFabManager.currPlayFabID,
            MaxResultsCount = 5
        };

        PlayFabClientAPI.GetLeaderboardAroundPlayer(lbReq, OnNearbyLeaderboardGet, OnLeaderboardError);
    }

    public void OnNearbyLeaderboardGet(GetLeaderboardAroundPlayerResult r)
    {
        StopAllCoroutines();
        
        for (int i = 0; i < lbItems.Count; i++)
        {
            StartCoroutine(ObjectPoolManager.Instance.ReturnObjectToPool(lbItems[i]));
        }

        ResetAllRows();

        GetLeaderboardIcon(r.Leaderboard);
    }

    public void OnButtonGetFriendLeaderboard()
    {
        currLBType = LeaderboardType.FRIENDS;

        GetCurrStatisticName();

        var lbReq = new GetFriendLeaderboardRequest
        {
            StatisticName = currStatistic,
            StartPosition = 0,
            MaxResultsCount = 100
        };

        PlayFabClientAPI.GetFriendLeaderboard(lbReq, lbResult=> {
            var flReq = new GetFriendsListRequest
            {
                
            };

            PlayFabClientAPI.GetFriendsList(flReq, flResult=> {
                OnFriendLeaderboardGet(lbResult, flResult);
            }, OnLeaderboardError);
        }, OnLeaderboardError);
    }

    public void OnFriendLeaderboardGet(GetLeaderboardResult lb, GetFriendsListResult fl)
    {
        List<PlayerLeaderboardEntry> entryList = new List<PlayerLeaderboardEntry>();
        StopAllCoroutines();
        
        for (int i = 0; i < lbItems.Count; i++)
        {
            StartCoroutine(ObjectPoolManager.Instance.ReturnObjectToPool(lbItems[i]));
        }

        ResetAllRows();

        for (int i = 0; i < lb.Leaderboard.Count; i++)
        {
            if (lb.Leaderboard[i].PlayFabId == PlayFabManager.currPlayFabID)
            {
                entryList.Add(lb.Leaderboard[i]);
            }

            for (int j = 0; j < fl.Friends.Count; j++)
            {
                if (fl.Friends[j].Tags.Contains("Friends") && 
                    lb.Leaderboard[i].PlayFabId == fl.Friends[j].FriendPlayFabId)
                {
                    entryList.Add(lb.Leaderboard[i]);
                }
            }
        }

        GetLeaderboardIcon(entryList);
    }

    public void OnButtonRefreshLB()
    {
        switch (currLBType)
        {
            case LeaderboardType.GLOBAL:
                OnButtonGetLeaderboard();
                break;
            case LeaderboardType.NEARBY:
                OnButtonGetNearbyLeaderboard();
                break;
            case LeaderboardType.FRIENDS:
                OnButtonGetFriendLeaderboard();
                break;
        }
    }

    public void ClearLeaderboard()
    {
        for (int i = 0; i < lbItems.Count; i++)
        {
            PlayFabObjectPoolManager.ReturnObjectToPool(lbItems[i]);
        }

        ResetAllRows();

        StopAllCoroutines();
    }

    private void UpdateRow(PlayerLeaderboardEntry item, int rank)
    {
        GameObject newRow = ObjectPoolManager.Instance.SpawnObject(rowPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        lbItems.Add(newRow);

        newRow.transform.SetParent(lbGroup.transform);
        newRow.transform.localPosition = new Vector3(0, 0, 0);
        newRow.transform.localRotation = Quaternion.Euler(0, 0, 0);
        newRow.transform.localScale = new Vector3(1, 1, 1);

        ResetRow(newRow);

        CanvasGroup background = FindChildWithTag(newRow, "RowBG").GetComponent<CanvasGroup>();
        TMP_Text rankText = FindChildWithTag(newRow, "RankText").GetComponent<TMP_Text>();
        TMP_Text nameText = FindChildWithTag(newRow, "DisplayNameText").GetComponent<TMP_Text>();
        TMP_Text scoreText = FindChildWithTag(newRow, "ScoreText").GetComponent<TMP_Text>();

        int runs = int.Parse(playersRunCompleted[rank - 1]);
        string rankIcon = "";

        if (runs >= 40)
        {
            rankIcon = "<sprite name=\"M8\"> ";
        }
        else if (runs >= 35)
        {
            rankIcon = "<sprite name=\"M7\"> ";
        }
        else if (runs >= 30)
        {
            rankIcon = "<sprite name=\"M6\"> ";
        }
        else if (runs >= 25)
        {
            rankIcon = "<sprite name=\"M5\"> ";
        }
        else if (runs >= 20)
        {
            rankIcon = "<sprite name=\"M4\"> ";
        }
        else if (runs >= 15)
        {
            rankIcon = "<sprite name=\"M3\"> ";
        }
        else if (runs >= 10)
        {
            rankIcon = "<sprite name=\"M2\"> ";
        }
        else if (runs >= 5)
        {
            rankIcon = "<sprite name=\"M1\"> ";
        }

        newRow.transform.SetSiblingIndex(rank);
        rankText.text = AddOrdinal(rank);
        nameText.text = rankIcon + item.DisplayName;
        scoreText.text = ConvertSecondsToHHMMSS(item.StatValue);

        delay += 0.25f;
        StartCoroutine(FadeInRow(background, rankText, nameText, scoreText, delay));
    }

    private void ResetAllRows()
    {
        delay = 0;

        foreach (var item in lbItems)
        {
            ResetRow(item);
        }

        lbGroup.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(1200, 0);

        lbItems.Clear();
    }

    private void ResetRow(GameObject row)
    {
        CanvasGroup background = FindChildWithTag(row, "RowBG").GetComponent<CanvasGroup>();
        TMP_Text rankText = FindChildWithTag(row, "RankText").GetComponent<TMP_Text>();
        TMP_Text nameText = FindChildWithTag(row, "DisplayNameText").GetComponent<TMP_Text>();
        TMP_Text scoreText = FindChildWithTag(row, "ScoreText").GetComponent<TMP_Text>();

        background.alpha = 0;
        Color32 rankColor = rankText.color;
        Color32 nameColor = nameText.color;
        Color32 scoreColor = scoreText.color;

        rankColor.a = 0;
        nameColor.a = 0;
        scoreColor.a = 0;

        rankText.color = rankColor;
        nameText.color = nameColor;
        scoreText.color = scoreColor;

        rankText.text = "";
        nameText.text = "";
        scoreText.text = "";
    }

    private IEnumerator FadeInRow(CanvasGroup background, TMP_Text rank, TMP_Text name, TMP_Text score, float delay)
    {
        yield return new WaitForSeconds(delay);

        Coroutine fadeInBG = StartCoroutine(FadeInBG(background));

        yield return new WaitForSeconds(0.4f);

        lbGroup.gameObject.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 65);
        Coroutine fadeInTexts = StartCoroutine(FadeInTexts(rank, name, score));

        yield return fadeInBG;
        yield return fadeInTexts;
    }

    private IEnumerator FadeInBG(CanvasGroup background)
    {
        float targetAlpha = 1f;

        while (Mathf.Abs(background.alpha - targetAlpha) > 0)
        {
            background.alpha = Mathf.Lerp(background.alpha, targetAlpha, 10 * Time.deltaTime);

            yield return null;
        }
    }

    private IEnumerator FadeInTexts(TMP_Text rank, TMP_Text name, TMP_Text score)
    {
        Color textColor = score.color;

        float targetAlpha = 1f;

        while (Mathf.Abs(textColor.a - targetAlpha) > 0)
        {
            textColor.a = Mathf.Lerp(textColor.a, targetAlpha, 5 * Time.deltaTime);
            
            rank.color = textColor;
            name.color = textColor;
            score.color = textColor;

            yield return null;
        }
    }

    private GameObject FindChildWithTag(GameObject parent, string tag)
    {
        GameObject child = null;
        
        foreach(Transform transform in parent.transform)
        {
            if(transform.CompareTag(tag))
            {
                child = transform.gameObject;
                break;
            }
        }
 
        return child;
    }

    private string AddOrdinal(int num)
    {
        if (num <= 0) 
        {
            return num.ToString();
        }

        switch (num % 100)
        {
            case 11:
            case 12:
            case 13:
                return num + "th";
        }
        
        switch (num % 10)
        {
            case 1:
                return num + "st";
            case 2:
                return num + "nd";
            case 3:
                return num + "rd";
            default:
                return num + "th";
        }
    }

    private string ConvertSecondsToHHMMSS(int totalSeconds)
    {
        int hours = totalSeconds / 3600;
        int minutes = (totalSeconds % 3600) / 60;
        int seconds = totalSeconds % 60;

        return string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }

    void OnLeaderboardError(PlayFabError e)
    {
        Debug.Log("Leaderboard Get Error : " + e.GenerateErrorReport());
    }
}
