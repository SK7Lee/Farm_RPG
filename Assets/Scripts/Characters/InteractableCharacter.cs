using UnityEngine;

public class InteractableCharacter : InteractableObject
{
    public CharacterData characterData;

    public override void Pickup()
    {
        DialogueManager.Instance.StartDialogue(characterData.defaultDialogue);
    }


}    
