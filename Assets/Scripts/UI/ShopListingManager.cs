using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopListingManager : ListingManager<ItemData>
{

    ItemData itemToBuy;
    int quantity;

    [Header("Confirmation Screen")]
    public GameObject confirmationScreen;
    public Text confirmationPrompt;
    public Image confirmationItemImage;
    public Text quantityText;
    public Text costCalculationText;
    public Button purchaseButton;

    [Header("Grid Settings")]
    public GridLayoutGroup gridLayoutGroup;


    /*
        public void RenderShop(List<ItemData> shopItems)
        {
            //Reset the grid to remove any previous listings
            if (listingGrid.childCount > 0)
            {
                foreach (Transform child in listingGrid)
                {
                    Destroy(child.gameObject);
                }
            }

            //Create a new listing for each item in the shop
            foreach (ItemData shopItem in shopItems)
            {
                GameObject listingGameObject = Instantiate(shopListing, listingGrid);
                listingGameObject.GetComponent<ShopListing>().Display(shopItem);
            }
        }
    */

    protected override void DisplayListing(ItemData listingItem, GameObject listingGameObject)
    {
        listingGameObject.GetComponent<ShopListing>().Display(listingItem);

    }

    protected override void OnRenderComplete()
    {
        AdjustGridHeight();
    }


    public void OpenConfirmationScreen(ItemData item)
    {
        itemToBuy = item;
        quantity = 1; // Default quantity
        RenderConfirmationScreen();
    }

    public void RenderConfirmationScreen()
    {
        confirmationScreen.SetActive(true);
        confirmationPrompt.text = $"Do you want to buy {itemToBuy.name}?";
        quantityText.text = $"x" + quantity;

        confirmationItemImage.sprite = itemToBuy.thumbnail;

        int cost = itemToBuy.cost * quantity;
        int playerMoneyLeft = PlayerStats.Money - cost;
        //if not enough money, show error message
        if (playerMoneyLeft < 0)
        {
            costCalculationText.text = $"Insufficient funds!";
            purchaseButton.interactable = false;
            return;
        }
        purchaseButton.interactable = true;
        costCalculationText.text = $"{PlayerStats.Money} > {playerMoneyLeft} ";
    }

    void AdjustGridHeight()
    {
        int itemCount = listingGrid.childCount;
        int rows = Mathf.CeilToInt(itemCount / (float)gridLayoutGroup.constraintCount);
        float heightPerItem = gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y;
        float totalHeight = rows * heightPerItem;

        RectTransform rt = gridLayoutGroup.GetComponent<RectTransform>();
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, totalHeight);
    }


    public void AddQuantity()
    {
        quantity++;
        RenderConfirmationScreen();
    }

    public void SubtractQuantity()
    {
        if (quantity > 1)
        {
            quantity--;
        }
        RenderConfirmationScreen();
    }

    //purchase
    public void ConfirmPurchase()
    {
        Shop.Purchase(itemToBuy, quantity);
        confirmationScreen.SetActive(false);
    }

    public void CancelPurchase()
    {
        confirmationScreen.SetActive(false);
    }

   
}
