using UnityEngine;
using UnityEngine.Rendering;

public class Land : MonoBehaviour, ITimeTracker
{
    public int id;
    public enum LandStatus
    {
        Soil, Farmland, Watered
    }
    public LandStatus landStatus;

    public Material soilMat, farmlandMat, wateredMat;
    new Renderer renderer;
    public GameObject select;
    
    GameTimestamp timeWatered;

    [Header("Crops")]
    public GameObject cropPrefab;

    CropBehaviour cropPlanted = null;


    //Obstacles
    public enum FarmObstacleStatus { None, Rock, Wood, Weeds}
    [Header("Obstacles")]
    public FarmObstacleStatus obstacleStatus;
    public GameObject rockPrefab, woodPrefab, weedsPrefab;
    GameObject obstacleObject;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        renderer = GetComponent<Renderer>();      
        SwitchLandStatus(LandStatus.Soil);        
        Select(false);
        TimeManager.Instance.RegisterTracker(this);
    }

    public void LoadLandData(LandStatus landStatusToSwitch, GameTimestamp lastWatered, Land.FarmObstacleStatus obstacleStatusToSwitch)
    {
        landStatus = landStatusToSwitch;
        timeWatered = lastWatered;

        Material materialToSwitch = soilMat;
        switch (landStatusToSwitch)
        {
            case LandStatus.Soil:
                materialToSwitch = soilMat;
                break;
            case LandStatus.Farmland:
                materialToSwitch = farmlandMat;
                break;
            case LandStatus.Watered:
                materialToSwitch = wateredMat;
                break;

        }
        renderer.material = materialToSwitch;

        switch (obstacleStatusToSwitch)
        {
            case FarmObstacleStatus.None:
                if (obstacleObject != null) Destroy(obstacleObject);
                break;
            case FarmObstacleStatus.Rock:
                obstacleObject = Instantiate(rockPrefab, transform);
                break;
            case FarmObstacleStatus.Wood:
                obstacleObject = Instantiate(woodPrefab, transform);
                break;
            case FarmObstacleStatus.Weeds:
                obstacleObject = Instantiate(weedsPrefab, transform);
                break;
        }

        if (obstacleObject != null) obstacleObject.transform.position = new Vector3(transform.position.x, 0.76f, transform.position.z);
        obstacleStatus = obstacleStatusToSwitch;
    }
    public void SwitchLandStatus(LandStatus statusToSwitch)
    {
        landStatus = statusToSwitch;
        Material materialToSwitch = soilMat;
        switch (statusToSwitch)
        {
            case LandStatus.Soil:
                materialToSwitch = soilMat;
                break;
            case LandStatus.Farmland:
                materialToSwitch = farmlandMat;
                break;
            case LandStatus.Watered:
                materialToSwitch = wateredMat;
                timeWatered = TimeManager.Instance.GetGameTimestamp();
                break;

        }

        renderer.material = materialToSwitch;
        LandManager.Instance.OnLandStateChange(id, landStatus, timeWatered, obstacleStatus);

    }

    public void SetObstacleStatus(FarmObstacleStatus statusToSwitch)
    {
        switch (statusToSwitch)
        {
            case FarmObstacleStatus.None:
                if (obstacleObject!=null)  Destroy(obstacleObject);
                break;
            case FarmObstacleStatus.Rock:
                obstacleObject = Instantiate(rockPrefab, transform);
                break;
            case FarmObstacleStatus.Wood:
                obstacleObject = Instantiate(woodPrefab, transform);
                break;
            case FarmObstacleStatus.Weeds:
                obstacleObject = Instantiate(weedsPrefab, transform);
                break;
        }

        if (obstacleObject != null) obstacleObject.transform.position = new Vector3(transform.position.x,0.76f,transform.position.z);    
        obstacleStatus = statusToSwitch;
        LandManager.Instance.OnLandStateChange(id, landStatus, timeWatered, obstacleStatus);

    }

    public void Select(bool toggle)
    {
        select.SetActive(toggle);
    }
    //While select land
    public void Interact()
    {
        ItemData toolSlot = InventoryManager.Instance.GetEquippedSlotItem(InventorySlot.InventoryType.Tool);
       
        if (!InventoryManager.Instance.SlotEquipped(InventorySlot.InventoryType.Tool))
        {
            return;
        }

        EquipmentData equipmentTool = toolSlot as EquipmentData;
        if (equipmentTool != null)
        {
            EquipmentData.ToolType toolType = equipmentTool.toolType;
            switch (toolType)
            {
                case EquipmentData.ToolType.Hoe:
                    //Switch the land status to farmland
                    SwitchLandStatus(LandStatus.Farmland);
                    break;
                case EquipmentData.ToolType.WateringCan:
                    //Switch the land status to watered
                    if (landStatus != LandStatus.Soil)
                    {
                        SwitchLandStatus(LandStatus.Watered);
                    }
                    break;
                case EquipmentData.ToolType.Shovel:
                    if (cropPlanted != null)
                    {
                        cropPlanted.RemoveCrop();
                    }
                    if (obstacleStatus == FarmObstacleStatus.Weeds) SetObstacleStatus(FarmObstacleStatus.None);
                    break;
                case EquipmentData.ToolType.Axe:
                    if (obstacleStatus == FarmObstacleStatus.Rock) SetObstacleStatus(FarmObstacleStatus.None);
                    break;
                case EquipmentData.ToolType.Pickaxe:
                    if (obstacleStatus == FarmObstacleStatus.Rock) SetObstacleStatus(FarmObstacleStatus.None);
                    break;
            }
            return;
        }
        SeedData seedTool = toolSlot as SeedData;
        if (seedTool != null &&  landStatus != LandStatus.Soil && cropPlanted == null && obstacleStatus == FarmObstacleStatus.None) 
        {
            SpawnCrop();
            cropPlanted.Plant(id,seedTool);
            InventoryManager.Instance.ConsumeItem(InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Tool));
        }
    }

    public CropBehaviour SpawnCrop()
    {
        GameObject cropObject = Instantiate(cropPrefab, transform);
        cropObject.transform.position = new Vector3(transform.position.x, 0.76f, transform.position.z);
        cropPlanted = cropObject.GetComponent<CropBehaviour>();
        return cropPlanted;
    }

    public void ClockUpdate(GameTimestamp timestamp)
    {
        //when raining, have it watered if it is farmland
        if (WeatherManager.Instance.WeatherToday == WeatherData.WeatherType.Rain && landStatus == LandStatus.Farmland)
        {
            SwitchLandStatus(LandStatus.Watered);
        }

        //check if 24hours since last watered
        if (landStatus == LandStatus.Watered)
        {
            int hoursElapsed = GameTimestamp.CompareTimestamps(timeWatered, timestamp);
            Debug.Log("Hours Elapsed: " + hoursElapsed);

            if (cropPlanted != null) 
            {
                cropPlanted.Grow();
            }
          
            if (hoursElapsed > 24)
            {
                SwitchLandStatus(LandStatus.Farmland);
            }
        }
        if (landStatus != LandStatus.Watered && cropPlanted != null)
        {
            if (cropPlanted.cropState != CropBehaviour.CropState.Seed)
            {
                cropPlanted.Wither();
            }
        }
    }

    private void OnDestroy()
    {
        TimeManager.Instance.UnregisterTracker(this);
    }

}
