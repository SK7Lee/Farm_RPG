using UnityEngine;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        renderer = GetComponent<Renderer>();      
        SwitchLandStatus(LandStatus.Soil);        
        Select(false);
        TimeManager.Instance.RegisterTracker(this);
    }

    public void LoadLandData(LandStatus statusToSwitch, GameTimestamp lastWatered)
    {
        landStatus = statusToSwitch;
        timeWatered = lastWatered;

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
                break;

        }
        renderer.material = materialToSwitch;

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
        LandManager.Instance.OnLandStateChange(id, landStatus, timeWatered);

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
                    break;

            }
            return;
        }
        SeedData seedTool = toolSlot as SeedData;
        if (seedTool != null &&  landStatus != LandStatus.Soil && cropPlanted == null) 
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
