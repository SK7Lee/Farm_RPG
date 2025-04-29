using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEditor.ShaderGraph;
public class GameStateManager : MonoBehaviour, ITimeTracker
{
    public static GameStateManager Instance { get; private set; }

    bool screenFadedOut;

    //to track interval updates;
    private int minutesElapsed = 0;

    //event triggered every 15mins
    public UnityEvent onIntervalUpdate;

    [SerializeField]
    GameBlackboard blackboard = new GameBlackboard();
    //this is blackboard will not be saved
    GameBlackboard sceneItemsBoard = new GameBlackboard();

    const string TIMESTAMP = "Timestamp";

    public GameBlackboard GetBlackboard()
    {
        return blackboard;
    }

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
        blackboard.SetValue(TIMESTAMP, timestamp);

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

    public GameObject PersistentInstantiate(GameObject gameObject, Vector3 position, Quaternion rotation)
    {
        GameObject item = Instantiate(gameObject, position, rotation);
        //save to blackcoard
        (GameObject, Vector3) itemInformation = (gameObject, position);        
        List<(GameObject, Vector3)> items = sceneItemsBoard.GetOrInitList<(GameObject, Vector3)>(SceneTransitionManager.Instance.currentLocation.ToString());
        items.Add(itemInformation);
        return item;
    }

    public void PersistentDestroy(GameObject item)
    {
        if (sceneItemsBoard.TryGetValue(SceneTransitionManager.Instance.currentLocation.ToString(), out List<(GameObject, Vector3)> items))
        {
            int index = items.FindIndex(i => i.Item2 == item.transform.position);
            items.RemoveAt(index);
        }
        Destroy(item);
    }

    void RenderPersistentObjects()
    {
        if (sceneItemsBoard.TryGetValue(SceneTransitionManager.Instance.currentLocation.ToString(),out List<(GameObject, Vector3)> items))
        {
            foreach ((GameObject, Vector3) item in items)
            {
                Instantiate(item.Item1, item.Item2, Quaternion.identity);
            }
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

        //Time
        GameTimestamp timestamp = TimeManager.Instance.GetGameTimestamp();

        //return new GameSaveState(landData, cropData, toolSlots, itemSlots, equippedItemSlot, equippedToolSlot, timestamp,PlayerStats.Money, RelationshipStats.relationships, AnimalStats.animalRelationships, IncubationManager.eggsIncubating);
        

        
        //Retrieve Farm Data
        FarmSaveState farmSaveState = FarmSaveState.Export();
        
        //Retrieve Inventory Data
        InventorySaveState inventorySaveState = InventorySaveState.Export();

        PlayerSaveState playerSaveState = PlayerSaveState.Export();

        RelationshipSaveState relationshipSaveState = RelationshipSaveState.Export();

        return new GameSaveState(blackboard, farmSaveState, inventorySaveState, timestamp, playerSaveState);

    }

    public void LoadSave()
    {

        
        GameSaveState save = SaveManager.Load();

        blackboard = save.blackboard;
        blackboard.Debug();
        //Time
        TimeManager.Instance.LoadTime(save.timestamp);
   
        save.inventorySaveState.LoadData();


        //Farm Data
        //LandManager.farmData = new System.Tuple<List<LandSaveState>, List<CropSaveState>>(save.landData, save.cropData);
        save.farmSaveState.LoadData();

        //Currency
        //PlayerStats.LoadStats(save.money);
        save.playerSaveState.LoadData();
        RelationshipStats.LoadStats();
        AnimalStats.LoadStats();

    }
}
