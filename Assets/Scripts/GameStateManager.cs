using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
public class GameStateManager : MonoBehaviour, ITimeTracker
{
    public static GameStateManager Instance { get; private set; }

    bool screenFadedOut;

    //to track interval updates;
    private int minutesElapsed = 0;

    //event triggered every 15mins
    public UnityEvent onIntervalUpdate;

    private void Awake()
    {
       if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        TimeManager.Instance.RegisterTracker(this);
    }

    void OnDayReset()
    {
        Debug.Log("Day Reset");
        foreach(NPCRelationshipState npc in RelationshipStats.relationships)
        {
            npc.hasTalkedToday = false;
            npc.giftGivenToday = false;
        }
        AnimalFeedManager.ResetFeedboxes();
        AnimalStats.OnDayReset();
    }

    public void ClockUpdate(GameTimestamp timestamp)
    {
        UpdateShippingState(timestamp);
        UpdateFarmState(timestamp);
        IncubationManager.UpdateEggs();
        if (timestamp.hour == 0 && timestamp.minute == 0)
        {
            OnDayReset();
        }
        //call events 15mins
        if (minutesElapsed >= 15)
        {
            minutesElapsed = 0;
            onIntervalUpdate?.Invoke();
        }
        else
        {
            minutesElapsed++;
        }
    }

    void UpdateShippingState(GameTimestamp timestamp) 
    { 
        if (timestamp.hour == ShippingBin.hourToShip && timestamp.minute == 0)
        {
            ShippingBin.ShipItems();
        }
    }
    void UpdateFarmState(GameTimestamp timestamp)
    {
        if (SceneTransitionManager.Instance.currentLocation != SceneTransitionManager.Location.Farm)
        {
            if (LandManager.farmData == null) return;
            List<LandSaveState> landData = LandManager.farmData.Item1;
            List<CropSaveState> cropData = LandManager.farmData.Item2;

            if (cropData.Count == 0) return;

            for (int i = 0; i < cropData.Count; i++)
            {
                CropSaveState crop = cropData[i];
                LandSaveState land = landData[crop.landID];

                if (crop.cropState == CropBehaviour.CropState.Wilted) continue;

                land.ClockUpdate(timestamp);
                if (land.landStatus == Land.LandStatus.Watered)
                {
                    crop.Grow();
                }
                else if (crop.cropState != CropBehaviour.CropState.Seed)
                {
                    crop.Wither();
                }

                cropData[i] = crop;
                landData[crop.landID] = land;

            }

            //LandManager.farmData.Item2.ForEach((CropSaveState crop) => {
            //    Debug.Log(crop.seedToGrow + "\n Health: " + crop.health + "\n Growth: " + crop.growth + "\n State: " + crop.cropState.ToString());
            //});

        }
    }

    public void Sleep()
    {
        UIManager.Instance.FadeOutScreen();
        screenFadedOut = false;
        StartCoroutine(TransitionTime());
    }

    IEnumerator TransitionTime()
    {
        GameTimestamp timestampOfNextDay = TimeManager.Instance.GetGameTimestamp();
        timestampOfNextDay.day += 1;
        timestampOfNextDay.hour = 6;
        timestampOfNextDay.minute = 0;
        Debug.Log("Sleeping to: " + timestampOfNextDay.day + " " + timestampOfNextDay.hour + ":" + timestampOfNextDay.minute);

        while (!screenFadedOut) 
        {
            yield return new WaitForSeconds(1f);
        }
        TimeManager.Instance.SkipTime(timestampOfNextDay);
        //Save file
        SaveManager.Save(ExportSaveState());
        screenFadedOut = false;
        UIManager.Instance.ResetFadeDefaults();
    }

    public void OnFadeOutComplete()
    {
        //Disable the fade out screen
        screenFadedOut = true;
    }

    public GameSaveState ExportSaveState()
    {
        
        List<LandSaveState> landData = new List<LandSaveState>();
        List<CropSaveState> cropData = new List<CropSaveState>();
        //Retrieve Farm Data
        if (LandManager.farmData != null) 
        {
            landData = LandManager.farmData.Item1;
            cropData = LandManager.farmData.Item2;    
        }


        //Retrieve Inventory Data
        ItemSlotData[] toolSlots = InventoryManager.Instance.GetInventorySlots(InventorySlot.InventoryType.Tool);
        ItemSlotData[] itemSlots = InventoryManager.Instance.GetInventorySlots(InventorySlot.InventoryType.Item);
        ItemSlotData equippedToolSlot = InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Tool);
        ItemSlotData equippedItemSlot = InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Item);
        //Time
        GameTimestamp timestamp = TimeManager.Instance.GetGameTimestamp();

        return new GameSaveState(landData, cropData, toolSlots, itemSlots, equippedItemSlot, equippedToolSlot, timestamp,PlayerStats.Money, RelationshipStats.relationships, AnimalStats.animalRelationships, IncubationManager.eggsIncubating);
        /*
        //Retrieve Farm Data
        FarmSaveState farmSaveState = FarmSaveState.Export();
        //Retrieve Inventory Data
        InventorySaveState inventorySaveState = InventorySaveState.Export();
        //Time
        GameTimestamp timestamp = TimeManager.Instance.GetGameTimestamp();
        //Player Data
        PlayerSaveState playerSaveState = PlayerSaveState.Export();
        //Relationships
        RelationshipSaveState relationshipSaveState = RelationshipSaveState.Export();
        return new GameSaveState(farmSaveState, inventorySaveState, timestamp, playerSaveState, relationshipSaveState);
        */
    }
 
    public void LoadSave()
    {

        
        GameSaveState save = SaveManager.Load();
        //Time
        TimeManager.Instance.LoadTime(save.timestamp);
        //Inventory
        ItemSlotData[] toolSlots = ItemSlotData.DeserializeArray(save.toolSlots);
        ItemSlotData equippedToolSlot = ItemSlotData.DeserializeData(save.equippedToolSlot);
        ItemSlotData[] itemSlots = ItemSlotData.DeserializeArray(save.itemSlots);
        ItemSlotData equippedItemSlot = ItemSlotData.DeserializeData(save.equippedItemSlot);
        InventoryManager.Instance.LoadInventory(toolSlots, equippedToolSlot, itemSlots, equippedItemSlot);
        //Farm Data
        LandManager.farmData = new System.Tuple<List<LandSaveState>, List<CropSaveState>>(save.landData, save.cropData);
        //Currency
        PlayerStats.LoadStats(save.money);
        //Relationships
        RelationshipStats.LoadStats(save.relationships);
        AnimalStats.LoadStats(save.animals);
        //Animals
        IncubationManager.eggsIncubating = save.eggsIncubating;
        
        /*
        GameSaveState save = SaveManager.Load();
        TimeManager.Instance.LoadTime(save.timestamp);
        //Inventory
        save.inventorySaveState.LoadData();
        //Retrieve Farm Data
        save.farmSaveState.LoadData();
        //Player Data
        save.playerSaveState.LoadData();
        //Relationships
        save.relationshipSaveState.LoadData();
        */
    }
}
