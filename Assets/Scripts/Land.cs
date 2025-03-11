using UnityEngine;

public class Land : MonoBehaviour
{
    public enum LandStatus
    {
        Soil, Farmland, Watered
    }
    public LandStatus landStatus;

    public Material soilMat, farmlandMat, wateredMat;
    new Renderer renderer;
    public GameObject select;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        renderer = GetComponent<Renderer>();
        //default land
        SwitchLandStatus(LandStatus.Soil);
        Select(false);
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

}
