using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    PlayerController playerController;
    PlayerToolController toolControl;

    Animator animator;

    //EquipmentData equipmentTool;

    Land selectedLand = null;

    InteractableObject selectedInteractable = null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        toolControl = transform.parent.GetComponent<PlayerToolController>();
        
        //playerController = transform.parent.GetComponent<PlayerController>();    
        //animator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit , 1))
        {
            OnInteractableHit(hit);
        }
    }
    //Interact raycast
    void OnInteractableHit(RaycastHit hit)
    {
        Collider other = hit.collider;
        if (other.tag == "Land")
        {
            Land land = other.GetComponent<Land>();
            SelectLand(land);
            return;
        }

        if (other.tag == "Item")
        {
            selectedInteractable = other.GetComponent<InteractableObject>();
            selectedInteractable.OnHover();
            return;
        }

        //Deselect
        if (selectedInteractable != null)
        {
            selectedInteractable.OnMoveAway();
            selectedInteractable = null;
        }

        if (selectedLand != null)
        {
            selectedLand.Select(false);
            selectedLand = null;
        }
    }
    //Handles select
    void SelectLand(Land land)
    {
        if (selectedLand != null)
        {
            selectedLand.Select(false);
        }
        selectedLand = land;
        land.Select(true);
    }

    public void Interact()
    {
     /*   
        //detecting what is the current tool that the player is holding
        ItemData toolSlot = InventoryManager.Instance.GetEquippedSlotItem(InventorySlot.InventoryType.Tool);

        EquipmentData equipmentTool = toolSlot as EquipmentData;

        if (InventoryManager.Instance.SlotEquipped(InventorySlot.InventoryType.Item))
        {
            return;
        }

        if (selectedLand != null)
        {
            EquipmentData.ToolType toolType = equipmentTool.toolType;

            switch (toolType)
            {
                case EquipmentData.ToolType.WateringCan:
                    PlayerStats.UseStamina(2); 
                    animator.SetTrigger("Watering");
                    selectedLand.Interact();
                    return;
                case EquipmentData.ToolType.Hoe:
                    PlayerStats.UseStamina(2);
                    animator.SetTrigger("Plowing");
                    selectedLand.Interact();
                    return;
            }

            return;
        }

        Debug.Log("Not on any land");
        */

        //use the tool

        toolControl.StartToolUse(selectedLand);
    }

    public void ItemInteract()
    {
        if (InventoryManager.Instance.SlotEquipped(InventorySlot.InventoryType.Item))
        {
            //if the player is holding an item
            ItemData itemSlot = InventoryManager.Instance.GetEquippedSlotItem(InventorySlot.InventoryType.Item);
            //check if it is consumable
            if (itemSlot is FoodData)
            {
                FoodData foodData = itemSlot as FoodData;
                foodData.OnConsume();
                //consume it
                InventoryManager.Instance.ConsumeItem(InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Item));
            }
        }

        //check if there is an interactable selected
        if (selectedInteractable != null)
        {
            //pick it up
            selectedInteractable.Pickup();
        }

    }

    public void ItemKeep()
    {
        if (InventoryManager.Instance.SlotEquipped(InventorySlot.InventoryType.Item))
        {
            InventoryManager.Instance.HandToInventory(InventorySlot.InventoryType.Item);
            return;
        }
    }
}
