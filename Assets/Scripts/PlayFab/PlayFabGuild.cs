using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.GroupsModels;

public class PlayFabGuild : MonoBehaviour
{
    [System.Serializable]
    public class GuildData
    {
        public string[] GuildIDs;
        public string[] GuildDatas;
    }

    [System.Serializable]
    public class GetDisplayNameResults
    {
        public List<string> DisplayNames = new();
    }

    public enum ListType
    {
        GUILDSLIST,
        REQUESTSLIST
    }

    public static List<GameObject> guildItems = new List<GameObject>();
    public static List<GameObject> requestItems = new List<GameObject>();

    [SerializeField] public GameObject guildsRowPrefab;
    [SerializeField] public GameObject requestRowPrefab;

    [SerializeField] public GridLayoutGroup guildsListGroup;
    [SerializeField] public GridLayoutGroup requestsListGroup;

    [SerializeField] public TMP_Text titleText;
    [SerializeField] public TMP_Text detail2Text;
    [SerializeField] public Button myGuildButton;
    [SerializeField] public Button listTypeButton;
    [SerializeField] public Button createGuildButton;

    [SerializeField] public TMP_Text createGuildText;
    [SerializeField] public TMP_InputField if_guildName;

    [SerializeField] public TMP_InputField if_searchByGuildName;
    [SerializeField] public TMP_InputField if_searchByLeaderName;

    public List<string> guildIDs;
    public List<string> guildDatas;
    public List<string> guildNames;
    public List<string> guildLeaderIDs;
    public List<string> guildLeaderDNs;

    public ListType currListType;

    private float delay = 0;

    void Awake()
    {
        currListType = ListType.GUILDSLIST;
    }

    public void OnButtonSwitchType()
    {
        switch (currListType)
        {
            case ListType.GUILDSLIST:
                currListType = ListType.REQUESTSLIST;
                titleText.text = "REQUESTS";

                guildsListGroup.gameObject.SetActive(false);

                detail2Text.text = "Status";
                if_searchByGuildName.gameObject.SetActive(false);
                listTypeButton.GetComponentInChildren<TMP_Text>().text = "Guilds";
                listTypeButton.interactable = false;

                OnButtonRequestsList();
                break;
            case ListType.REQUESTSLIST:
                currListType = ListType.GUILDSLIST;
                titleText.text = "GUILDS";

                requestsListGroup.gameObject.SetActive(false);

                detail2Text.text = "Guild Leader";
                if_searchByGuildName.gameObject.SetActive(true);
                listTypeButton.GetComponentInChildren<TMP_Text>().text = "Requests";
                listTypeButton.interactable = false;

                OnButtonGuildsList();
                break;
        }

        if (PlayFabManager.currGuildID == null)
        {
            myGuildButton.interactable = false;
            myGuildButton.GetComponentInChildren<TMP_Text>().text = "My Guild";
        }
        else
        {
            myGuildButton.interactable = true;
            myGuildButton.GetComponentInChildren<TMP_Text>().text = "<> " + PlayFabManager.currGuildName + " <>";
        }
    }

    public void OnEnterPanel()
    {
        if (PlayFabManager.currGuildID == null)
        {
            myGuildButton.interactable = false;
            myGuildButton.GetComponentInChildren<TMP_Text>().text = "My Guild";
        }
        else
        {
            myGuildButton.interactable = true;
            myGuildButton.GetComponentInChildren<TMP_Text>().text = "<> " + PlayFabManager.currGuildName + " <>";
        }

        OnButtonGuildsList();
    }

    public void OnGuildNameInputChange()
    {
        if (if_searchByGuildName.text != "")
        {
            if_searchByLeaderName.text = "";
            StopAllCoroutines();
            StartCoroutine(DelayBeforeSearching(if_searchByGuildName.text, "Name"));
        }
        else if (if_searchByGuildName.text == "" && if_searchByLeaderName.text == "")
        {
            StopAllCoroutines();
            OnButtonGuildsList();
        }
    }

    public void OnGuildLeaderInputChange()
    {
        if (if_searchByLeaderName.text != "")
        {
            if_searchByGuildName.text = "";
            StopAllCoroutines();
            StartCoroutine(DelayBeforeSearching(if_searchByLeaderName.text, "Leader"));
        }
        else if (if_searchByGuildName.text == "" && if_searchByLeaderName.text == "")
        {
            StopAllCoroutines();
            OnButtonGuildsList();
        }
    }

    private IEnumerator DelayBeforeSearching(string input, string type)
    {
        yield return new WaitForSeconds(1.5f);

        OnSearchGuildsList(input, type);
    }

    public void OnSearchGuildsList(string input, string type)
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
        {
            FunctionName = "FindGuilds",
            FunctionParameter = new
            {
                searchInput = input,
                searchType = type
            }
        }, fgResult=>
        {
            ResetAllDatas();

            var jsonString = fgResult.FunctionResult != null ? fgResult.FunctionResult.ToString() : null;

            if (!string.IsNullOrEmpty(jsonString))
            {
                var guildData = JsonUtility.FromJson<GuildData>(jsonString);

                guildIDs = guildData.GuildIDs.ToList<string>();
                guildDatas = guildData.GuildDatas.ToList<string>();

                Debug.Log(jsonString);

                if (guildIDs != null && guildDatas != null)
                {
                    ProcessGuildData();
                }
            }
        }, OnError);
    }

    public void OnButtonGuildsList()
    {
        currListType = ListType.GUILDSLIST;

        try
        {
            PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
            {
                FunctionName = "RefreshGuilds",
                FunctionParameter = new
                {
                    guildAmount = 10
                }
            }, rgResult=>
            {
                ResetAllDatas();
                
                var jsonString = rgResult.FunctionResult.ToString();
                var guildData = JsonUtility.FromJson<GuildData>(jsonString);

                guildIDs = guildData.GuildIDs.ToList<string>();
                guildDatas = guildData.GuildDatas.ToList<string>();

                Debug.Log(jsonString);

                if (guildIDs != null && guildDatas != null)
                {
                    ProcessGuildData();
                }
            }, OnError);
        }
        catch {}
    }

    private void ProcessGuildData()
    {
        for (int i = 0; i < guildDatas.Count; i++)
        {
            List<string> splitDatas = guildDatas[i].Split('|').ToList<string>();

            if (splitDatas.Count >= 2)
            {
                guildNames.Add(splitDatas[0]);
                guildLeaderIDs.Add(splitDatas[1]);
            }
            else
            {
                Debug.LogError("Invalid format in Guild Data: " + guildDatas[i]);
                continue;
            }
        }

        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
        {
            FunctionName = "GetDisplayNames",
            FunctionParameter = new
            {
                PlayFabIDs = JSONManager.StringListToJSON("IDs", guildLeaderIDs)
            }
        }, csResult=>
        {
            var jsonString = csResult.FunctionResult.ToString();
            var displayNames = JsonUtility.FromJson<GetDisplayNameResults>(jsonString);

            guildLeaderDNs = displayNames.DisplayNames.ToList();

            OnGuildListGet();
            listTypeButton.interactable = true;
        }, OnError);
    }

    public void OnButtonRequestsList()
    {
        currListType = ListType.REQUESTSLIST;
        ResetAllDatas();

        PlayFabGroupsAPI.ListMembershipOpportunities(new ListMembershipOpportunitiesRequest
        {

        }, ilResult=>
        {
            guildIDs = new List<string>();
            guildNames = new List<string>();
            int total = ilResult.Invitations.Count;
            int i = 0;

            foreach (var invite in ilResult.Invitations)
            {
                guildIDs.Add(invite.Group.Id);

                i++;

                if (total == i)
                {
                    PlayFabGroupsAPI.GetGroup(new GetGroupRequest
                    {
                        Group = new()
                        {
                            Id = guildIDs[0],
                            Type = "group"
                        }
                    }, ggResult=>
                    {
                        guildNames.Add(ggResult.GroupName);

                        OnRequestsListGet();
                        listTypeButton.interactable = true;
                    }, OnError);
                }
            }
            
            listTypeButton.interactable = true;
        }, OnError);
    }

    public void OnButtonCreateGuild()
    {
        if (!if_guildName.text.Contains("|") && PlayFabManager.currGuildID == null)
        {
            PlayFabGroupsAPI.CreateGroup(new CreateGroupRequest
            {
                GroupName = if_guildName.text
            }, createGuildResult=>
            {
                PlayFabManager.currGuildID = createGuildResult.Group;
                PlayFabManager.currGuildName = createGuildResult.GroupName;

                myGuildButton.GetComponentInChildren<TMP_Text>().text = "<> " + PlayFabManager.currGuildName + " <>";

                PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
                {
                    FunctionName = "AddGuildToData",
                    FunctionParameter = new
                    {
                        guildID = PlayFabManager.currGuildID.Id,
                        guildName = PlayFabManager.currGuildName,
                        guildLeaderID = PlayFabManager.currPlayFabID
                    }
                }, addDataResult=>
                {
                    createGuildText.text = "Created Guild";
                    myGuildButton.interactable = true;
                    OnButtonGuildsList();
                }, OnError);
            }, OnError);
        }
        else if (PlayFabManager.currGuildID != null)
        {
            createGuildText.text = "Already in Guild";
        }
    }

    public void OnButtonRequestGuild(GameObject btn)
    {
        string guildDN = FindChildWithTag(btn.transform.parent.gameObject, "SocialDetail1Text").GetComponent<TMP_Text>().text;
        string guildID = "";

        Debug.Log("GuildDN : " + guildDN);

        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
        {
            FunctionName = "FindGuilds",
            FunctionParameter = new
            {
                searchInput = guildDN,
                searchType = "Name"
            }
        }, fgResult=>
        {
            var jsonString = fgResult.FunctionResult != null ? fgResult.FunctionResult.ToString() : null;

            if (!string.IsNullOrEmpty(jsonString))
            {
                var guildData = JsonUtility.FromJson<GuildData>(jsonString);

                List<string> guildIDList = guildData.GuildIDs.ToList<string>();

                if (guildIDList != null)
                {
                    guildID = guildIDList[0];

                    PlayFabGroupsAPI.ApplyToGroup(new ApplyToGroupRequest
                    {
                        Group = new()
                        {
                            Id = guildID,
                            Type = "group"
                        }
                    }, atgResult=>
                    {
                        btn.GetComponentInChildren<TMP_Text>().text = "Applied";
                        btn.GetComponent<Button>().interactable = false;
                    }, OnError);
                }
            }
        }, OnError);
    }

    public void OnButtonAcceptGuildInvite(GameObject btn)
    {
        Debug.Log("Button : Accept Guild Invite pressed.");
        string guildDN = FindChildWithTag(btn.transform.parent.gameObject, "SocialDetail1Text").GetComponent<TMP_Text>().text;
        string guildID = "";

        Debug.Log("GuildDN : " + guildDN);

        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
        {
            FunctionName = "FindGuilds",
            FunctionParameter = new
            {
                searchInput = guildDN,
                searchType = "Name"
            }
        }, fgResult=>
        {
            var jsonString = fgResult.FunctionResult != null ? fgResult.FunctionResult.ToString() : null;

            if (!string.IsNullOrEmpty(jsonString))
            {
                var guildData = JsonUtility.FromJson<GuildData>(jsonString);

                List<string> guildIDList = guildData.GuildIDs.ToList<string>();

                if (guildIDList != null)
                {
                    guildID = guildIDList[0];

                    PlayFabGroupsAPI.AcceptGroupInvitation(new AcceptGroupInvitationRequest
                    {
                        Group = new()
                        {
                            Id = guildID,
                            Type = "group"
                        }
                    }, aiResult=>
                    {
                        PlayFabGroupsAPI.GetGroup(new GetGroupRequest
                        {
                            Group = new()
                            {
                                Id = guildID,
                                Type = "group"
                            }
                        }, ggResult=>
                        {
                            PlayFabManager.currGuildID = ggResult.Group;
                            PlayFabManager.currGuildName = ggResult.GroupName;

                            myGuildButton.GetComponentInChildren<TMP_Text>().text = "<> " + PlayFabManager.currGuildName + " <>";

                            OnRequestsListGet();
                        }, OnError);
                    }, OnError);
                }
            }
        }, OnError);
    }

    public void OnButtonDenyGuildInvite(GameObject btn)
    {
        Debug.Log("Button : Deny Guild Invite pressed.");
        string guildDN = FindChildWithTag(btn.transform.parent.gameObject, "SocialDetail1Text").GetComponent<TMP_Text>().text;
        string guildID = "";

        Debug.Log("GuildDN : " + guildDN);

        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
        {
            FunctionName = "FindGuilds",
            FunctionParameter = new
            {
                searchInput = guildDN,
                searchType = "Name"
            }
        }, fgResult=>
        {
            var jsonString = fgResult.FunctionResult != null ? fgResult.FunctionResult.ToString() : null;

            if (!string.IsNullOrEmpty(jsonString))
            {
                var guildData = JsonUtility.FromJson<GuildData>(jsonString);

                List<string> guildIDList = guildData.GuildIDs.ToList<string>();

                if (guildIDList != null)
                {
                    guildID = guildIDList[0];

                    PlayFabGroupsAPI.RemoveGroupInvitation(new RemoveGroupInvitationRequest
                    {
                        Group = new()
                        {
                            Id = guildID,
                            Type = "group"
                        },
                        Entity = new()
                        {
                            Id = PlayFabManager.currTitleID,
                            Type = "title_player_account"
                        }
                    }, aiResult=>
                    {
                        OnRequestsListGet();
                    }, OnError);
                }
            }
        }, OnError);
    }

    public void OnGuildListGet()
    {
        StopAllCoroutines();

        guildsListGroup.gameObject.SetActive(true);

        for (int i = 0; i < guildItems.Count; i++)
        {
            StartCoroutine(ObjectPoolManager.Instance.ReturnObjectToPool(guildItems[i]));
        }

        ResetAllRows(guildsListGroup, guildItems, 0);

        for (int i = 0; i < guildIDs.Count; i++)
        {
            UpdateGuildsListRow(i);
        }
    }

    private void UpdateGuildsListRow(int i)
    {
        GameObject newRow = ObjectPoolManager.Instance.SpawnObject(guildsRowPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        guildItems.Add(newRow);

        newRow.transform.SetParent(guildsListGroup.transform);
        newRow.transform.localPosition = new Vector3(0, 0, 0);
        newRow.transform.localRotation = Quaternion.Euler(0, 0, 0);
        newRow.transform.localScale = new Vector3(1, 1, 1);
        newRow.transform.SetSiblingIndex(i);

        ResetListRow(newRow, 0);

        Image background = FindChildWithTag(newRow, "RowBG").GetComponent<Image>();
        TMP_Text detail1Text = FindChildWithTag(newRow, "SocialDetail1Text").GetComponent<TMP_Text>();
        TMP_Text detail2Text = FindChildWithTag(newRow, "SocialDetail2Text").GetComponent<TMP_Text>();
        Button joinBtn = FindChildWithTag(newRow, "JoinRequest").GetComponent<Button>();

        detail1Text.text = guildNames[i];
        detail2Text.text = guildLeaderDNs[i];

        joinBtn.onClick.AddListener(delegate { OnButtonRequestGuild(joinBtn.gameObject); });

        delay += 0.25f;
        StartCoroutine(FadeInRow(guildsListGroup, background, detail1Text, detail2Text, joinBtn, null, delay));
    }

    public void OnRequestsListGet()
    {
        StopAllCoroutines();

        requestsListGroup.gameObject.SetActive(true);

        for (int i = 0; i < requestItems.Count; i++)
        {
            StartCoroutine(ObjectPoolManager.Instance.ReturnObjectToPool(requestItems[i]));
        }

        ResetAllRows(requestsListGroup, requestItems, 1);

        for (int i = 0; i < guildIDs.Count; i++)
        {
            UpdateRequestListRow(i);
        }
    }

    private void UpdateRequestListRow(int i)
    {
        GameObject newRow = ObjectPoolManager.Instance.SpawnObject(requestRowPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        requestItems.Add(newRow);

        newRow.transform.SetParent(requestsListGroup.transform);
        newRow.transform.localPosition = new Vector3(0, 0, 0);
        newRow.transform.localRotation = Quaternion.Euler(0, 0, 0);
        newRow.transform.localScale = new Vector3(1, 1, 1);
        newRow.transform.SetSiblingIndex(i);

        ResetListRow(newRow, 1);

        Image background = FindChildWithTag(newRow, "RowBG").GetComponent<Image>();
        TMP_Text detail1Text = FindChildWithTag(newRow, "SocialDetail1Text").GetComponent<TMP_Text>();
        TMP_Text detail2Text = FindChildWithTag(newRow, "SocialDetail2Text").GetComponent<TMP_Text>();
        Button acceptBtn = FindChildWithTag(newRow, "AcceptRequest").GetComponent<Button>();
        Button denyBtn = FindChildWithTag(newRow, "DenyRequest").GetComponent<Button>();

        detail1Text.text = guildNames[i];
        detail2Text.text = "Invited";

        acceptBtn.onClick.AddListener(delegate { OnButtonAcceptGuildInvite(acceptBtn.gameObject); });
        denyBtn.onClick.AddListener(delegate { OnButtonDenyGuildInvite(denyBtn.gameObject); });
        
        delay += 0.25f;
        StartCoroutine(FadeInRow(guildsListGroup, background, detail1Text, detail2Text, acceptBtn, denyBtn, delay));
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
        Image background = FindChildWithTag(row, "RowBG").GetComponent<Image>();
        TMP_Text detail1Text = FindChildWithTag(row, "SocialDetail1Text").GetComponent<TMP_Text>();
        TMP_Text detail2Text = FindChildWithTag(row, "SocialDetail2Text").GetComponent<TMP_Text>();
        Color32 defaultColor = new Color32(255, 255, 255, 255);

        switch (type)
        {
            case 0:
                Button joinBtn = FindChildWithTag(row, "JoinRequest").GetComponent<Button>();
                CanvasGroup joinBtnCG = joinBtn.gameObject.GetComponent<CanvasGroup>();
                ColorBlock joinBtnCB = joinBtn.colors;

                joinBtn.onClick.RemoveAllListeners();
                joinBtn.interactable = false;

                joinBtnCB.disabledColor = defaultColor;
                joinBtn.colors = joinBtnCB;

                joinBtnCG.alpha = 0;
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

        Color bgColor = background.color;
        Color32 nameColor = detail1Text.color;
        Color32 leaderColor = detail2Text.color;

        bgColor.a = 0;
        nameColor.a = 0;
        leaderColor.a = 0;

        background.color = bgColor;
        detail1Text.color = nameColor;
        detail2Text.color = leaderColor;

        detail1Text.text = "";
        detail2Text.text = "";
    }

    private IEnumerator FadeInRow(GridLayoutGroup glGroup, Image background, TMP_Text name, TMP_Text status, Button btn1, Button btn2, float delay)
    {
        yield return new WaitForSeconds(delay);

        Coroutine fadeInBG = StartCoroutine(FadeInBG(background));

        yield return new WaitForSeconds(0.25f);

        glGroup.gameObject.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 75);
        
        Coroutine fadeInTextAndButton;

        if (btn1 != null && btn2 != null)
        {
            if (PlayFabManager.currGuildID != null)
            {
                ColorBlock btn1cb = btn1.colors;
                btn1cb.disabledColor = new Color32(128, 128, 128, 255);
                btn1.colors = btn1cb;

                ColorBlock btn2cb = btn2.colors;
                btn2cb.disabledColor = new Color32(128, 128, 128, 255);
                btn2.colors = btn2cb;
            }

            fadeInTextAndButton = StartCoroutine(FadeInTextAndButton(name, status, btn1.GetComponent<CanvasGroup>(), btn2.GetComponent<CanvasGroup>()));
        }
        else
        {
            if (PlayFabManager.currGuildID != null)
            {
                ColorBlock btn1cb = btn1.colors;
                btn1cb.disabledColor = new Color32(128, 128, 128, 255);
                btn1.colors = btn1cb;
            }

            fadeInTextAndButton = StartCoroutine(FadeInTextAndButton(name, status, btn1.GetComponent<CanvasGroup>(), null));
        }

        yield return fadeInBG;
        yield return fadeInTextAndButton;
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
                
                if (btnCG1 != null && PlayFabManager.currGuildID == null)
                {
                    btnCG1.GetComponent<Button>().interactable = true;
                }
                if (btnCG2 != null && PlayFabManager.currGuildID == null)
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
        guildIDs.Clear();
        guildDatas.Clear();
        guildNames.Clear();
        guildLeaderIDs.Clear();
        guildLeaderDNs.Clear();
    }

    void OnError(PlayFabError e)
    {
        Debug.Log("Error : " + e.GenerateErrorReport());
        listTypeButton.interactable = true;
    }
}
