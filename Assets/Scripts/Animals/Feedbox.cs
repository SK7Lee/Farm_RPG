using UnityEngine;

public class Feedbox : InteractableObject
{
    bool containsFeed;
    public GameObject displayFeed;
    public int id;

    public override void Pickup()
    {
        if (CanFeed())
        {
            //Feed the animal
            FeedAnimal();   
        }
    }

    void FeedAnimal()
    {
        //Consume the item in the players hand
        InventoryManager.Instance.ConsumeItem(InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Item));
        SetFeedState(true);
    }

    public void SetFeedState(bool feed)
    {
        containsFeed = feed;
        //Render the food in the feedbox
        displayFeed.SetActive(feed);
    }

    //Check if the player is in range of the feedbox
    bool CanFeed()
    {
        //Get the item data player
        ItemData handSlotItem = InventoryManager.Instance.GetEquippedSlotItem(InventorySlot.InventoryType.Item);
        //if the player is not holding a feed item
        if (handSlotItem == null || containsFeed)
        {
            return false;
        }
        //make sure you set the item to the feed item
        if (handSlotItem.name != item.name)
        {
            return false;
        }
        return true;

    }
}
