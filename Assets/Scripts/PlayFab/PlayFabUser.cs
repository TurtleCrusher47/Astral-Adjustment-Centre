using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using CatalogItem = PlayFab.ClientModels.CatalogItem;

public class PlayFabUser : MonoBehaviour
{
    public PlayFabObjectPoolManager opManager;
    public static List<GameObject> itemsInInv = new List<GameObject>();

    [SerializeField] public string CurrencyCode = "GC";

    [SerializeField] public GameObject invItemPrefab;
    [SerializeField] public GridLayoutGroup invGroup;
    [SerializeField] public TMP_Text highscoreText, nameText, creditText;

    [SerializeField] private List<CatalogItem> catItems = new List<CatalogItem>();
    [SerializeField] private List<ItemInstance> invItems = new List<ItemInstance>();

    private float delay = 2;
    
    public void GetUserPage()
    {
        GetCatalog();

        GetHighScore();
        GetVirtualCurrency();
        nameText.text = PlayFabManager.currPlayFabDN;
    }
    
    void OnError(PlayFabError e)
    {
        Debug.Log(e.ErrorMessage);
    }

    public void GetHighScore()
    {
        var lbReq = new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = "HighScore",
            PlayFabId = PlayFabManager.currPlayFabID,
            MaxResultsCount = 1
        };

        PlayFabClientAPI.GetLeaderboardAroundPlayer(lbReq,
        result=>{
            highscoreText.text = result.Leaderboard[0].StatValue.ToString();
        }, OnError);
    }
    
    public void GetVirtualCurrency()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
        result=>{
            creditText.text = result.VirtualCurrency[CurrencyCode].ToString();
        }, OnError);
    }

    public void GetCatalog()
    {
        catItems.Clear();

        var catReq = new GetCatalogItemsRequest
        {
            CatalogVersion = "main"
        };

        PlayFabClientAPI.GetCatalogItems(catReq,
        result=>{
            catItems = result.Catalog;
            GetPlayerInventory();
        }, OnError);
    }

    public void GetPlayerInventory()
    {
        invItems.Clear();

        var invReq = new GetUserInventoryRequest
        {
            
        };

        PlayFabClientAPI.GetUserInventory(invReq,
        result=>{
            invItems = result.Inventory;
            SetUpInventory();
        }, OnError);
    }

    void SetUpInventory()
    {
        StopAllCoroutines();

        for (int i = 0; i < itemsInInv.Count; i++)
        {
            ObjectPoolManager.ReturnObjectToPool(itemsInInv[i]);
        }

        ResetAllItemCards();

        foreach (var item in invItems)
        {
            UpdateInventoryItem(item);
        }

        foreach (var item in itemsInInv)
        {
            CanvasGroup group = item.GetComponent<CanvasGroup>();

            delay += 0.25f;
            StartCoroutine(FadeInItem(group, delay));
        }
    }

    public void ClearInventory()
    {
        for (int i = 0; i < itemsInInv.Count; i++)
        {
            ObjectPoolManager.ReturnObjectToPool(itemsInInv[i]);
        }

        ResetAllItemCards();

        StopAllCoroutines();
    }

    private void ResetAllItemCards()
    {
        delay = 1;

        foreach (var item in itemsInInv)
        {
            ResetItemCard(item);
        }

        invGroup.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 450);

        itemsInInv.Clear();
    }

    private void ResetItemCard(GameObject itemObj)
    {
        CanvasGroup group = itemObj.GetComponent<CanvasGroup>();
        RawImage itemIcon = FindChildWithTag(itemObj, "ItemIcon").GetComponent<RawImage>();
        TMP_Text nameText = FindChildWithTag(itemObj, "ItemName").GetComponent<TMP_Text>();
        TMP_Text descText = FindChildWithTag(itemObj, "ItemDescription").GetComponent<TMP_Text>();
        TMP_Text ownedText = FindChildWithTag(itemObj, "ItemOwned").GetComponent<TMP_Text>();

        group.alpha = 0;
        itemIcon.texture = null;
        nameText.text = "";
        descText.text = "";
        ownedText.text = "";
    }

    private async void UpdateInventoryItem(ItemInstance item)
    {
        GameObject newItem = opManager.SpawnObject(invItemPrefab);
        itemsInInv.Add(newItem);

        newItem.transform.SetParent(invGroup.transform);
        newItem.transform.localScale = new Vector3(1, 1, 1);

        ResetItemCard(newItem);

        RawImage itemIcon = FindChildWithTag(newItem, "ItemIcon").GetComponent<RawImage>();
        TMP_Text nameText = FindChildWithTag(newItem, "ItemName").GetComponent<TMP_Text>();
        TMP_Text descText = FindChildWithTag(newItem, "ItemDescription").GetComponent<TMP_Text>();
        TMP_Text ownedText = FindChildWithTag(newItem, "ItemOwned").GetComponent<TMP_Text>();

        for (int i = 0; i < catItems.Count; i++)
        {
            if (catItems[i].ItemId == item.ItemId)
            {
                itemIcon.texture = await DownloadImage(catItems[i].ItemImageUrl);
                nameText.text = catItems[i].DisplayName;
                descText.text = catItems[i].Description;

                for (int j = 0; j < invItems.Count; j++)
                {
                    if (invItems[j].ItemId == item.ItemId)
                    {
                        ownedText.text = "Owned : " + invItems[j].RemainingUses.ToString();
                    }
                }

                if (ownedText.text == "Owned : ")
                {
                    ownedText.text = "Owned";
                }
            }
        }
    }

    private IEnumerator FadeInItem(CanvasGroup group, float delay)
    {
        yield return new WaitForSeconds(delay);

        Coroutine fadeIn = StartCoroutine(FadeIn(group));

        invGroup.gameObject.GetComponent<RectTransform>().sizeDelta += new Vector2(400, 0);

        yield return fadeIn;
    }

    private IEnumerator FadeIn(CanvasGroup group)
    {
        float targetAlpha = 1f;

        while (Mathf.Abs(group.alpha - targetAlpha) > 0)
        {
            group.alpha = Mathf.Lerp(group.alpha, targetAlpha, 2 * Time.deltaTime);

            yield return null;
        }
    }

    private GameObject FindChildWithTag(GameObject parent, string tag)
    {
        GameObject child = null;
        
        foreach(Transform transform in parent.transform)
        {
            if (transform.CompareTag(tag))
            {
                child = transform.gameObject;
                break;
            }
        }
 
        return child;
    }

    private async Task<Texture2D> DownloadImage(string MediaUrl)
    {
        try
        {
            var request = UnityWebRequestTexture.GetTexture(MediaUrl);
            request.SendWebRequest();

            while (!request.isDone) await Task.Yield();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("Error : " + request.error);
                return null;
            }
            
            return DownloadHandlerTexture.GetContent(request);
        }
        catch
        {

        }
        return null;
    }
}
