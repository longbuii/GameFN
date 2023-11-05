using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [System.Serializable]
    public class ShopItem
    {
        public string itemName;
        public int itemPrice;
        public int availableQuantity;
        public GameObject itemPrefab; 
    }

    public List<ShopItem> shopItems;

    [Header("Shop Items")]
    private int selectedShopItemIndex = -1;

    [Header("Ui Items Section")]
    // Inventory system
    public GameObject ui_Window;
    public Image[] items_Images;
    public TextMeshProUGUI[] item_Price;

    [Header("Ui Items Description")]
    public GameObject ui_Description_Window;
    public Image description_Image;
    public TextMeshProUGUI description_Title;
    public TextMeshProUGUI description_Description;


    // Set to Open Shop for player
    public void OpenShop()
    {
        ui_Window.SetActive(true);

        for (int i = 0; i < items_Images.Length; i++)
        {
            if (i < shopItems.Count)
            {
                items_Images[i].sprite = shopItems[i].itemPrefab.GetComponent<SpriteRenderer>().sprite;
                int itemPrice = shopItems[i].itemPrice;
                item_Price[i].text = itemPrice.ToString();

                if (shopItems[i].availableQuantity > 0)
                {
                    if (FindObjectOfType<ScoreSystem>().scoreNum >= itemPrice)
                    {
                    }
                }
                else
                {
                    items_Images[i].gameObject.SetActive(false);
                    item_Price[i].gameObject.SetActive(false);

                }
            }
            else
            {
                items_Images[i].gameObject.SetActive(false);
                item_Price[i].gameObject.SetActive(false); 
            }
        }
    }

    // Set to Close Shop for player
    public void CloseShop()
    {
        ui_Window.SetActive(false);

        if (selectedShopItemIndex == -1 || shopItems[selectedShopItemIndex].itemPrice > FindObjectOfType<ScoreSystem>().scoreNum)
        {
            selectedShopItemIndex = -1; 
        }

        for (int i = 0; i < items_Images.Length; i++)
        {
            if (i < shopItems.Count)
            {
                int itemPrice = shopItems[i].itemPrice;
                if (FindObjectOfType<ScoreSystem>().scoreNum >= itemPrice)
                {
                    int index = i;
                    items_Images[i].gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
                    items_Images[i].gameObject.GetComponent<Button>().onClick.AddListener(() => PurchaseItem(index));
                }
            }
        }
    }
    // Set to Purchase Shop for player
    public void PurchaseItem(int itemIndex)
    {
        if (itemIndex >= 0 && itemIndex < shopItems.Count)
        {
            if (selectedShopItemIndex == -1)
            {
                selectedShopItemIndex = itemIndex;
                bool itemPurchased = Purchase(shopItems[selectedShopItemIndex]);
                if (itemPurchased)
                {
                    OpenShop();
                    selectedShopItemIndex = -1; 
                }
            }
        }
    }


    // Set to condition when player buy something of Shop
    public void ConfirmPurchase(int itemIndex)
    {
        bool itemPurchased = Purchase(shopItems[itemIndex]);
        if (itemPurchased)
        {
            OpenShop();
            selectedShopItemIndex = -1; 
        }
    }
    bool Purchase(ShopItem item)
    {
        ScoreSystem scoreSystem = FindObjectOfType<ScoreSystem>();
        if (scoreSystem.scoreNum >= item.itemPrice && item.availableQuantity > 0)
        {
            scoreSystem.TryPurchaseItem(item.itemPrice);
            item.availableQuantity--; 
            GameObject purchasedItem = Instantiate(item.itemPrefab);
            purchasedItem.name = item.itemName;
            FindObjectOfType<InventorySystem>().Pickup(purchasedItem);

            scoreSystem.UpdateScoreUI(); 

            Debug.Log("Purchased " + item.itemName + " for " + item.itemPrice + " coins.");

            return true;
        }

        else
        {
            Debug.Log("Purchase failed for " + item.itemName);
        }

        scoreSystem.UpdateScoreUI();
        return false;
    }

    // Show to description of Item
    public void showDescription(int _id)
    {
        description_Image.sprite = items_Images[_id].sprite;
        if (shopItems[_id].availableQuantity > 0)
        {
            description_Title.text = shopItems[_id].itemName;
        }
        else
        {
            description_Title.text = "Sold out";
        }
        description_Description.text = "Price: " + shopItems[_id].itemPrice;
        ui_Description_Window.SetActive(true);
    }
    public void hideDescription()
    {
        ui_Description_Window.SetActive(false);

    }

}
