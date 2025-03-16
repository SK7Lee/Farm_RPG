using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public ItemData item;
    public virtual void Pickup()
    {
        InventoryManager.Instance.EquipHandSlot(item);
        InventoryManager.Instance.RenderHand();
        Destroy(gameObject);
    }
}
