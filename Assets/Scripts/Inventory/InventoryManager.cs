using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static InventorySlot;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    
    List<ItemData> _itemIndex;

    public ItemData GetItemFromString(string name)
    {
        if (_itemIndex == null)
        {
            _itemIndex = Resources.LoadAll<ItemData>("").ToList();
        }
        return _itemIndex.Find( i => i.name == name);
    }

    [Header("Tools")]
    [SerializeField] 
    private ItemSlotData[] toolSlots = new ItemSlotData[8];
    [SerializeField] 
    private ItemSlotData equippedToolSlot = null;
    
    [Header("Items")]
    [SerializeField] 
    private ItemSlotData[] itemSlots = new ItemSlotData[8];
    [SerializeField] 
    private ItemSlotData equippedItemSlot = null;
    public Transform handPoint;

    public void LoadInventory(ItemSlotData[] toolSlots, ItemSlotData equippedToolSlot, ItemSlotData[] itemSlots, ItemSlotData equippedItemSlot)
    {
        this.toolSlots = toolSlots;
        this.equippedToolSlot = equippedToolSlot;
        this.itemSlots = itemSlots;
        this.equippedItemSlot = equippedItemSlot;
        UIManager.Instance.RenderInventory();
    }

    public void InventoryToHand(int slotIndex, InventorySlot.InventoryType inventoryType)
    {
        ItemSlotData handToEquip = equippedToolSlot;
        ItemSlotData[] inventoryToAlter = toolSlots;

        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            handToEquip = equippedItemSlot;
            inventoryToAlter = itemSlots;
        }

        if (handToEquip.Stackable(inventoryToAlter[slotIndex]))
        {
            ItemSlotData slotToAlter = inventoryToAlter[slotIndex];
            handToEquip.AddQuanity(slotToAlter.quantity);
            slotToAlter.Empty();
        }
        else
        {
            ItemSlotData slotToEquip = new ItemSlotData(inventoryToAlter[slotIndex]);        
            inventoryToAlter[slotIndex] =new ItemSlotData( handToEquip);

            if (slotToEquip.IsEmpty())
            {
                handToEquip.Empty();
            }
            else 
            { 
                EquipHandSlot(slotToEquip);
            }
        }

        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            RenderHand();
        }
        UIManager.Instance.RenderInventory();
        RenderHand();

        /*
        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            ItemData itemToEquip = itemSlots[slotIndex];
            itemSlots[slotIndex] = equippedItemSlot;
            equippedItemSlot = itemToEquip;
            RenderHand();
        }
        else
        {
            ItemData toolToEquip = toolSlots[slotIndex];
            toolSlots[slotIndex] = equippedToolSlot;
            equippedToolSlot = toolToEquip;
        }

        UIManager.Instance.RenderInventory();
        */
    }

    public void HandToInventory(InventorySlot.InventoryType inventoryType)
    {
        ItemSlotData handSlot = equippedToolSlot;
        ItemSlotData[] inventoryToAlter = toolSlots;

        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            handSlot = equippedItemSlot;
            inventoryToAlter = itemSlots;
        }

        if (!StackItemToInventory(handSlot, inventoryToAlter))
        {
            for (int i = 0; i < inventoryToAlter.Length; i++)
            {
                if (inventoryToAlter[i].IsEmpty())
                {
                    inventoryToAlter[i] = new ItemSlotData(handSlot);
                    handSlot.Empty();
                    break;
                }
            }
        }       

        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            RenderHand();
        }
        UIManager.Instance.RenderInventory();
        /*
        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            for (int i = 0; i < itemSlots.Length; i++)
            {
                if (itemSlots[i] == null)
                {
                    itemSlots[i] = equippedItemSlot;
                    equippedItemSlot = null;
                    break;
                }
            }
            RenderHand();
        }
        else
        {
            for (int i = 0; i < toolSlots.Length; i++)
            {
                if (toolSlots[i] == null)
                {
                    toolSlots[i] = equippedToolSlot;
                    equippedToolSlot = null;
                    break;
                }
            }

        }
        UIManager.Instance.RenderInventory();
        */
    }

    public bool StackItemToInventory(ItemSlotData itemSlot, ItemSlotData[] inventoryArray)
    {
        for (int i = 0; i < inventoryArray.Length; i++)
        {
            if (inventoryArray[i].Stackable(itemSlot))
            {
                inventoryArray[i].AddQuanity(itemSlot.quantity);
                itemSlot.Empty();
                return true;
            }
        }
        return false;
    }

    public void ShopToInventory(ItemSlotData itemSlotToMove)
    {
        ItemSlotData[] inventoryToAlter = IsTool(itemSlotToMove.itemData) ? toolSlots : itemSlots;

        if (!StackItemToInventory(itemSlotToMove, inventoryToAlter))
        {
            for (int i = 0; i < inventoryToAlter.Length; i++)
            {
                if (inventoryToAlter[i].IsEmpty())
                {
                    inventoryToAlter[i] = new ItemSlotData(itemSlotToMove);
                    break;
                }
            }
        }      
        UIManager.Instance.RenderInventory();
        RenderHand();
    }

    public void RenderHand()
    {
        if (handPoint.childCount > 0)
        {
            Destroy(handPoint.GetChild(0).gameObject);
        }
        if (SlotEquipped(InventorySlot.InventoryType.Item))
        {
            Instantiate(GetEquippedSlotItem(InventorySlot.InventoryType.Item).gameModel, handPoint);
        }
    }

    #region Gets and Checks
    public ItemData GetEquippedSlotItem(InventorySlot.InventoryType inventoryType) 
    { 
        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            return equippedItemSlot.itemData;
        }
        return equippedToolSlot.itemData;
    }

    public ItemSlotData GetEquippedSlot(InventorySlot.InventoryType inventoryType)
    {
        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            return equippedItemSlot;
        }
        return equippedToolSlot;
    }

    public ItemSlotData[] GetInventorySlots(InventorySlot.InventoryType inventoryType)
    {
        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            return itemSlots;
        }
        return toolSlots;
    }

    public bool SlotEquipped(InventorySlot.InventoryType inventoryType)
    {
        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            return !equippedItemSlot.IsEmpty();
        }
        return !equippedToolSlot.IsEmpty();
    }

    public bool IsTool(ItemData item)
    {
        EquipmentData equipment = item as EquipmentData;
        if (equipment != null)
        {
            return true;
        }

        SeedData seed = item as SeedData;
        return seed != null;
    }

    #endregion

    public void EquipHandSlot(ItemData item)
    {
       if (IsTool(item))
        {
            equippedToolSlot = new ItemSlotData(item);
        }
        else
        {
            equippedItemSlot = new ItemSlotData(item);
        }
    }
    
    public void EquipHandSlot(ItemSlotData itemSlot)
    {
        ItemData item = itemSlot.itemData;
       if (IsTool(item))
        {
            equippedToolSlot = new ItemSlotData(itemSlot);
        }
        else
        {
            equippedItemSlot = new ItemSlotData(itemSlot);
        }
    }

    public void ConsumeItem(ItemSlotData itemSlot)
    {
        if(itemSlot.IsEmpty())
        {
            Debug.LogError("Nothing to consume!");
           return;
        }
        itemSlot.Remove();
        RenderHand();
        UIManager.Instance.RenderInventory();
    }
    
    #region Inventory Slot validation
    private void OnValidate()
    {
        ValidateInventorySlot(equippedToolSlot);
        ValidateInventorySlot(equippedItemSlot);
        ValidateInventorySlots(itemSlots);
        ValidateInventorySlots(toolSlots);
    }


    void ValidateInventorySlot(ItemSlotData slot)
    {
        if (slot.itemData != null && slot.quantity == 0 ) 
        { 
            slot.quantity = 1;
        }
    }

    void ValidateInventorySlots(ItemSlotData[] array)
    {
        foreach (ItemSlotData slot in array)
        {
            ValidateInventorySlot(slot);
        }
    }
    #endregion

}
