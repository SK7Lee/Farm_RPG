using UnityEngine;

public class Land : MonoBehaviour, ITimeTracker
{
    public enum LandStatus
    {
        Soil, Farmland, Watered
    }
    public LandStatus landStatus;

    public Material soilMat, farmlandMat, wateredMat;
    new Renderer renderer;
    public GameObject select;
    
    GameTimestamp timeWatered;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        renderer = GetComponent<Renderer>();      
        SwitchLandStatus(LandStatus.Soil);        
        Select(false);
        TimeManager.Instance.RegisterTracker(this);
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

    }

    public void Select(bool toggle)
    {
        select.SetActive(toggle);
    }
    //While select land
    public void Interact()
    {
        ItemData toolSlot = InventoryManager.Instance.equippedTool;
        if (toolSlot == null)
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
                    SwitchLandStatus(LandStatus.Watered);
                    break;
            }
            return;
        }
    }

    public void ClockUpdate(GameTimestamp timestamp)
    {
        if (landStatus == LandStatus.Watered)
        {
            int hoursElapsed = GameTimestamp.CompareTimestamps(timeWatered, timestamp);
            Debug.Log("Hours Elapsed: " + hoursElapsed);

          
            if (hoursElapsed > 24)
            {
                SwitchLandStatus(LandStatus.Farmland);
            }
        }
    }


}
