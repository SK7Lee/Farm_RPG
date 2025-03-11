using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, ITimeTracker
{
    public static UIManager Instance { get; private set; }

    [Header("Status Bar")]
    public Image toolEquipSlot;

    public Text timeText;
    public Text dateText;

    [Header("Inventory System")]
    public GameObject inventoryPanel;

    public HandInventorySlot toolHandSlot;

    public HandInventorySlot itemHandSlot;

    public InventorySlot[] toolSlots;
    public InventorySlot[] itemSlots;

    //Item info box
    public Text itemNameText;
    public Text itemDescriptionText;

    private void Awake()
    {
        //If there is more than one instance of this class, destroy the new one
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            //Set the instance to this object
            Instance = this;
        }
    }

    private void Start()
    {
        //Render the inventory screen to reflect the current inventory
        RenderInventory();
        AssignSlotIndex();

        TimeManager.Instance.RegisterTracker(this);
    }

    public void AssignSlotIndex()
    {
        for (int i = 0; i < toolSlots.Length; i++)
        {
            toolSlots[i].AssignIndex(i);
            itemSlots[i].AssignIndex(i);
        }
    }


    // Render the inventory screen to reflect the current inventory
    public void RenderInventory()
    {
        //Get the inventory slots from the InventoryManager
        ItemData[] inventoryToolSlots = InventoryManager.Instance.tools;
        ItemData[] inventoryItemSlots = InventoryManager.Instance.items;

        //Render the Tool section 
        RenderInvertoryPanel(inventoryToolSlots, toolSlots);
        RenderInvertoryPanel(inventoryItemSlots, itemSlots);

        toolHandSlot.Display(InventoryManager.Instance.equippedTool);
        itemHandSlot.Display(InventoryManager.Instance.equippedItem);

        ItemData equippedTool = InventoryManager.Instance.equippedTool;
        //Check if there is an item to display
        if (equippedTool != null)
        {
            //Switch the thumbnail to the item's thumbnail
            toolEquipSlot.sprite = equippedTool.thumbnail;

            toolEquipSlot.gameObject.SetActive(true);
            return;
        }

        toolEquipSlot.gameObject.SetActive(false);

    }

    void RenderInvertoryPanel(ItemData[] slots, InventorySlot[] uiSlots)
    {
        for (int i = 0; i < uiSlots.Length; i++)
        {
            uiSlots[i].Display(slots[i]);
        }
    }

    public void ToggleInventoryPanel()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);

        RenderInventory();
    }

    public void DisplayItemInfo(ItemData data)
    {
        if (data == null)
        {
            itemNameText.text = "";
            itemDescriptionText.text = "";

            return;
        }

        itemNameText.text = data.name;
        itemDescriptionText.text = data.description;
    }

    public void ClockUpdate(GameTimestamp timestamp)
    {
        int hours = timestamp.hour;
        int minutes = timestamp.minute;

        string prefix = "AM ";

        if (hours > 12)
        {
            prefix = "PM ";
            hours -= 12;
        }

        timeText.text = prefix + hours + ":" + minutes.ToString("00");

        //Handle the date
        int day = timestamp.day;
        string season = timestamp.season.ToString();
        string dayOfTheWeek = timestamp.GetDayOfTheWeek().ToString();

        dateText.text = season + " " + day + " (" + dayOfTheWeek + ")";

    }

}