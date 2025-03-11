using UnityEngine;
using UnityEngine.UI;   
using UnityEngine.EventSystems;   


public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    ItemData itemToDisplay;

    public Image itemDisplayImage;

    //Type of inventory slot
    public enum InventoryType
    {
        Tool,
        Item
    }

    int slotIndex;

    public InventoryType inventoryType;
    public void Display(ItemData itemToDisplay)
    {
        if (itemToDisplay != null) 
        {
            itemDisplayImage.sprite = itemToDisplay.thumbnail;
            this.itemToDisplay = itemToDisplay;
            itemDisplayImage.gameObject.SetActive(true);
            return;
        }

        itemDisplayImage.gameObject.SetActive(false);
    }

    //Assign the index of the slot
    public void AssignIndex(int slotIndex)
    {
        this.slotIndex = slotIndex;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Instance.DisplayItemInfo(itemToDisplay);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.DisplayItemInfo(null);
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        InventoryManager.Instance.InventoryToHand(slotIndex, inventoryType);
        Debug.Log("Clicked on " + itemToDisplay.name);
    }

}
