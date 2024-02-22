using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using PlayFab;
using UnityEngine.Pool;


public class MainSceneManager : MonoBehaviour
{
    [SerializeField] private PlayFabSocial playfabSocial;
    [SerializeField] private PlayFabLeaderboard playfabLeaderboard;
    [SerializeField] private PlayFabGuild playfabGuild;
    [SerializeField] private PlayFabMyGuild playfabMyGuild;


    [SerializeField] private List<MonoBehaviour> playfabScripts;
    [SerializeField] private GameObject loginPage, regPage, forgetpassPage;

    public TMP_Text statusText;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.None;

        if (PlayFabManager.currPlayFabID == null || PlayFabManager.currPlayFabID == "")
        {
            regPage.SetActive(false);
            forgetpassPage.SetActive(false);
            loginPage.SetActive(true);
        }

        statusText.text = "";

        playfabScripts.Add(playfabSocial);
        playfabScripts.Add(playfabLeaderboard);
        playfabScripts.Add(playfabGuild);
        playfabScripts.Add(playfabMyGuild);
    }

    void Start()
    {
        AudioManager.Instance.StartCoroutine(AudioManager.Instance.PlayBGM("BGMDemonSky"));
    }

    public void ShowLoginPage()
    {
        regPage.SetActive(false);
        forgetpassPage.SetActive(false);
        loginPage.SetActive(true);
    }

    public void SetStatusText(string text)
    {
        statusText.text = text;

        //StartCoroutine(ResetStatusText());
    }

    private IEnumerator ResetStatusText()
    {
        yield return new WaitForSeconds(10);

        statusText.text = "";
    }

    public static void ClearAllLeaderboard()
    {
        foreach (var item in PlayFabLeaderboard.lbItems)
        {
            ObjectPoolManager.Instance.StartCoroutine(ObjectPoolManager.Instance.ReturnObjectToPool(item));
        }

        PlayFabLeaderboard.lbItems.Clear();
    }

    public static void ClearAllStoreItems()
    {
        PlayFabStore.itemsInStore.Clear();
    }

    public static void ClearAllInventoryItems()
    {
        PlayFabUser.itemsInInv.Clear();
    }

    public static void ClearFriendListItems()
    {
        foreach (var item in PlayFabSocial.friendItems)
        {
            ObjectPoolManager.Instance.StartCoroutine(ObjectPoolManager.Instance.ReturnObjectToPool(item));
        }

        PlayFabSocial.friendItems.Clear();
    }

    public static void ClearFriendRequestListItems()
    {
        foreach (var item in PlayFabSocial.requestItems)
        {
            ObjectPoolManager.Instance.StartCoroutine(ObjectPoolManager.Instance.ReturnObjectToPool(item));
        }

        PlayFabSocial.requestItems.Clear();
    }

    public static void ClearFriendPendingListItems()
    {
        foreach (var item in PlayFabSocial.pendingItems)
        {
            ObjectPoolManager.Instance.StartCoroutine(ObjectPoolManager.Instance.ReturnObjectToPool(item));
        }

        PlayFabSocial.pendingItems.Clear();
    }

    public static void ClearFriendTradeListItems()
    {
        PlayFabSocial.tradeItems.Clear();
    }

    public static void ClearGuildsListItems()
    {
        foreach (var item in PlayFabGuild.guildItems)
        {
            ObjectPoolManager.Instance.StartCoroutine(ObjectPoolManager.Instance.ReturnObjectToPool(item));
        }

        PlayFabGuild.guildItems.Clear();
    }

    public static void ClearGuildRequestsListItems()
    {
        foreach (var item in PlayFabGuild.requestItems)
        {
            ObjectPoolManager.Instance.StartCoroutine(ObjectPoolManager.Instance.ReturnObjectToPool(item));
        }

        PlayFabGuild.requestItems.Clear();
    }

    public static void ClearMembersListItems()
    {
        foreach (var item in PlayFabMyGuild.memberItems)
        {
            ObjectPoolManager.Instance.StartCoroutine(ObjectPoolManager.Instance.ReturnObjectToPool(item));
        }

        PlayFabMyGuild.memberItems.Clear();
    }

    public static void ClearMemberRequestsListItems()
    {
        foreach (var item in PlayFabMyGuild.requestItems)
        {
            ObjectPoolManager.Instance.StartCoroutine(ObjectPoolManager.Instance.ReturnObjectToPool(item));
        }

        PlayFabMyGuild.requestItems.Clear();
    }

    public void OnButtonPlay()
    {
        ClearAllLeaderboard();
        ClearAllStoreItems();
        ClearAllInventoryItems();
        ClearFriendListItems();
        ClearFriendRequestListItems();
        ClearFriendPendingListItems();
        ClearGuildsListItems();
        ClearGuildRequestsListItems();
        ClearMembersListItems();
        ClearMemberRequestsListItems();

        SceneManager.LoadScene("Menu");
    }

    public void OnButtonLogout()
    {
        PlayFabManager.currPlayFabID = "";
        PlayFabManager.currPlayFabDN = "";
        PlayFabManager.currTitleID = "";
        PlayFabManager.currGuildID = null;
        PlayFabManager.currGuildName = "";
        PlayFabManager.currGuildRole = "";

        StopAllCoroutines();
        StopAllCoroutinesInScripts();
        
        StartCoroutine(ClearAllLists());

        PlayFabClientAPI.ForgetAllCredentials();
    }

    public void StopAllCoroutinesInScripts()
    {
        foreach (MonoBehaviour script in playfabScripts)
        {
            script.StopAllCoroutines();
        }
    }

    private IEnumerator ClearAllLists()
    {
        Coroutine leaderboard = StartCoroutine(ClearAllItems(PlayFabLeaderboard.lbItems));

        Coroutine friend = StartCoroutine(ClearAllItems(PlayFabSocial.friendItems));
        Coroutine friendRequest = StartCoroutine(ClearAllItems(PlayFabSocial.requestItems));
        Coroutine friendPending = StartCoroutine(ClearAllItems(PlayFabSocial.pendingItems));
        
        Coroutine guild = StartCoroutine(ClearAllItems(PlayFabGuild.guildItems));
        Coroutine guildRequest = StartCoroutine(ClearAllItems(PlayFabGuild.requestItems));
        
        Coroutine member = StartCoroutine(ClearAllItems(PlayFabMyGuild.memberItems));
        Coroutine memberRequest = StartCoroutine(ClearAllItems(PlayFabMyGuild.requestItems));

        yield return leaderboard;
        
        yield return friend;
        yield return friendRequest;
        yield return friendPending;
        
        yield return guild;
        yield return guildRequest;
        
        yield return member;
        yield return memberRequest;

        ObjectPoolManager.Instance.ObjectPools.Clear();
    }

    private IEnumerator ClearAllItems(List<GameObject> list)
    {
        Coroutine clearListItems = StartCoroutine(ClearListItems(list));

        yield return clearListItems;

        foreach (var item in list)
        {
            Destroy(item);
        }

        list.Clear();

        yield return null;
    }

    private IEnumerator ClearListItems(List<GameObject> list)
    {
        foreach (var item in list)
        {
            StartCoroutine(ObjectPoolManager.Instance.ReturnObjectToPool(item));
        }

        yield return null;
    }

    // public void SetDisplayName(string name)
    // {
    //     menuNameText.text = name;
    // }
}
