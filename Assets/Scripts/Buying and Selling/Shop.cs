using System.Collections.Generic;
using UnityEngine;

public class Shop : InteractableObject
{
    [SerializeField]
    CharacterData owner;

    public List<ItemData> shopItems;

    [Header("Dialogues")]
    public List<DialogueLine> dialogueOnShopOpen;

    public static void Purchase(ItemData item, int quantity)
    {
        int totalCost = item.cost * quantity;
        if (PlayerStats.Money >= totalCost)
        {
            PlayerStats.Spend(totalCost);
            ItemSlotData purchasedItem = new ItemSlotData(item, quantity);
            InventoryManager.Instance.ShopToInventory(purchasedItem);
        }

    }

    public override void Pickup()
    {
        //check if the store is manned
        if (!IsStoreManned()) return;
        DialogueManager.Instance.StartDialogue(dialogueOnShopOpen,  OpenShop);
    }

    bool IsStoreManned()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 4);
        foreach (Collider col in colliders)
        {
            if (col.tag != "Item") continue;

            InteractableCharacter characterInteractable = col.gameObject.GetComponent<InteractableCharacter>();
            if (characterInteractable == null) continue;
            if (characterInteractable.characterData.name == owner.name) return true;
        }
        return false;
    }

    void OpenShop()
    {
        UIManager.Instance.OpenShop(shopItems);
    }

}
