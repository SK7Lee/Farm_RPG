using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
    // the item information
    public ItemData item;
    public UnityEvent onInteract = new UnityEvent();

    [SerializeField] 
    protected string interactText = "Pick Up";
    [SerializeField]
    protected float offset = 1.5f;  

    public virtual void Pickup()
    {
        //call the onInteract event
        onInteract?.Invoke();
        //set the player's inventory to the item
        InventoryManager.Instance.EquipHandSlot(item);
        //update the changes in scene
        InventoryManager.Instance.RenderHand();
        //destroy the object
        Destroy(gameObject);
    }

    //when the player is hovering around the item
    public virtual void OnHover()
    {
        //display the interact bubble
        UIManager.Instance.InteractPrompt(transform, interactText, offset);
    }

    //what happens when the player stops hovering around the item
    public virtual void OnMoveAway()
    {
        UIManager.Instance.DeactivateInteractPrompt();
    }
}
