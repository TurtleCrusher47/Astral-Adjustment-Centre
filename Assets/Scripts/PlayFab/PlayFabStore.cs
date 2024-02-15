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

public class PlayFabStore : MonoBehaviour
{
    public PlayFabObjectPoolManager opManager;
    public static List<GameObject> itemsInStore = new List<GameObject>();

    [SerializeField] public string CurrencyCode = "GC";

    [SerializeField] public GameObject storeItemPrefab;
    [SerializeField] public GridLayoutGroup storeGroup;
    [SerializeField] public TMP_Text creditText;

    [SerializeField] private List<CatalogItem> catItems = new List<CatalogItem>();
    [SerializeField] private List<StoreItem> storeItems = new List<StoreItem>();
    [SerializeField] private List<ItemInstance> invItems = new List<ItemInstance>();

    private float delay = 2;

    public void GetStorePage()
    {
        GetCatalog();
    }

    void OnError(PlayFabError e)
    {
        Debug.Log(e.ErrorMessage);
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
            GetStoreItems();
        }, OnError);
    }

    public void GetStoreItems()
    {
        storeItems.Clear();

        var storeReq = new GetStoreItemsRequest
        {
            CatalogVersion = "main",
            StoreId = "upgrades"
        };

        PlayFabClientAPI.GetStoreItems(storeReq,
        result=>{
            storeItems = result.Store;
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
            CheckStoreToInventory();
            GetVirtualCurrency();
        }, OnError);
    }

    private void CheckStoreToInventory()
    {
        try
        {
            for (int i = 0; i < storeItems.Count; i++)
            {
                for (int j = 0; j < invItems.Count; j++)
                {
                    if (storeItems[i].ItemId == invItems[j].ItemId)
                    {
                        if (storeItems[i].ItemId == "Ship_Speed" && invItems[j].ItemId == storeItems[i].ItemId)
                        {
                            storeItems.Remove(storeItems[i]);
                        }
                        else if (invItems[j].RemainingUses >= 5)
                        {
                            storeItems.Remove(storeItems[i]);
                        }
                    }
                }
            }
        }
        catch
        {

        }
        
        SetStore();
    }

    public void BuyItem(string itemID)
    {
        int itemPrice = 0;

        foreach (var item in catItems)
        {
            if (itemID == item.ItemId)
            {
                itemPrice = (int)item.VirtualCurrencyPrices[CurrencyCode];
            }
        }

        var buyReq = new PurchaseItemRequest
        {
            CatalogVersion = "main",
            StoreId = "upgrades",
            ItemId = itemID,
            VirtualCurrency = CurrencyCode,
            Price = itemPrice
        };

        PlayFabClientAPI.PurchaseItem(buyReq,
        result=>{
            Debug.Log("Purchased");
            GetPlayerInventory();
            GetVirtualCurrency();
        }, OnError);
    }

    public void GetVirtualCurrency()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
        result=>{
            creditText.text = "Credit : " + result.VirtualCurrency[CurrencyCode].ToString();
        }, OnError);
    }

    private void SetStore()
    {
        StopAllCoroutines();

        for (int i = 0; i < itemsInStore.Count; i++)
        {
            ObjectPoolManager.ReturnObjectToPool(itemsInStore[i]);
        }

        ResetAllItemCards();

        foreach (var item in storeItems)
        {
            UpdateStoreItem(item);
        }

        foreach (var item in itemsInStore)
        {
            CanvasGroup group = item.GetComponent<CanvasGroup>();
            Button buyButton = FindChildWithTag(item, "ItemBuy").GetComponent<Button>();

            delay += 0.25f;
            StartCoroutine(FadeInItem(group, buyButton, delay));
        }
    }

    public void ClearStore()
    {
        for (int i = 0; i < itemsInStore.Count; i++)
        {
            ObjectPoolManager.ReturnObjectToPool(itemsInStore[i]);
        }

        ResetAllItemCards();

        StopAllCoroutines();
    }

    private void ResetAllItemCards()
    {
        delay = 1;

        foreach (var item in itemsInStore)
        {
            ResetItemCard(item);
        }

        storeGroup.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 690);

        itemsInStore.Clear();
    }

    private void ResetItemCard(GameObject itemObj)
    {
        CanvasGroup group = itemObj.GetComponent<CanvasGroup>();
        RawImage itemIcon = FindChildWithTag(itemObj, "ItemIcon").GetComponent<RawImage>();
        TMP_Text nameText = FindChildWithTag(itemObj, "ItemName").GetComponent<TMP_Text>();
        TMP_Text descText = FindChildWithTag(itemObj, "ItemDescription").GetComponent<TMP_Text>();
        TMP_Text costText = FindChildWithTag(itemObj, "ItemCost").GetComponent<TMP_Text>();
        TMP_Text ownedText = FindChildWithTag(itemObj, "ItemOwned").GetComponent<TMP_Text>();
        Button buyButton = FindChildWithTag(itemObj, "ItemBuy").GetComponent<Button>();

        group.alpha = 0;
        itemIcon.texture = null;
        nameText.text = "";
        descText.text = "";
        costText.text = "";
        ownedText.text = "";
        buyButton.enabled = false;
        buyButton.onClick.RemoveAllListeners();
    }

    private async void UpdateStoreItem(StoreItem item)
    {
        GameObject newItem = opManager.SpawnObject(storeItemPrefab);
        itemsInStore.Add(newItem);

        newItem.transform.SetParent(storeGroup.transform);
        newItem.transform.localScale = new Vector3(1, 1, 1);

        ResetItemCard(newItem);

        RawImage itemIcon = FindChildWithTag(newItem, "ItemIcon").GetComponent<RawImage>();
        TMP_Text nameText = FindChildWithTag(newItem, "ItemName").GetComponent<TMP_Text>();
        TMP_Text descText = FindChildWithTag(newItem, "ItemDescription").GetComponent<TMP_Text>();
        TMP_Text costText = FindChildWithTag(newItem, "ItemCost").GetComponent<TMP_Text>();
        TMP_Text ownedText = FindChildWithTag(newItem, "ItemOwned").GetComponent<TMP_Text>();
        Button buyButton = FindChildWithTag(newItem, "ItemBuy").GetComponent<Button>();

        for (int i = 0; i < catItems.Count; i++)
        {
            if (catItems[i].ItemId == item.ItemId)
            {
                itemIcon.texture = await DownloadImage(catItems[i].ItemImageUrl);
                nameText.text = catItems[i].DisplayName;
                descText.text = catItems[i].Description;
                costText.text = catItems[i].VirtualCurrencyPrices[CurrencyCode].ToString() + " Credits";
                buyButton.onClick.AddListener(delegate { BuyItem(item.ItemId); });
                buyButton.onClick.AddListener(delegate { buyButton.enabled = false; });

                for (int j = 0; j < invItems.Count; j++)
                {
                    if (invItems[j].ItemId == item.ItemId)
                    {
                        ownedText.text = "Owned : " + invItems[j].RemainingUses.ToString();
                    }
                }

                if (ownedText.text == "")
                {
                    ownedText.text = "Not Owned";
                }
            }
        }
    }

    private IEnumerator FadeInItem(CanvasGroup group, Button buy, float delay)
    {
        yield return new WaitForSeconds(delay);

        Coroutine fadeIn = StartCoroutine(FadeIn(group));

        storeGroup.gameObject.GetComponent<RectTransform>().sizeDelta += new Vector2(450, 0);

        yield return new WaitForSeconds(0.5f);
        
        buy.enabled = true;

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
}
