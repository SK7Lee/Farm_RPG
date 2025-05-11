using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerToolController : MonoBehaviour
{
    //whether the player is in the middle of a tool usage operation
    bool lockedIn = false;
    //the land the operation will be performed in
    Land selectedLand = null;

    Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void StartToolUse(Land selectedLand)
    {
        //the player shouldn't be able to use his tool when he has his hands full with an item
        if (InventoryManager.Instance.SlotEquipped(InventorySlot.InventoryType.Item))
        {
            return;
        }
        //detecting what is the current tool that the player is holding
        ItemData toolSlot = InventoryManager.Instance.GetEquippedSlotItem(InventorySlot.InventoryType.Tool);
        
        if(toolSlot == null) { return; }
        if (lockedIn) return;
        //acquire the lock and lock in the selected land
        lockedIn = true;
        this.selectedLand = selectedLand;

        //we dont have animations for planting seeds for now we can complete it
        if (toolSlot is SeedData)
        {
            CompleteInteraction();
            return;
        }
        
        EquipmentData equipmentTool = toolSlot as EquipmentData;

        //play animation
        EquipmentData.ToolType toolType = equipmentTool.toolType;
        //consume stamina
        PlayerStats.UseStamina(2);
        switch (toolType)
        {
            case EquipmentData.ToolType.WateringCan:
                animator.SetTrigger("Watering");
                SFXManager.instance?.PlayPlant();
                selectedLand.Interact();
                selectedLand = null;
                lockedIn = false;
                return;
            case EquipmentData.ToolType.Hoe:
                animator.SetTrigger("Plowing");
                SFXManager.instance?.PlayDig();
                selectedLand.Interact();
                selectedLand = null;
                lockedIn = false;
                return;
            case EquipmentData.ToolType.Axe:
                animator.SetTrigger("Axe");
                SFXManager.instance?.PlayChop();
                selectedLand.Interact();
                selectedLand = null;
                lockedIn = false;
                return;
            case EquipmentData.ToolType.Pickaxe:
                animator.SetTrigger("Pickaxe");
                SFXManager.instance?.PlayMine();
                selectedLand.Interact();
                selectedLand = null;
                lockedIn = false;
                return;
            case EquipmentData.ToolType.Shovel:
                animator.SetTrigger("Shovel");
                SFXManager.instance?.PlayDig();
                selectedLand.Interact();
                selectedLand = null;
                lockedIn = false;
                return;
            default:
                //we dont have animation for the rest so we just complete it
                CompleteInteraction();
                break;
        }
        Debug.Log("Not on any land!");
    }

    //to be called via the animator
    public void CompleteInteraction()
    {
        if (selectedLand != null)
        {
            selectedLand.Interact();
        }
        selectedLand = null;
        lockedIn = false;
    }

}    

