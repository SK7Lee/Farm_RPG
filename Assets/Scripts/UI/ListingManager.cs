using System.Collections.Generic;
using UnityEngine;

public abstract class ListingManager<T> : MonoBehaviour
{
    public GameObject listingEntryPrefab;
    public Transform listingGrid;
    public void Render(List<T> listingItems)
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
        foreach (T listingItem in listingItems)
        {
            GameObject listingGameObject = Instantiate(listingEntryPrefab, listingGrid);
            DisplayListing(listingItem,listingGameObject);
        }
    }

    protected abstract void DisplayListing(T listingItem,GameObject listingGameObject);
}
