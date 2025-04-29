using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, ITimeTracker
{
    public static UIManager Instance { get; private set; }
    public CalendarUIListing calendar;
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

    [Header("Item info box")]
    public GameObject ItemInfoBox;
    public Text itemNameText;
    public Text itemDescriptionText;

    [Header("Screen Transition")]
    public GameObject fadeIn;
    public GameObject fadeOut;

    [Header ("Prompts") ]
    public YesNoPrompt yesNoPrompt;
    public NamingPrompt namingPrompt;
    [SerializeField] InteractBubble interactBubble;

    [Header("Player Stats")]
    public Text moneyText;

    [Header("Shop")]
    public ShopListingManager shopListingManager;

    [Header("Relationships")]
    public RelationshipListingManager relationshipListingManager;
    public AnimalListingManager animalRelationshipListingManager;

    [Header("Screen Management")]
    public GameObject menuScreen;
    public enum Tab
    {
        Inventory,
        Relationships,
        Animals
    }
    public Tab selectedTab;

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
        RenderPlayerStats();
        DisplayItemInfo(null);
        TimeManager.Instance.RegisterTracker(this);

    }

    #region Prompts
    public void TriggerNamingPrompt(string message, System.Action<string> onConfirmCallback)
    {
        //Check if the naming prompt is assigned
        if (namingPrompt.gameObject.activeSelf)
        {
            namingPrompt.QueuePromptAction(() => TriggerNamingPrompt(message, onConfirmCallback));
            return;
        }

        Debug.Log("Showing Naming UI");
        namingPrompt.gameObject.SetActive(true);
        namingPrompt.CreatePrompt(message, onConfirmCallback);
    }

    public void TriggerYesNoPrompt(string message, System.Action onYesCallback)
    {
        if (yesNoPrompt == null)
        {
            Debug.LogError("Yes/No prompt is not assigned!");
            return;
        }
        Debug.Log("Showing Yes/No UI");
        yesNoPrompt.gameObject.SetActive(true);
        yesNoPrompt.CreatePrompt(message, onYesCallback);
    }
    #endregion

    #region Tab Management
    public void ToggleMenuPanel()
    {
        menuScreen.SetActive(!menuScreen.activeSelf);
        OpenWindow(selectedTab);
        TabBehaviour.onTabStateChange?.Invoke();
    }
    public void OpenWindow(Tab windowToOpen)
    {
        relationshipListingManager.gameObject.SetActive(false);
        inventoryPanel.SetActive(false);
        animalRelationshipListingManager.gameObject.SetActive(false);
        switch (windowToOpen)
        {
            case Tab.Inventory:
                inventoryPanel.SetActive(true);
                RenderInventory();
                break;
            case Tab.Relationships:
                relationshipListingManager.gameObject.SetActive(true);
                relationshipListingManager.Render(RelationshipStats.relationships);
                break;
            case Tab.Animals:
                animalRelationshipListingManager.gameObject.SetActive(true);
                animalRelationshipListingManager.Render(AnimalStats.animalRelationships);
                break;
        }
        selectedTab = windowToOpen;
    }
    
    #endregion
    
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

    #region Inventory
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
            ItemInfoBox.SetActive(false);
            return;
        }
        ItemInfoBox.SetActive(true);
        itemNameText.text = data.name;
        itemDescriptionText.text = data.description;
    }
    #endregion

    #region Time
    public void ClockUpdate(GameTimestamp timestamp)
    {
        int hours = timestamp.hour;
        int minutes = timestamp.minute;

        string prefix = "AM ";

        if (hours >= 12)
        {
            prefix = "PM ";
            hours -= 12;
            Debug.Log(hours);
        }
        //special case for 12am/pm
        hours = hours == 0? 12 : hours;

        timeText.text = prefix + hours + ":" + minutes.ToString("00");

        //Handle the date
        int day = timestamp.day;
        string season = timestamp.season.ToString();
        string dayOfTheWeek = timestamp.GetDayOfTheWeek().ToString();

        dateText.text = season + " " + day + " (" + dayOfTheWeek + ")";

    }
    #endregion

    public void RenderPlayerStats()
    {
        moneyText.text = PlayerStats.Money + PlayerStats.CURRENCY;
    }

    public void OpenShop(List<ItemData> shopItems)
    {
        shopListingManager.gameObject.SetActive(true);
        shopListingManager.Render(shopItems);
    }

    public void ToggleRelationshipPanel()
    {
        GameObject panel = relationshipListingManager.gameObject;
        panel.SetActive(!panel.activeSelf);

        if (panel.activeSelf)
        {
            relationshipListingManager.Render(RelationshipStats.relationships);
        }
    }

    public void InteractPrompt(Transform item, string message, float offset)
    {
        interactBubble.gameObject.SetActive(true);
        interactBubble.transform.position = item.transform.position + new Vector3(0, offset , 0);
        interactBubble.Display(message);
    }

    public void DeactivateInteractPrompt()
    {
        interactBubble.gameObject.SetActive(false);
    }

}