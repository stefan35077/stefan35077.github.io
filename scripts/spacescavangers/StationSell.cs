using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class StationSell : MonoBehaviour
{
    CollectionBeam C;

    [SerializeField] private Transform itemContainer;
    [SerializeField] private GameObject sellItemPrefab;

    [Header("Total balance display")]
    public TextMeshProUGUI totalBalanceDisplay;

    private void Start()
    {
        C = PlayerController.Instance.collectionBeam;
    }

    private void Update()
    {
        UpdateBalanceAmount();
    }

    public void ReloadItemList()
    {
        //get inventory
        int failCount = 0;
        while(!C)
        {
            C = PlayerController.Instance.collectionBeam;
            failCount++;

            if(failCount >= 10)
            {
                print("NO INVENTORY FOUND");
                return;
            }
        }

        //clear the list
        for(int i = 0; i < itemContainer.childCount; i++)
        {
            Destroy(itemContainer.GetChild(i).gameObject);
        }

        //setup the sell items
        foreach(InventoryItem item in C.inventory)
        {
            SellItem newSell = Instantiate(sellItemPrefab, itemContainer.position, itemContainer.rotation, itemContainer).GetComponent<SellItem>();
            newSell.gameObject.SetActive(true);
            int price = PriceGenerator(item.itemWeight, item.itemRarity);
            newSell.SetItem(item.itemIcon, item.itemName, item.itemWeight, item.itemRarity, price, item);
        }
    }

    int PriceGenerator(int weight, int rarity)
    {
        float rarityMulti = rarity;
        if(PlayerController.Instance.StationRarity > 0)
        {
            rarityMulti -= PlayerController.Instance.StationRarity;
        }
        if(rarityMulti < 0)
        {
            rarityMulti *= -1;
        }

        float output = (weight * GameManager.Instance.currencyInflation) * rarityMulti;
        if(Mathf.RoundToInt(output) <= 0)
        {
            print(rarityMulti);
            output = weight * GameManager.Instance.currencyInflation;
        }

        return Mathf.RoundToInt(output);
    }

    public void OpenSellMenu()
    {
        ReloadItemList();
    }
    public void CloseSellMenu()
    {
        
    }

    public void UpdateBalanceAmount()
    {
        totalBalanceDisplay.text = "₮" + GameManager.Instance.credit.ToString();
    }
}
