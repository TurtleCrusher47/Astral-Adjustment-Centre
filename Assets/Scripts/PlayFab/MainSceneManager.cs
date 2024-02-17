using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using PlayFab;

public class MainSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject loginPage, regPage, forgetpassPage;

    public TMP_Text statusText;

    void Awake()
    {
        AudioManager.Instance.PlayBGM("BGMDemonSky");

        if (PlayFabManager.currPlayFabID == null || PlayFabManager.currPlayFabID == "")
        {
            regPage.SetActive(false);
            forgetpassPage.SetActive(false);
            loginPage.SetActive(true);
        }
        else
        {
            // LoadScene to Menu
        }

        statusText.text = "";
        // SetDisplayName(PlayFabManager.currPlayFabDN);
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
        PlayFabSocial.friendItems.Clear();
    }

    public static void ClearFriendRequestListItems()
    {
        PlayFabSocial.requestItems.Clear();
    }

    public static void ClearFriendPendingListItems()
    {
        PlayFabSocial.pendingItems.Clear();
    }

    public static void ClearFriendTradeListItems()
    {
        PlayFabSocial.tradeItems.Clear();
    }

    public static void ClearGuildsListItems()
    {
        PlayFabGuild.guildItems.Clear();
    }

    public static void ClearGuildRequestsListItems()
    {
        PlayFabGuild.requestItems.Clear();
    }

    public static void ClearMembersListItems()
    {
        PlayFabMyGuild.memberItems.Clear();
    }

    public static void ClearMemberRequestsListItems()
    {
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

        ClearAllLeaderboard();
        //ClearAllStoreItems();
        //ClearAllInventoryItems();
        ClearFriendListItems();
        ClearFriendRequestListItems();
        ClearFriendPendingListItems();
        ClearGuildsListItems();
        ClearGuildRequestsListItems();
        ClearMembersListItems();
        ClearMemberRequestsListItems();
        
        ObjectPoolManager.Instance.ObjectPools.Clear();

        StopAllCoroutines();

        PlayFabClientAPI.ForgetAllCredentials();

        SceneManager.LoadScene("MenuScene");
    }

    // public void SetDisplayName(string name)
    // {
    //     menuNameText.text = name;
    // }
}
