using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.Impl;

public class PlayFabLeaderboard : MonoBehaviour
{
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

    public PlayFabObjectPoolManager opManager;
    public static List<GameObject> lbItems = new List<GameObject>();

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
    }

    public void OnButtonSwitchType()
    {
        switch (currStatType)
        {
            case StatisticType.OVERALL:
                currStatType = StatisticType.DAILY;
                statTypeButton.GetComponentInChildren<TMP_Text>().text = "Current : Daily";
                break;
            case StatisticType.DAILY:
                currStatType = StatisticType.WEEKLY;
                statTypeButton.GetComponentInChildren<TMP_Text>().text = "Current : Weekly";
                break;
            case StatisticType.WEEKLY:
                currStatType = StatisticType.MONTHLY;
                statTypeButton.GetComponentInChildren<TMP_Text>().text = "Current : Monthly";
                break;
            case StatisticType.MONTHLY:
                currStatType = StatisticType.OVERALL;
                statTypeButton.GetComponentInChildren<TMP_Text>().text = "Current : Overall";
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
                lbTypeButton.GetComponentInChildren<TMP_Text>().text = "Friend Leaderboard";
                break;
            case LeaderboardType.NEARBY:
                currLBType = LeaderboardType.FRIENDS;
                titleText.text = "FRIEND LEADERBOARD";
                lbTypeButton.GetComponentInChildren<TMP_Text>().text = "Global Leaderboard";
                break;
            case LeaderboardType.FRIENDS:
                currLBType = LeaderboardType.GLOBAL;
                titleText.text = "GLOBAL LEADERBOARD";
                lbTypeButton.GetComponentInChildren<TMP_Text>().text = "Nearby Leaderboard";
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
            ObjectPoolManager.ReturnObjectToPool(lbItems[i]);
        }

        ResetAllRows();

        for (int i = 0; i < r.Leaderboard.Count; i++)
        {
            UpdateRow(r.Leaderboard[i], r.Leaderboard[i].Position + 1);
        }
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
            ObjectPoolManager.ReturnObjectToPool(lbItems[i]);
        }

        ResetAllRows();

        for (int i = 0; i < r.Leaderboard.Count; i++)
        {
            UpdateRow(r.Leaderboard[i], r.Leaderboard[i].Position + 1);
        }
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
            ObjectPoolManager.ReturnObjectToPool(lbItems[i]);
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

        for (int i = 0; i < entryList.Count; i++)
        {
            UpdateRow(entryList[i], i + 1);
        }
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
            ObjectPoolManager.ReturnObjectToPool(lbItems[i]);
        }

        ResetAllRows();

        StopAllCoroutines();
    }

    private void UpdateRow(PlayerLeaderboardEntry item, int rank)
    {
        GameObject newRow = opManager.SpawnObject(rowPrefab);
        lbItems.Add(newRow);

        newRow.transform.SetParent(lbGroup.transform);
        newRow.transform.localScale = new Vector3(1, 1, 1);

        ResetRow(newRow);

        Image background = FindChildWithTag(newRow, "RowBG").GetComponent<Image>();
        TMP_Text rankText = FindChildWithTag(newRow, "RankText").GetComponent<TMP_Text>();
        TMP_Text nameText = FindChildWithTag(newRow, "DisplayNameText").GetComponent<TMP_Text>();
        TMP_Text scoreText = FindChildWithTag(newRow, "ScoreText").GetComponent<TMP_Text>();

        newRow.transform.SetSiblingIndex(item.Position);
        rankText.text = AddOrdinal(rank);
        nameText.text = item.DisplayName;
        scoreText.text = item.StatValue.ToString();

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

        lbGroup.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(1500, 0);

        lbItems.Clear();
    }

    private void ResetRow(GameObject row)
    {
        Image background = FindChildWithTag(row, "RowBG").GetComponent<Image>();
        TMP_Text rankText = FindChildWithTag(row, "RankText").GetComponent<TMP_Text>();
        TMP_Text nameText = FindChildWithTag(row, "DisplayNameText").GetComponent<TMP_Text>();
        TMP_Text scoreText = FindChildWithTag(row, "ScoreText").GetComponent<TMP_Text>();

        Color bgColor = background.color;
        Color32 rankColor = rankText.color;
        Color32 nameColor = nameText.color;
        Color32 scoreColor = scoreText.color;

        bgColor.a = 0;
        rankColor.a = 0;
        nameColor.a = 0;
        scoreColor.a = 0;

        background.color = bgColor;
        rankText.color = rankColor;
        nameText.color = nameColor;
        scoreText.color = scoreColor;

        rankText.text = "";
        nameText.text = "";
        scoreText.text = "";
    }

    private IEnumerator FadeInRow(Image background, TMP_Text rank, TMP_Text name, TMP_Text score, float delay)
    {
        yield return new WaitForSeconds(delay);

        Coroutine fadeInBG = StartCoroutine(FadeInBG(background));

        yield return new WaitForSeconds(0.4f);

        lbGroup.gameObject.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 65);
        Coroutine fadeInTexts = StartCoroutine(FadeInTexts(rank, name, score));

        yield return fadeInBG;
        yield return fadeInTexts;
    }

    private IEnumerator FadeInBG(Image background)
    {
        Color bgColor = background.color;

        float targetAlpha = 0.75f;

        while (Mathf.Abs(bgColor.a - targetAlpha) > 0)
        {
            bgColor.a = Mathf.Lerp(bgColor.a, targetAlpha, 10 * Time.deltaTime);
            
            background.color = bgColor;

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

    void OnLeaderboardError(PlayFabError e)
    {
        Debug.Log("Leaderboard Get Error : " + e.GenerateErrorReport());
    }
}
