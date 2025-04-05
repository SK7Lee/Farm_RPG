using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ShopListingManager : MonoBehaviour
{
    public GameObject shopListing;
    public Transform listingGrid;
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
}
