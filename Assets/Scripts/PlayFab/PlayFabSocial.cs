using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;

public class PlayFabSocial : MonoBehaviour
{
    public enum ListType
    {
        FRIENDLIST,
        REQUESTPENDINGLIST
    }

    public PlayFabObjectPoolManager opManager;

    public static List<GameObject> friendItems = new List<GameObject>();
    public static List<GameObject> requestItems = new List<GameObject>();
    public static List<GameObject> pendingItems = new List<GameObject>();
    public static List<GameObject> tradeItems = new List<GameObject>();

    [SerializeField] public GameObject friendRowPrefab;
    [SerializeField] public GameObject requestRowPrefab;
    [SerializeField] public GameObject pendingRowPrefab;
    [SerializeField] public GameObject tradeRowPrefab;

    [SerializeField] public ScrollRect socialScrollRect;
    [SerializeField] public GridLayoutGroup friendListGroup;
    [SerializeField] public GridLayoutGroup requestAndPendingListGroup;
    [SerializeField] public GridLayoutGroup tradeListGroup;

    [SerializeField] public Button listTypeButton;

    [SerializeField] private List<ItemInstance> invItems = new List<ItemInstance>();
    [SerializeField] public GameObject leftItem1, leftItem2;
    [SerializeField] public GameObject rightItem1, rightItem2;
    [SerializeField] public GameObject tradePanel;
    [SerializeField] public TMP_Text tradeTitleText;
    [SerializeField] private TMP_Text tradeeDN;
    [SerializeField] private string tradeeID;

    [SerializeField] private List<TradeInfo> tradeInfos;
    [SerializeField] private List<string> tradeIDs;
    [SerializeField] private List<string> traderIDs;
    [SerializeField] private List<string> traderDNs;

    [SerializeField] private ItemInstance offerInvItemInstance1, offerInvItemInstance2;
    [SerializeField] private CatalogItem requestCatalogItem1, requestCatalogItem2;

    [SerializeField] public TMP_Text titleText;
    [SerializeField] public TMP_Text addFriendText;
    [SerializeField] public TMP_InputField if_recipient;

    private ListType currListType;
    private float delay = 0;

    private int offerItem = 0, wantItem = 0;

    public void OnButtonSwitchType()
    {
        switch (currListType)
        {
            case ListType.FRIENDLIST:
                currListType = ListType.REQUESTPENDINGLIST;
                titleText.text = "REQUESTS & PENDING";
                socialScrollRect.content = requestAndPendingListGroup.GetComponent<RectTransform>();

                friendListGroup.gameObject.SetActive(false);

                listTypeButton.GetComponentInChildren<TMP_Text>().text = "Friend List";

                OnButtonRequestAndPendingList();
                break;
            case ListType.REQUESTPENDINGLIST:
                currListType = ListType.FRIENDLIST;
                titleText.text = "FRIENDS";
                socialScrollRect.content = friendListGroup.GetComponent<RectTransform>();

                requestAndPendingListGroup.gameObject.SetActive(false);

                listTypeButton.GetComponentInChildren<TMP_Text>().text = "Requests / Pending";

                OnButtonFriendList();
                break;
        }
    }

    public void OnButtonFriendList()
    {
        currListType = ListType.FRIENDLIST;

        PlayFabClientAPI.GetFriendsList(new GetFriendsListRequest
        {
            ProfileConstraints = new PlayerProfileViewConstraints
            {
                ShowLastLogin = true
            }
        }, result=>
        {
            OnFriendListGet(result);
        }, OnError);
    }

    public void OnButtonRequestAndPendingList()
    {
        titleText.text = "REQUESTS & PENDING";
        socialScrollRect.content = requestAndPendingListGroup.GetComponent<RectTransform>();
        currListType = ListType.REQUESTPENDINGLIST;

        PlayFabClientAPI.GetFriendsList(new GetFriendsListRequest
        {
            ProfileConstraints = new PlayerProfileViewConstraints
            {
                ShowLastLogin = true
            }
        }, result=>
        {
            OnRequestPendingListGet(result);
        }, OnError);
    }

    public void OnButtonSendFriendRequest()
    {
        try
        {
            PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest
            {
                Email = if_recipient.text
            }, result=>
            {
                OnFoundUser(result.AccountInfo.PlayFabId.ToString());
            }, error=>
            {
                PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest
                {
                    PlayFabId = if_recipient.text
                }, result=>
                {
                    OnFoundUser(result.AccountInfo.PlayFabId.ToString());
                }, error=>
                {
                    PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest
                    {
                        TitleDisplayName = if_recipient.text
                    }, result=>
                    {
                        OnFoundUser(result.AccountInfo.PlayFabId.ToString());
                    }, error=>
                    {
                        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest
                        {
                            Username = if_recipient.text
                        }, result=>
                        {
                            OnFoundUser(result.AccountInfo.PlayFabId.ToString());
                        }, OnError);
                    });
                });
            });
        }
        catch {}
    }

    void OnFoundUser(string recipientIDContainer)
    {
        var req = new ExecuteCloudScriptRequest()
        {
            FunctionName = "SendFriendRequest",
            FunctionParameter = new
            {
                senderID = PlayFabManager.currPlayFabID,
                recipientID = recipientIDContainer
            }
        };

        PlayFabClientAPI.ExecuteCloudScript(req, result=>
        {
            addFriendText.text = "Friend Request Sent";
        }, OnError);
    }

    public void OnButtonTradesList()
    {
        ClearListData();

        PlayFabClientAPI.GetUserData(new GetUserDataRequest
        {
            PlayFabId = PlayFabManager.currPlayFabID
        }, result=>
        {
            if (result.Data.ContainsKey("IncomingTrades"))
            {
                List<Dictionary<string, string>> jsonDictionary = 
                PlayFabSimpleJson.DeserializeObject<List<Dictionary<string, string>>>(result.Data["IncomingTrades"].Value.ToString());

                foreach (var request in jsonDictionary)
                {
                    foreach (var info in request)
                    {
                        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest
                        {
                            PlayFabId = info.Value
                        }, aiResult=>
                        {
                            tradeIDs.Add(info.Key);
                            traderIDs.Add(info.Value);
                            traderDNs.Add(aiResult.AccountInfo.TitleInfo.DisplayName);
                        }, OnError);

                        PlayFabClientAPI.GetTradeStatus(new GetTradeStatusRequest
                        {
                            TradeId = info.Key,
                            OfferingPlayerId = info.Value
                        }, gtsResult=>
                        {
                            tradeInfos.Add(gtsResult.Trade);
                        }, OnError);
                    }
                }

                StartCoroutine(WaitForRequests(jsonDictionary.Count));
            }
        }, OnError);
    }

    private IEnumerator WaitForRequests(int condition)
    {
        yield return new WaitUntil(() => tradeInfos.Count == condition && traderDNs.Count == condition);

        OnTradeListGet();
    }

    public void OnButtonAcceptFriendRequest(GameObject btn)
    {
        Debug.Log("Button : Accept Friend Request pressed.");
        string friendDN = FindChildWithTag(btn.transform.parent.gameObject, "SocialDetail1Text").GetComponent<TMP_Text>().text;
        string friendID = "";

        Debug.Log("FriendDN : " + friendDN);

        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest
        {
            TitleDisplayName = friendDN
        }, result=>
        {
            friendID = result.AccountInfo.PlayFabId.ToString();

            PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
            {
                FunctionName = "AcceptFriendRequest",
                FunctionParameter = new
                {
                    senderID = PlayFabManager.currPlayFabID,
                    recipientID = friendID
                }
            }, result=>
            {
                OnButtonRequestAndPendingList();
            }, OnError);
        }, OnError);
    }

    public void OnButtonDenyFriendRequest(GameObject btn)
    {
        Debug.Log("Button : Deny Friend Request pressed.");
        string requesteeDN = FindChildWithTag(btn.transform.parent.gameObject, "SocialDetail1Text").GetComponent<TMP_Text>().text;
        string requesteeID = "";

        Debug.Log("FriendDN : " + requesteeDN);

        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest
        {
            TitleDisplayName = requesteeDN
        }, result=>
        {
            requesteeID = result.AccountInfo.PlayFabId.ToString();

            PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
            {
                FunctionName = "DenyFriendRequest",
                FunctionParameter = new
                {
                    senderID = PlayFabManager.currPlayFabID,
                    recipientID = requesteeID
                }
            }, result=>
            {
                if (currListType == ListType.FRIENDLIST)
                {
                    OnButtonFriendList();
                }
                else
                {
                    OnButtonRequestAndPendingList();
                }
            }, OnError);
        }, OnError);
    }

    public void OnButtonTradeRequest(GameObject btn)
    {
        Debug.Log("Button : Trade Request pressed.");
        string otherDN = FindChildWithTag(btn.transform.parent.gameObject, "SocialDetail1Text").GetComponent<TMP_Text>().text;
        
        tradeTitleText.text = "TRADE";
        tradeeDN.text = otherDN;

        leftItem1.SetActive(false);
        leftItem2.SetActive(false);
        rightItem1.SetActive(false);
        rightItem2.SetActive(false);

        tradePanel.SetActive(true);

        Debug.Log("FriendDN : " + otherDN);

        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest
        {
            TitleDisplayName = otherDN
        }, aiResult=>
        {
            tradeeID = aiResult.AccountInfo.PlayFabId.ToString();

            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest
            {

            }, giResult=>
            {
                invItems = giResult.Inventory;

                foreach (var item in invItems)
                {
                    if (item.ItemId == "Gold")
                    {
                        leftItem1.SetActive(true);
                        offerInvItemInstance1 = item;
                    }

                    if (item.ItemId == "Ship_Speed")
                    {
                        leftItem2.SetActive(true);
                        offerInvItemInstance2 = item;
                    }
                }

                rightItem1.SetActive(true);
                rightItem2.SetActive(true);

                PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest
                {
                    CatalogVersion = "main"
                }, gcResult=>
                {
                    foreach (var catItem in gcResult.Catalog)
                    {
                        if (catItem.ItemId == "Gold")
                        {
                            requestCatalogItem1 = catItem;
                        }

                        if (catItem.ItemId == "Ship_Speed")
                        {
                            requestCatalogItem2 = catItem;
                        }
                    }
                }, OnError);
            }, OnError);
        }, OnError);
    }

    public void ResetColor(int index)
    {
        switch (index)
        {
            case 0:
                leftItem1.GetComponent<Image>().color = new Color32(13, 4, 23, 255);
                leftItem2.GetComponent<Image>().color = new Color32(13, 4, 23, 255);
                break;
            case 1:
                rightItem1.GetComponent<Image>().color = new Color32(13, 4, 23, 255);
                rightItem2.GetComponent<Image>().color = new Color32(13, 4, 23, 255);
                break;
        }
    }

    public void SetColor(int index)
    {
        switch (index)
        {
            case 0:
                leftItem1.GetComponent<Image>().color = new Color32(56, 132, 66, 255);
                offerItem = 1;
                break;
            case 1:
                leftItem2.GetComponent<Image>().color = new Color32(56, 132, 66, 255);
                offerItem = 2;
                break;
            case 2:
                rightItem1.GetComponent<Image>().color = new Color32(56, 132, 66, 255);
                wantItem = 1;
                break;
            case 3:
                rightItem2.GetComponent<Image>().color = new Color32(56, 132, 66, 255);
                wantItem = 2;
                break;
        }
    }

    public void ConfirmTrade()
    {
        ItemInstance offeredItem = new ItemInstance();
        CatalogItem requestedItem = new CatalogItem();
        string tradeInfoID = "";

        switch (offerItem)
        {
            case 1:
                offeredItem = offerInvItemInstance1;
                break;
            case 2:
                offeredItem = offerInvItemInstance2;
                break;
            default:
                Debug.Log("No Items Selected");
                break;
        }

        switch (wantItem)
        {
            case 1:
                requestedItem = requestCatalogItem1;
                break;
            case 2:
                requestedItem = requestCatalogItem2;
                break;
            default:
                Debug.Log("No Items Requested");
                break;
        }

        if (offeredItem == null || requestedItem == null)
        {
            tradeTitleText.text = "INVALID TRADE";
            return;
        }

        PlayFabClientAPI.OpenTrade(new OpenTradeRequest
        {
            AllowedPlayerIds = new List<string>
            {
                tradeeID
            },
            OfferedInventoryInstanceIds = new List<string>
            {
                offeredItem.ItemInstanceId
            },
            RequestedCatalogItemIds = new List<string>
            {
                requestedItem.ItemId
            }
        }, otResult=>
        {
            tradeInfoID = otResult.Trade.TradeId;

            Dictionary<string, string> currTradeInfo = new()
            {
                {
                    tradeInfoID,
                    PlayFabManager.currPlayFabID
                }
            };

            PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
            {
                FunctionName = "SetTradeData",
                FunctionParameter = new
                {
                    requesteeID = tradeeID,
                    tradeInfo = currTradeInfo
                }
            }, utdResult=>
            {
                tradeTitleText.text = "SENT TRADE REQUEST";
            }, OnError);
        }, OnError);
    }

    public void OnButtonAcceptTrade(GameObject btn)
    {
        Debug.Log("Button : Accept Trade pressed.");
        string otherDN = FindChildWithTag(btn.transform.parent.gameObject, "SocialDetail1Text").GetComponent<TMP_Text>().text;
        int index = 0;

        for (int i = 0; i < traderDNs.Count; i++)
        {
            if (traderDNs[i].Contains(otherDN))
            {
                index = i;
            }
        }

        Debug.Log("Trade Status : " + tradeInfos[index].Status);

        Debug.Log("Offered Items : " + string.Join(", ", tradeInfos[index].OfferedInventoryInstanceIds));
        Debug.Log("Accepted Items : " + string.Join(", ", tradeInfos[index].RequestedCatalogItemIds));

        Debug.Log("[Trade]" + tradeIDs[index] + " | [TradeInfo]" + tradeInfos[index].TradeId);
        Debug.Log("OfferingPlayerID : " + traderIDs[index]);

        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest
        {

        }, giResult=>
        {
            foreach (var item in giResult.Inventory)
            {
                if (item.ItemId == tradeInfos[index].RequestedCatalogItemIds[0])
                {
                    PlayFabClientAPI.AcceptTrade(new AcceptTradeRequest
                    {
                        TradeId = tradeIDs[index],
                        OfferingPlayerId = traderIDs[index],
                        AcceptedInventoryInstanceIds = new()
                        {
                            {
                                item.ItemInstanceId
                            }
                        }
                    }, atResult=>
                    {
                        Debug.Log("Accepted Trade from [" + traderDNs[index] + "]");

                        RemoveFromTradeData(tradeIDs[index]);
                    }, OnError);

                    return;
                }
            }
        }, OnError);
    }

    public void OnButtonDenyTrade(GameObject btn)
    {
        Debug.Log("Button : Deny Trade pressed.");
        string otherDN = FindChildWithTag(btn.transform.parent.gameObject, "SocialDetail1Text").GetComponent<TMP_Text>().text;
        int index = 0;

        for (int i = 0; i < traderDNs.Count; i++)
        {
            if (traderDNs[i].Contains(otherDN))
            {
                index = i;
            }
        }

        RemoveFromTradeData(tradeIDs[index]);
    }

    private void RemoveFromTradeData(string keyToRemove)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest
        {
            PlayFabId = PlayFabManager.currPlayFabID
        }, result=>
        {
            if (result.Data.ContainsKey("IncomingTrades"))
            {
                List<Dictionary<string, string>> jsonDictionary = 
                PlayFabSimpleJson.DeserializeObject<List<Dictionary<string, string>>>(result.Data["IncomingTrades"].Value.ToString());

                foreach (var request in jsonDictionary)
                {
                    if (request.ContainsKey(keyToRemove))
                    {
                        jsonDictionary.Remove(request);

                        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
                        {
                            FunctionName = "UpdateTradeData",
                            FunctionParameter = new
                            {
                                requesteeID = PlayFabManager.currPlayFabID,
                                tradeInfo = jsonDictionary
                            }
                        }, csResult=>
                        {
                            OnButtonTradesList();
                        }, OnError);

                        return;
                    }
                }
            }
        }, OnError);
    }

    void OnFriendListGet(GetFriendsListResult r)
    {
        StopAllCoroutines();

        friendListGroup.gameObject.SetActive(true);
        requestAndPendingListGroup.gameObject.SetActive(false);

        for (int i = 0; i < friendItems.Count; i++)
        {
            PlayFabObjectPoolManager.ReturnObjectToPool(friendItems[i]);
        }

        ResetAllRows(friendListGroup, friendItems, 0);

        foreach (var item in r.Friends)
        {
            if (item.Tags.Contains("Friends"))
            {
                UpdateFriendListRow(item);
            }
        }
    }

    void OnRequestPendingListGet(GetFriendsListResult r)
    {
        StopAllCoroutines();

        requestAndPendingListGroup.gameObject.SetActive(true);
        friendListGroup.gameObject.SetActive(false);

        for (int i = 0; i < requestItems.Count; i++)
        {
            PlayFabObjectPoolManager.ReturnObjectToPool(requestItems[i]);
        }

        for (int i = 0; i < pendingItems.Count; i++)
        {
            PlayFabObjectPoolManager.ReturnObjectToPool(pendingItems[i]);
        }

        ResetAllRows(requestAndPendingListGroup, requestItems, 1);
        ResetAllRows(requestAndPendingListGroup, pendingItems, 2);

        foreach (var item in r.Friends)
        {
            if (item.Tags.Contains("Requester"))
            {
                UpdateRequestListRow(item);
            }
        }

        foreach (var item in r.Friends)
        {
            if (item.Tags.Contains("Requestee"))
            {
                UpdatePendingListRow(item);
            }
        }
    }

    private void OnTradeListGet()
    {
        StopAllCoroutines();

        for (int i = 0; i < tradeItems.Count; i++)
        {
            PlayFabObjectPoolManager.ReturnObjectToPool(tradeItems[i]);
        }

        ResetAllRows(tradeListGroup, tradeItems, 3);

        for (int i = 0; i < tradeInfos.Count; i++)
        {
            UpdateTradeListRow(tradeInfos[i], traderDNs[i]);
        }
    }

    private void UpdateFriendListRow(FriendInfo item)
    {
        GameObject newRow = opManager.SpawnObject(friendRowPrefab);
        friendItems.Add(newRow);

        newRow.transform.SetParent(friendListGroup.transform);
        newRow.transform.localPosition = new Vector3(0, 0, 0);
        newRow.transform.localRotation = Quaternion.Euler(0, 0, 0);
        newRow.transform.localScale = new Vector3(1, 1, 1);

        ResetListRow(newRow, 0);

        Image background = FindChildWithTag(newRow, "RowBG").GetComponent<Image>();
        TMP_Text detail1Text = FindChildWithTag(newRow, "SocialDetail1Text").GetComponent<TMP_Text>();
        TMP_Text detail2Text = FindChildWithTag(newRow, "SocialDetail2Text").GetComponent<TMP_Text>();
        Button tradeBtn = FindChildWithTag(newRow, "TradeButton").GetComponent<Button>();
        Button removeBtn = FindChildWithTag(newRow, "RemoveFriend").GetComponent<Button>();

        detail1Text.text = item.TitleDisplayName;
        detail2Text.text = item.Profile.LastLogin.ToString();

        tradeBtn.onClick.AddListener( delegate { OnButtonTradeRequest(tradeBtn.gameObject); });
        removeBtn.onClick.AddListener( delegate { OnButtonDenyFriendRequest(removeBtn.gameObject); });

        delay += 0.25f;
        StartCoroutine(FadeInRow(friendListGroup, background, detail1Text, detail2Text, tradeBtn, removeBtn, null, null, delay));
    }

    private void UpdateRequestListRow(FriendInfo item)
    {
        GameObject newRow = opManager.SpawnObject(requestRowPrefab);
        requestItems.Add(newRow);

        newRow.transform.SetParent(requestAndPendingListGroup.transform);
        newRow.transform.localPosition = new Vector3(0, 0, 0);
        newRow.transform.localRotation = Quaternion.Euler(0, 0, 0);
        newRow.transform.localScale = new Vector3(1, 1, 1);

        ResetListRow(newRow, 1);

        Image background = FindChildWithTag(newRow, "RowBG").GetComponent<Image>();
        TMP_Text detail1Text = FindChildWithTag(newRow, "SocialDetail1Text").GetComponent<TMP_Text>();
        TMP_Text detail2Text = FindChildWithTag(newRow, "SocialDetail2Text").GetComponent<TMP_Text>();
        Button acceptBtn = FindChildWithTag(newRow, "AcceptRequest").GetComponent<Button>();
        Button denyBtn = FindChildWithTag(newRow, "DenyRequest").GetComponent<Button>();

        detail1Text.text = item.TitleDisplayName;
        detail2Text.text = item.Profile.LastLogin.ToString();

        acceptBtn.onClick.AddListener(delegate { OnButtonAcceptFriendRequest(acceptBtn.gameObject); });
        denyBtn.onClick.AddListener(delegate { OnButtonDenyFriendRequest(denyBtn.gameObject); });
        
        delay += 0.25f;
        StartCoroutine(FadeInRow(requestAndPendingListGroup, background, detail1Text, detail2Text, acceptBtn, denyBtn, null, null, delay));
    }

    private void UpdatePendingListRow(FriendInfo item)
    {
        GameObject newRow = opManager.SpawnObject(pendingRowPrefab);
        pendingItems.Add(newRow);

        newRow.transform.SetParent(requestAndPendingListGroup.transform);
        newRow.transform.localPosition = new Vector3(0, 0, 0);
        newRow.transform.localRotation = Quaternion.Euler(0, 0, 0);
        newRow.transform.localScale = new Vector3(1, 1, 1);

        ResetListRow(newRow, 2);

        Image background = FindChildWithTag(newRow, "RowBG").GetComponent<Image>();
        TMP_Text detail1Text = FindChildWithTag(newRow, "SocialDetail1Text").GetComponent<TMP_Text>();
        TMP_Text detail2Text = FindChildWithTag(newRow, "SocialDetail2Text").GetComponent<TMP_Text>();
        Button cancelBtn = FindChildWithTag(newRow, "CancelRequest").GetComponent<Button>();

        detail1Text.text = item.TitleDisplayName;
        detail2Text.text = item.Profile.LastLogin.ToString();

        cancelBtn.onClick.AddListener(delegate { OnButtonDenyFriendRequest(cancelBtn.gameObject); });

        delay += 0.25f;
        StartCoroutine(FadeInRow(requestAndPendingListGroup, background, detail1Text, detail2Text, cancelBtn, null, null, null, delay));
    }

    private void UpdateTradeListRow(TradeInfo info, string traderDN)
    {
        GameObject newRow = opManager.SpawnObject(tradeRowPrefab);
        tradeItems.Add(newRow);
        Debug.Log(newRow.gameObject.name);

        newRow.transform.SetParent(tradeListGroup.transform);
        newRow.transform.localPosition = new Vector3(0, 0, 0);
        newRow.transform.localRotation = Quaternion.Euler(0, 0, 0);
        newRow.transform.localScale = new Vector3(1, 1, 1);

        ResetListRow(newRow, 3);

        Image background = FindChildWithTag(newRow, "RowBG").GetComponent<Image>();
        TMP_Text detail1Text = FindChildWithTag(newRow, "SocialDetail1Text").GetComponent<TMP_Text>();
        
        GameObject detail2 = FindChildWithTag(newRow, "SocialDetail2Text");
        Image offerItem1 = FindChildWithTag(detail2, "Item1").GetComponent<Image>();
        Image offerItem2 = FindChildWithTag(detail2, "Item2").GetComponent<Image>();

        GameObject detail3 = FindChildWithTag(newRow, "SocialDetail3Text");
        Image wantItem1 = FindChildWithTag(detail3, "Item1").GetComponent<Image>();
        Image wantItem2 = FindChildWithTag(detail3, "Item2").GetComponent<Image>();
        
        Button acceptBtn = FindChildWithTag(newRow, "AcceptRequest").GetComponent<Button>();
        Button denyBtn = FindChildWithTag(newRow, "DenyRequest").GetComponent<Button>();

        acceptBtn.onClick.AddListener(delegate { OnButtonAcceptTrade(acceptBtn.gameObject); });
        denyBtn.onClick.AddListener(delegate { OnButtonDenyTrade(denyBtn.gameObject); });
        
        detail1Text.text = traderDN;
        
        delay += 0.25f;

        if (info.OfferedCatalogItemIds[0] == "Gold")
        {
            StartCoroutine(FadeInRow(requestAndPendingListGroup, background, detail1Text, null, acceptBtn, denyBtn, offerItem1, null, delay));
        }
        else if (info.OfferedCatalogItemIds[0] == "Ship_Speed")
        {
            StartCoroutine(FadeInRow(requestAndPendingListGroup, background, detail1Text, null, acceptBtn, denyBtn, offerItem2, null, delay));
        }

        if (info.RequestedCatalogItemIds[0] == "Gold")
        {
            StartCoroutine(FadeInRow(requestAndPendingListGroup, background, detail1Text, null, acceptBtn, denyBtn, null, wantItem1, delay));
        }
        else if (info.RequestedCatalogItemIds[0] == "Ship_Speed")
        {
            StartCoroutine(FadeInRow(requestAndPendingListGroup, background, detail1Text, null, acceptBtn, denyBtn, null, wantItem2, delay));
        }
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
        TMP_Text detail2Text;
        Color32 statusColor;

        Button acceptBtn;
        Button denyBtn;

        CanvasGroup acceptBtnCG;
        CanvasGroup denyBtnCG;

        switch (type)
        {
            case 0:
                detail2Text = FindChildWithTag(row, "SocialDetail2Text").GetComponent<TMP_Text>();
                
                Button tradeBtn = FindChildWithTag(row, "TradeButton").GetComponent<Button>();
                Button removeBtn = FindChildWithTag(row, "RemoveFriend").GetComponent<Button>();

                CanvasGroup tradeBtnCG = tradeBtn.gameObject.GetComponent<CanvasGroup>();
                CanvasGroup removeBtnCG = removeBtn.gameObject.GetComponent<CanvasGroup>();

                tradeBtn.onClick.RemoveAllListeners();
                removeBtn.onClick.RemoveAllListeners();

                tradeBtnCG.alpha = 0;
                removeBtnCG.alpha = 0;

                tradeBtn.interactable = false;
                removeBtn.interactable = false;
                
                statusColor = detail2Text.color;
                statusColor.a = 0;
                detail2Text.color = statusColor;
                detail2Text.text = "";
                break;
            case 1:
                detail2Text = FindChildWithTag(row, "SocialDetail2Text").GetComponent<TMP_Text>();
                
                acceptBtn = FindChildWithTag(row, "AcceptRequest").GetComponent<Button>();
                denyBtn = FindChildWithTag(row, "DenyRequest").GetComponent<Button>();
                
                acceptBtnCG = acceptBtn.gameObject.GetComponent<CanvasGroup>();
                denyBtnCG = denyBtn.gameObject.GetComponent<CanvasGroup>();
                
                acceptBtn.onClick.RemoveAllListeners();
                denyBtn.onClick.RemoveAllListeners();
                
                acceptBtnCG.alpha = 0;
                denyBtnCG.alpha = 0;
                
                acceptBtn.interactable = false;
                denyBtn.interactable = false;
                
                statusColor = detail2Text.color;
                statusColor.a = 0;
                detail2Text.color = statusColor;
                detail2Text.text = "";
                break;
            case 2:
                detail2Text = FindChildWithTag(row, "SocialDetail2Text").GetComponent<TMP_Text>();
                Button cancelBtn = FindChildWithTag(row, "CancelRequest").GetComponent<Button>();
                CanvasGroup cancelBtnCG = cancelBtn.gameObject.GetComponent<CanvasGroup>();
                cancelBtn.onClick.RemoveAllListeners();
                cancelBtnCG.alpha = 0;
                cancelBtn.interactable = false;
                
                statusColor = detail2Text.color;
                statusColor.a = 0;
                detail2Text.color = statusColor;
                detail2Text.text = "";
                break;
            case 3:
                GameObject detail2Obj = FindChildWithTag(row, "SocialDetail2Text");
                Image offerObj1 = FindChildWithTag(detail2Obj, "Item1").GetComponent<Image>();
                Image offerObj2 = FindChildWithTag(detail2Obj, "Item2").GetComponent<Image>();

                GameObject detail3Obj = FindChildWithTag(row, "SocialDetail3Text");
                Image wantObj1 = FindChildWithTag(detail3Obj, "Item1").GetComponent<Image>();
                Image wantObj2 = FindChildWithTag(detail3Obj, "Item2").GetComponent<Image>();

                offerObj1.color = new Color32(255, 255, 255, 0);
                offerObj2.color = new Color32(255, 255, 255, 0);

                wantObj1.color = new Color32(255, 255, 255, 0);
                wantObj2.color = new Color32(255, 255, 255, 0);

                acceptBtn = FindChildWithTag(row, "AcceptRequest").GetComponent<Button>();
                denyBtn = FindChildWithTag(row, "DenyRequest").GetComponent<Button>();
                
                acceptBtnCG = acceptBtn.gameObject.GetComponent<CanvasGroup>();
                denyBtnCG = denyBtn.gameObject.GetComponent<CanvasGroup>();
                
                acceptBtn.onClick.RemoveAllListeners();
                denyBtn.onClick.RemoveAllListeners();
                
                acceptBtnCG.alpha = 0;
                denyBtnCG.alpha = 0;
                
                acceptBtn.interactable = false;
                denyBtn.interactable = false;
                break;
        }

        Color bgColor = background.color;
        Color32 nameColor = detail1Text.color;

        bgColor.a = 0;
        nameColor.a = 0;

        background.color = bgColor;
        detail1Text.color = nameColor;

        detail1Text.text = "";
    }

    private IEnumerator FadeInRow(GridLayoutGroup glGroup, Image background, TMP_Text name, TMP_Text status, Button btn1, Button btn2, Image item1, Image item2, float delay)
    {
        yield return new WaitForSeconds(delay);

        Coroutine fadeInBG = StartCoroutine(FadeInBG(background));

        yield return new WaitForSeconds(0.25f);

        glGroup.gameObject.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 70);
        
        Coroutine fadeInTextAndButton;

        if (status != null)
        {
            if (btn1 != null && btn2 != null)
            {
                fadeInTextAndButton = StartCoroutine(FadeInTextAndButton(name, status, btn1.GetComponent<CanvasGroup>(), btn2.GetComponent<CanvasGroup>()));
            }
            else
            {
                fadeInTextAndButton = StartCoroutine(FadeInTextAndButton(name, status, btn1.GetComponent<CanvasGroup>(), null));
            }
        }
        else
        {
            fadeInTextAndButton = StartCoroutine(FadeInTextImageButton(name, item1, item2, btn1.GetComponent<CanvasGroup>(), btn2.GetComponent<CanvasGroup>()));
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

    private IEnumerator FadeInTextImageButton(TMP_Text name, Image item1, Image item2, CanvasGroup btnCG1, CanvasGroup btnCG2)
    {
        bool enableInteraction = true;
        Color textColor = name.color;

        float targetAlpha = 1f;

        while (Mathf.Abs(textColor.a - targetAlpha) > 0)
        {
            textColor.a = Mathf.Lerp(textColor.a, targetAlpha, 2 * Time.deltaTime);
            
            name.color = textColor;

            if (item1 != null)
            {
                item1.color = new Color(1, 1, 1, textColor.a);
            }
            if (item2 != null)
            {
                item2.color = new Color(1, 1, 1, textColor.a);
            }

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

    private void ClearListData()
    {
        tradeInfos.Clear();
        tradeIDs.Clear();
        traderIDs.Clear();
        traderDNs.Clear();
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

    void OnError(PlayFabError e)
    {
        Debug.Log("Error : " + e.GenerateErrorReport());
    }
}
