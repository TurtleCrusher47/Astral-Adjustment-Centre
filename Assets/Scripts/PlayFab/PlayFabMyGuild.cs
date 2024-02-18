using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.GroupsModels;
using Michsky.UI.Shift;

public class PlayFabMyGuild : MonoBehaviour
{
    [System.Serializable]
    public class GetDisplayNameResults
    {
        public List<string> DisplayNames = new();
    }

    public enum ListType
    {
        MEMBERSLIST,
        REQUESTPENDINGLIST
    }

    public PlayFabGuild playFabGuild;

    public static List<GameObject> memberItems = new List<GameObject>();
    public static List<GameObject> requestItems = new List<GameObject>();
    public static List<GameObject> pendingItems = new List<GameObject>();

    [SerializeField] public GameObject membersRowPrefab;
    [SerializeField] public GameObject requestRowPrefab;
    [SerializeField] public GameObject pendingRowPrefab;

    [SerializeField] public GridLayoutGroup membersListGroup;
    [SerializeField] public GridLayoutGroup requestAndPendingListGroup;

    [SerializeField] public TMP_Text titleText;
    [SerializeField] public TMP_Text detail2Text;
    [SerializeField] public Button inviteButton;
    [SerializeField] public Button listTypeButton;
    [SerializeField] public Button leaveButton;

    [SerializeField] public TMP_Text invitePlayerText;
    [SerializeField] public TMP_InputField if_displayName;

    public List<string> memberEntityIDs;
    public List<string> memberIDs;
    public List<string> memberDNs;
    public List<string> memberRoles;
    public List<string> memberAppExpiry;

    private ListType currListType;
    private float delay = 0;

    void Awake()
    {
        currListType = ListType.MEMBERSLIST;
    }
    
    public void OnButtonSwitchType()
    {
        switch (currListType)
        {
            case ListType.MEMBERSLIST:
                currListType = ListType.REQUESTPENDINGLIST;
                titleText.text = "REQUESTS & PENDING";

                membersListGroup.gameObject.SetActive(false);

                detail2Text.text = "Application Expiry";
                listTypeButton.GetComponent<MainButton>().ChangeText("MEMBERS");

                OnButtonRequestsList();
                break;
            case ListType.REQUESTPENDINGLIST:
                currListType = ListType.MEMBERSLIST;
                titleText.text = PlayFabManager.currGuildName;

                requestAndPendingListGroup.gameObject.SetActive(false);

                detail2Text.text = "Rank";
                listTypeButton.GetComponent<MainButton>().ChangeText("REQUEST / PENDING");

                OnButtonMembersList();
                break;
        }
    }

    public void OnEnterPanel()
    {
        titleText.text = PlayFabManager.currGuildName;

        OnButtonMembersList();

        if (PlayFabManager.currGuildRole != "Administrators")
        {
            inviteButton.interactable = false;
        }
    }

    public void OnButtonMembersList()
    {
        PlayFabGroupsAPI.ListGroupMembers(new ListGroupMembersRequest
        {
            Group = PlayFabManager.currGuildID
        }, mlResult =>
        {
            ResetAllDatas();
            StopAllCoroutines();

            foreach (var role in mlResult.Members)
            {
                foreach (var member in role.Members)
                {
                    if (role.RoleId == "admins")
                    {
                        memberRoles.Add("Admin");
                    }
                    else if (role.RoleId == "members")
                    {
                        memberRoles.Add("Member");
                    }

                    memberEntityIDs.Add(member.Key.Id);
                    memberIDs.Add(member.Lineage["master_player_account"].Id);
                }
            }

            GetAndSetDisplayNames("members");
        }, OnError);
    }

    public void OnButtonRequestsList()
    {
        PlayFabGroupsAPI.ListGroupApplications(new ListGroupApplicationsRequest
        {
            Group = PlayFabManager.currGuildID
        }, alResult=>
        {
            ResetAllDatas();
            StopAllCoroutines();

            foreach (var application in alResult.Applications)
            {
                memberEntityIDs.Add(application.Entity.Key.Id);
                memberIDs.Add(application.Entity.Lineage["master_player_account"].Id);
                memberAppExpiry.Add(application.Expires.ToString());
            }

            GetAndSetDisplayNames("requests");
        }, OnError);
    }

    private void GetAndSetDisplayNames(string type)
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
        {
            FunctionName = "GetDisplayNames",
            FunctionParameter = new
            {
                PlayFabIDs = JSONManager.StringListToJSON("IDs", memberIDs)
            }
        }, csResult=>
        {
            var jsonString = csResult.FunctionResult.ToString();
            var displayNames = JsonUtility.FromJson<GetDisplayNameResults>(jsonString);

            memberDNs = displayNames.DisplayNames.ToList();

            if (type == "members")
            {
                OnMemberListGet();
                listTypeButton.interactable = true;
            }
            else
            {
                OnRequestListGet();
                listTypeButton.interactable = true;
            }
        }, OnError);
    }

    public void OnInvitePlayer()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest
        {
            TitleDisplayName = if_displayName.text
        }, aiResult=>
        {
            var titlePlayerAccount = aiResult.AccountInfo.TitleInfo.TitlePlayerAccount;

            PlayFabGroupsAPI.InviteToGroup(new InviteToGroupRequest
            {
                Group = PlayFabManager.currGuildID,
                Entity = new PlayFab.GroupsModels.EntityKey
                {
                    Id = titlePlayerAccount.Id,
                    Type = titlePlayerAccount.Type
                }
            }, inviteResult=>
            {
                invitePlayerText.text = "Invited : " + aiResult.AccountInfo.TitleInfo.DisplayName;
                if_displayName.text = string.Empty;
            }, OnError);
        }, error=>
        {
            invitePlayerText.text = "Invalid Player";
        });
    }

    public void OnLeaveGuild()
    {
        if (memberIDs.Count >= 2)
        {
            if (PlayFabManager.currGuildRole == "Administrators")
            {
                List<string> membersToSelect = new List<string>();
                int index = 0;

                for (int i = 0; i < memberIDs.Count; i++)
                {
                    if (memberRoles[i] == "Member")
                    {
                        membersToSelect.Add(memberEntityIDs[i]);
                        index = i;
                    }
                }

                int rand = Random.Range(0, membersToSelect.Count - 1);

                PlayFabGroupsAPI.ChangeMemberRole(new ChangeMemberRoleRequest
                {
                    Group = PlayFabManager.currGuildID,
                    Members = new List<PlayFab.GroupsModels.EntityKey>
                    {
                        new PlayFab.GroupsModels.EntityKey
                        {
                            Id = membersToSelect[rand],
                            Type = "title_player_account"
                        }
                    },
                    OriginRoleId = "members",
                    DestinationRoleId = "admins"
                }, crResult=>
                {
                    PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
                    {
                        FunctionName = "EditGuildData",
                        FunctionParameter = new
                        {
                            guildID = PlayFabManager.currGuildID,
                            guildName = PlayFabManager.currGuildName,
                            guildLeaderID = memberIDs[index]
                        }
                    }, csResult=>
                    {
                        PlayFabGroupsAPI.RemoveMembers(new RemoveMembersRequest
                        {
                            Group = PlayFabManager.currGuildID,
                            Members = new List<PlayFab.GroupsModels.EntityKey>
                            {
                                new PlayFab.GroupsModels.EntityKey
                                {
                                    Id = PlayFabManager.currTitleID,
                                    Type = "title_player_account"
                                }
                            }
                        }, rmResult=>
                        {
                            Debug.Log("Left Guild : " + PlayFabManager.currGuildName);

                            PlayFabManager.currGuildID = null;
                            PlayFabManager.currGuildName = "";
                            PlayFabManager.currGuildRole = "";

                            playFabGuild.currListType = PlayFabGuild.ListType.REQUESTSLIST;
                            playFabGuild.OnButtonSwitchType();
                        }, OnError);
                    }, OnError);
                }, OnError);
            }
            else
            {
                PlayFabGroupsAPI.RemoveMembers(new RemoveMembersRequest
                {
                    Group = PlayFabManager.currGuildID,
                    Members = new List<PlayFab.GroupsModels.EntityKey>
                    {
                        new PlayFab.GroupsModels.EntityKey
                        {
                            Id = PlayFabManager.currTitleID,
                            Type = "title_player_account"
                        }
                    }
                }, rmResult=>
                {
                    Debug.Log("Left Guild : " + PlayFabManager.currGuildName);

                    PlayFabManager.currGuildID = null;
                    PlayFabManager.currGuildName = "";
                    PlayFabManager.currGuildRole = "";

                    playFabGuild.currListType = PlayFabGuild.ListType.REQUESTSLIST;
                    playFabGuild.OnButtonSwitchType();
                }, OnError);
            }
        }
        else
        {
            PlayFabGroupsAPI.DeleteGroup(new DeleteGroupRequest
            {
                Group = PlayFabManager.currGuildID
            }, dgResult=>
            {
                PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
                {
                    FunctionName = "DeleteGuildFromData",
                    FunctionParameter = new
                    {
                        guildID = PlayFabManager.currGuildID.Id
                    }
                }, ddResult=>
                {
                    Debug.Log("Left Guild : " + PlayFabManager.currGuildName);

                    PlayFabManager.currGuildID = null;
                    PlayFabManager.currGuildName = "";
                    PlayFabManager.currGuildRole = "";

                    playFabGuild.currListType = PlayFabGuild.ListType.REQUESTSLIST;
                    playFabGuild.OnButtonSwitchType();
                }, OnError);
            }, OnError);
        }
    }

    public void OnKickMember(GameObject btn)
    {
        Debug.Log("Button : Kick pressed.");
        string memberDN = FindChildWithTag(btn.transform.parent.gameObject, "SocialDetail1Text").GetComponent<TMP_Text>().text;
        string memberTitleID = "";

        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest
        {
            TitleDisplayName = memberDN
        }, aiResult=>
        {
            memberTitleID = aiResult.AccountInfo.TitleInfo.TitlePlayerAccount.Id;

            PlayFabGroupsAPI.RemoveMembers(new RemoveMembersRequest
            {
                Group = PlayFabManager.currGuildID,
                Members = new List<PlayFab.GroupsModels.EntityKey>
                {
                    new PlayFab.GroupsModels.EntityKey
                    {
                        Id = memberTitleID,
                        Type = "title_player_account"
                    }
                }
            }, arResult=>
            {
                Debug.Log("Kicked : " + memberDN);
                OnButtonMembersList();
            }, OnError);
        }, OnError);
    }

    public void OnAcceptJoinRequest(GameObject btn)
    {
        Debug.Log("Button : Accept Join Request pressed.");
        string userDN = FindChildWithTag(btn.transform.parent.gameObject, "SocialDetail1Text").GetComponent<TMP_Text>().text;
        string userTitleID = "";

        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest
        {
            TitleDisplayName = userDN
        }, aiResult=>
        {
            userTitleID = aiResult.AccountInfo.TitleInfo.TitlePlayerAccount.Id;

            PlayFabGroupsAPI.AcceptGroupApplication(new AcceptGroupApplicationRequest
            {
                Group = PlayFabManager.currGuildID,
                Entity = new()
                {
                    Id = userTitleID,
                    Type = "title_player_account"
                }
            }, arResult=>
            {
                Debug.Log("Accepted Join Request : " + userDN);
                OnButtonRequestsList();
            }, OnError);
        }, OnError);
    }

    public void OnDenyJoinRequest(GameObject btn)
    {
        Debug.Log("Button : Deny Join Request pressed.");
        string userDN = FindChildWithTag(btn.transform.parent.gameObject, "SocialDetail1Text").GetComponent<TMP_Text>().text;
        string userTitleID = "";

        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest
        {
            TitleDisplayName = userDN
        }, aiResult=>
        {
            userTitleID = aiResult.AccountInfo.PlayFabId;

            PlayFabGroupsAPI.RemoveGroupApplication(new RemoveGroupApplicationRequest
            {
                Group = PlayFabManager.currGuildID,
                Entity = new()
                {
                    Id = userTitleID,
                    Type = "title_player_account"
                }
            }, arResult=>
            {
                Debug.Log("Denied Join Request : " + userDN);
                OnButtonRequestsList();
            }, OnError);
        }, OnError);
    }

    void OnMemberListGet()
    {
        StopAllCoroutines();

        membersListGroup.gameObject.SetActive(true);

        for (int i = 0; i < memberItems.Count; i++)
        {
            StartCoroutine(ObjectPoolManager.Instance.ReturnObjectToPool(memberItems[i]));
        }

        ResetAllRows(membersListGroup, memberItems, 0);

        for (int i = 0; i < memberIDs.Count; i++)
        {
            UpdateMembersListRow(i);
        }
    }

    void OnRequestListGet()
    {
        StopAllCoroutines();
        
        requestAndPendingListGroup.gameObject.SetActive(true);

        for (int i = 0; i < requestItems.Count; i++)
        {
            StartCoroutine(ObjectPoolManager.Instance.ReturnObjectToPool(requestItems[i]));
        }

        ResetAllRows(requestAndPendingListGroup, requestItems, 1);

        for (int i = 0; i < memberIDs.Count; i++)
        {
            UpdateRequestsListRow(i);
        }
    }

    private void UpdateMembersListRow(int i)
    {
        GameObject newRow = ObjectPoolManager.Instance.SpawnObject(membersRowPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        memberItems.Add(newRow);

        newRow.transform.SetParent(membersListGroup.transform);
        newRow.transform.localPosition = new Vector3(0, 0, 0);
        newRow.transform.localRotation = Quaternion.Euler(0, 0, 0);
        newRow.transform.localScale = new Vector3(1, 1, 1);
        newRow.transform.SetSiblingIndex(i);

        ResetListRow(newRow, 0);

        CanvasGroup background = FindChildWithTag(newRow, "RowBG").GetComponent<CanvasGroup>();
        TMP_Text detail1Text = FindChildWithTag(newRow, "SocialDetail1Text").GetComponent<TMP_Text>();
        TMP_Text detail2Text = FindChildWithTag(newRow, "SocialDetail2Text").GetComponent<TMP_Text>();
        Button kickBtn = FindChildWithTag(newRow, "Kick").GetComponent<Button>();

        detail1Text.text = memberDNs[i];
        detail2Text.text = memberRoles[i];

        kickBtn.onClick.AddListener(delegate { OnKickMember(kickBtn.gameObject); });

        if (PlayFabManager.currGuildRole != "Administrators" || memberIDs[i] == PlayFabManager.currPlayFabID)
        {
            delay += 0.25f;
            StartCoroutine(FadeInRow(membersListGroup, background, detail1Text, detail2Text, null, null, delay));
        }
        else
        {
            delay += 0.25f;
            StartCoroutine(FadeInRow(membersListGroup, background, detail1Text, detail2Text, kickBtn, null, delay));
        }
    }

    private void UpdateRequestsListRow(int i)
    {
        GameObject newRow = ObjectPoolManager.Instance.SpawnObject(requestRowPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        requestItems.Add(newRow);

        newRow.transform.SetParent(requestAndPendingListGroup.transform);
        newRow.transform.localPosition = new Vector3(0, 0, 0);
        newRow.transform.localRotation = Quaternion.Euler(0, 0, 0);
        newRow.transform.localScale = new Vector3(1, 1, 1);
        newRow.transform.SetSiblingIndex(i);

        ResetListRow(newRow, 1);

        CanvasGroup background = FindChildWithTag(newRow, "RowBG").GetComponent<CanvasGroup>();
        TMP_Text detail1Text = FindChildWithTag(newRow, "SocialDetail1Text").GetComponent<TMP_Text>();
        TMP_Text detail2Text = FindChildWithTag(newRow, "SocialDetail2Text").GetComponent<TMP_Text>();
        Button acceptBtn = FindChildWithTag(newRow, "AcceptRequest").GetComponent<Button>();
        Button denyBtn = FindChildWithTag(newRow, "DenyRequest").GetComponent<Button>();

        detail1Text.text = memberDNs[i];
        detail2Text.text = memberAppExpiry[i];

        acceptBtn.onClick.AddListener(delegate { OnAcceptJoinRequest(acceptBtn.gameObject); });
        denyBtn.onClick.AddListener(delegate { OnDenyJoinRequest(denyBtn.gameObject); });

        delay += 0.25f;
        StartCoroutine(FadeInRow(membersListGroup, background, detail1Text, detail2Text, acceptBtn, denyBtn, delay));
    }

    private void ResetAllRows(GridLayoutGroup group, List<GameObject> list, int type)
    {
        delay = 0;

        foreach (var item in list)
        {
            ResetListRow(item, type);
        }

        group.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(1200, 0);

        list.Clear();
    }

    private void ResetListRow(GameObject row, int type)
    {
        CanvasGroup background = FindChildWithTag(row, "RowBG").GetComponent<CanvasGroup>();
        TMP_Text detail1Text = FindChildWithTag(row, "SocialDetail1Text").GetComponent<TMP_Text>();
        TMP_Text detail2Text = FindChildWithTag(row, "SocialDetail2Text").GetComponent<TMP_Text>();
        Color32 defaultColor = new Color32(255, 255, 255, 255);

        switch (type)
        {
            case 0:
                Button friendBtn = FindChildWithTag(row, "Kick").GetComponent<Button>();
                CanvasGroup friendBtnCG = friendBtn.gameObject.GetComponent<CanvasGroup>();
                ColorBlock friendBtnCB = friendBtn.colors;

                friendBtn.onClick.RemoveAllListeners();
                friendBtn.interactable = false;

                friendBtnCB.disabledColor = defaultColor;
                friendBtn.colors = friendBtnCB;

                friendBtnCG.alpha = 0;
                break;
            case 1:
                Button acceptBtn = FindChildWithTag(row, "AcceptRequest").GetComponent<Button>();
                Button denyBtn = FindChildWithTag(row, "DenyRequest").GetComponent<Button>();
                CanvasGroup acceptBtnCG = acceptBtn.gameObject.GetComponent<CanvasGroup>();
                CanvasGroup denyBtnCG = denyBtn.gameObject.GetComponent<CanvasGroup>();
                ColorBlock acceptBtnCB = acceptBtn.colors;
                ColorBlock denyBtnCB = denyBtn.colors;

                acceptBtn.onClick.RemoveAllListeners();
                denyBtn.onClick.RemoveAllListeners();
                acceptBtn.interactable = false;
                denyBtn.interactable = false;

                acceptBtnCB.disabledColor = defaultColor;
                denyBtnCB.disabledColor = defaultColor;
                acceptBtn.colors = acceptBtnCB;
                denyBtn.colors = denyBtnCB;

                acceptBtnCG.alpha = 0;
                denyBtnCG.alpha = 0;
                break;
            case 2:
                Button cancelBtn = FindChildWithTag(row, "CancelRequest").GetComponent<Button>();
                CanvasGroup cancelBtnCG = cancelBtn.gameObject.GetComponent<CanvasGroup>();
                ColorBlock cancelBtnCB = cancelBtn.colors;

                cancelBtn.onClick.RemoveAllListeners();
                cancelBtn.interactable = false;

                cancelBtnCB.disabledColor = defaultColor;
                cancelBtn.colors = cancelBtnCB;

                cancelBtnCG.alpha = 0;
                break;
        }

        background.alpha = 0;
        Color32 nameColor = detail1Text.color;
        Color32 leaderColor = detail2Text.color;

        nameColor.a = 0;
        leaderColor.a = 0;

        detail1Text.color = nameColor;
        detail2Text.color = leaderColor;

        detail1Text.text = "";
        detail2Text.text = "";
    }

    private IEnumerator FadeInRow(GridLayoutGroup glGroup, CanvasGroup background, TMP_Text name, TMP_Text status, Button btn1, Button btn2, float delay)
    {
        yield return new WaitForSeconds(delay);

        Coroutine fadeInBG = StartCoroutine(FadeInBG(background));

        yield return new WaitForSeconds(0.25f);

        glGroup.gameObject.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 75);
        
        Coroutine fadeInTextAndButton;
        
        if (btn1 != null && btn2 == null)
        {
            fadeInTextAndButton = StartCoroutine(FadeInTextAndButton(name, status, btn1.GetComponent<CanvasGroup>(), null));
            
            yield return fadeInBG;
            yield return fadeInTextAndButton;
        }
        else if (btn1 != null && btn2 != null)
        {
            fadeInTextAndButton = StartCoroutine(FadeInTextAndButton(name, status, btn1.GetComponent<CanvasGroup>(), btn2.GetComponent<CanvasGroup>()));
            
            yield return fadeInBG;
            yield return fadeInTextAndButton;
        }
        else if (btn1 == null && btn2 == null)
        {
            fadeInTextAndButton = StartCoroutine(FadeInTextAndButton(name, status, null, null));
            
            yield return fadeInBG;
            yield return fadeInTextAndButton;
        }
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

    private IEnumerator FadeInTextAndButton(TMP_Text name, TMP_Text status, CanvasGroup btnCG1, CanvasGroup btnCG2)
    {
        bool enableInteraction = true;
        Color textColor = name.color;

        float targetAlpha = 1f;

        while (Mathf.Abs(textColor.a - targetAlpha) > 0)
        {
            textColor.a = Mathf.Lerp(textColor.a, targetAlpha, 2 * Time.deltaTime);
            
            name.color = textColor;
            status.color = textColor;

            if (btnCG1 != null)
            {
                btnCG1.alpha = textColor.a;
            }
            if (btnCG2 != null)
            {
                btnCG2.alpha = textColor.a;
            }

            if (textColor.a >= 0.99f && enableInteraction)
            {
                enableInteraction = false;
                
                if (btnCG1 != null)
                {
                    btnCG1.GetComponent<Button>().interactable = true;
                }
                if (btnCG2 != null)
                {
                    btnCG2.GetComponent<Button>().interactable = true;
                }
            }

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

    private void ResetAllDatas()
    {
        memberEntityIDs.Clear();
        memberIDs.Clear();
        memberDNs.Clear();
        memberRoles.Clear();
        memberAppExpiry.Clear();
    }

    void OnError(PlayFabError e)
    {
        Debug.Log("Error : " + e.GenerateErrorReport());
    }
}
