using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ShippingBin : InteractableObject
{
    public static int hourToShip = 18;
    public static List<ItemSlotData> itemsToShip = new List<ItemSlotData>();

    const string ITEMS_SHIPPED_KEY = "ItemsShipped";
    const string TOTAL_SHIPPED = "TotalItemsShipped";

    public override void Pickup()
    {
        ItemData handSlotItem = InventoryManager.Instance.GetEquippedSlotItem(InventorySlot.InventoryType.Item);
        if (handSlotItem == null) return;
        UIManager.Instance.TriggerYesNoPrompt("Do you want to sell " + handSlotItem.name + " ?", PlaceItemInShippingBin);
    }
    void PlaceItemInShippingBin()
    {
        ItemSlotData handSlot = InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Item);
        itemsToShip.Add(new ItemSlotData(handSlot));
        handSlot.Empty();
        InventoryManager.Instance.RenderHand();
        foreach (ItemSlotData item in itemsToShip)
        {
            Debug.Log($"{item.itemData.name} x {item.quantity}");
        }
    }

    public static void ShipItems()
    {
        int moneyToReceive = TallyItems(itemsToShip);
        PlayerStats.Earn(moneyToReceive);
        itemsToShip.Clear();
    }

    static int TallyItems(List<ItemSlotData> items)
    {
        GameBlackboard blackboard = GameStateManager.Instance.GetBlackboard();
        List<(string,int)> itemsShipped = blackboard.GetOrInitList<(string,int)> (ITEMS_SHIPPED_KEY);
        int total = 0;
        foreach (ItemSlotData item in items)
        {
            //update the blackboard with new data
            (string, int) entry = itemsShipped.Find(x => x.Item1 == item.itemData.name);
            entry.Item2 += item.quantity;
            blackboard.IncreaseValue(TOTAL_SHIPPED, item.quantity);

            //get the item quantity and multiply by the cost value
            total += item.quantity * item.itemData.cost;
        }
        return total;
    }
}
