using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AnimalMovement))]
public class AnimalBehaviour : InteractableObject
{
    AnimalRelationshipState relationship;
    AnimalMovement movement;
    public void Start()
    {
        movement = GetComponent<AnimalMovement>();
        
    }

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
        //Set the animal to wander
        TriggerDialogue();
    }

    void TriggerDialogue()
    {
        //Set the animal to stop moving
        movement.ToggleMovement(false);
        //Get the mood
        int mood = relationship.Mood;
        //The dialogue message
        string dialogeLine = $"{relationship.name} seems ";
        System.Action onDialogueEnd = () =>
        {
            //Set the animal to wander
            movement.ToggleMovement(true);
        };

        //Check if the player has interacted with the animal before
        if (!relationship.hasTalkedToday)
        {
            onDialogueEnd += OnFirstConversation;
        }

        //Check the mood and set the dialogue line
        if (mood >= 200)
        {
            dialogeLine += "really happy today!";
        }
        else if (mood > 100 && mood < 200)
        {
            dialogeLine += "happy.";
        }
        else if (mood > 50 && mood <= 100)
        {
            dialogeLine += "fine.";
        }
        else
        {
            dialogeLine += "sad.";
        }

        DialogueManager.Instance.StartDialogue(DialogueManager.CreateSimpleMessage(dialogeLine), onDialogueEnd);
    }

    void OnFirstConversation()
    {
        relationship.Mood += 30;
        relationship.hasTalkedToday = true;
        Debug.Log("First conversation with " + relationship.name + " mood " + relationship.Mood);
    }
}
