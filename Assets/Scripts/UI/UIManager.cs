using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, ITimeTracker
{
    public static UIManager Instance { get; private set; }

    [Header("Status Bar")]
    public Image toolEquipSlot;
    public Text  toolQuantityText;

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

    [Header("Screen Transition")]
    public GameObject fadeIn;
    public GameObject fadeOut;

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

    #region Fadein Fadeout Transitions

    public void FadeOutScreen()
    {
        fadeOut.SetActive(true);
    }
    public void FadeInScreen()
    {
        fadeIn.SetActive(true);
    }

    public void OnFadeInComplete()
    {
        //Disable the fade in screen
        fadeIn.SetActive(false);
    }

    //Reset fadein fadeout
    public void ResetFadeDefaults()
    {        
        fadeOut.SetActive(false);
        fadeIn.SetActive(true); 
    }

    #endregion

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

        ItemSlotData[] inventoryToolSlots = InventoryManager.Instance.GetInventorySlots(InventorySlot.InventoryType.Tool);
        ItemSlotData[] inventoryItemSlots = InventoryManager.Instance.GetInventorySlots(InventorySlot.InventoryType.Item);
        //Render the Tool section 
        RenderInvertoryPanel(inventoryToolSlots, toolSlots);
        RenderInvertoryPanel(inventoryItemSlots, itemSlots);

        toolHandSlot.Display(InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Tool));
        itemHandSlot.Display(InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Item));

        ItemData equippedTool = InventoryManager.Instance.GetEquippedSlotItem(InventorySlot.InventoryType.Tool);

        toolQuantityText.text = "";
        
        //Check if there is an item to display
        if (equippedTool != null)
        {
            //Switch the thumbnail to the item's thumbnail
            toolEquipSlot.sprite = equippedTool.thumbnail;

            toolEquipSlot.gameObject.SetActive(true);

            int quantity = InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Tool).quantity;
            if (quantity > 1)
            {
                toolQuantityText.text = quantity.ToString();
            }
            
            return;
        }

        toolEquipSlot.gameObject.SetActive(false);

    }

    void RenderInvertoryPanel(ItemSlotData[] slots, InventorySlot[] uiSlots)
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