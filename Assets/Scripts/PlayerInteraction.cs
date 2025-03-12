using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    PlayerController playerController;

    Land selectedLand = null;

    InteractableObject selectedInteractable = null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = transform.parent.GetComponent<PlayerController>();    
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
            return;
        }

        //Deselect
        if (selectedInteractable != null)
        {
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
        if (InventoryManager.Instance.equippedItem != null)
        {
            return;
        }

        if (selectedLand != null)
        {
            selectedLand.Interact();
            return;
        }

        Debug.Log("Not on any land");
    }

    public void ItemInteract()
    {
        if (InventoryManager.Instance.equippedItem!= null)
        {
            InventoryManager.Instance.HandToInventory(InventorySlot.InventoryType.Item);
            return;
        }

        if (selectedInteractable != null)
        {
            selectedInteractable.Pickup();
        }

    }
}
