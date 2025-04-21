using Unity.VisualScripting;
using UnityEngine;

public class AnimalBehaviour : InteractableObject
{
    AnimalRelationshipState relationship;
    public void LoadRelationship(AnimalRelationshipState relationship)
    {
        this.relationship = relationship;
    }

    public override void Pickup()
    {
        if (relationship == null)
        {
            Debug.LogError("Animal relationship is null. Cannot pick up animal.");
            return;
        }

        DialogueManager.Instance.StartDialogue(DialogueManager.CreateSimpleMessage($"{relationship.name} seems happy"));

    }

}
