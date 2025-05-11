using UnityEngine;

public class AnimEventsTools : MonoBehaviour
{
    public Transform handPoint;

    GameObject spawnedObject;

    //spawn the tool 
    public void SpawnTool()
    {
        //if there is already an instantiated object
        if (spawnedObject != null)
        {
            Destroy(spawnedObject);
        }
        //getthe equipment data
        EquipmentData equipmentData = InventoryManager.Instance.GetEquippedSlotItem(InventorySlot.InventoryType.Tool) as EquipmentData;
        if (equipmentData == null) return;

        //check if there is an item assigned
        if (equipmentData.gameModel != null)
        {
            //spawn object and cache
            spawnedObject = Instantiate(equipmentData.gameModel, handPoint);
        }

    }

    //despawn the tool gameobject once animation completes
    public void Despawn()
    {
        //if theres nothing to despawn so be it
        if (spawnedObject == null) return;
        //destroy the spawned object
        Destroy(spawnedObject);
    }

}
