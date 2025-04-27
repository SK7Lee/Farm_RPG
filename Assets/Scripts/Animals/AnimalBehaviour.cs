using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AnimalMovement))]
public class AnimalBehaviour : InteractableObject
{
    protected AnimalRelationshipState relationship;
    protected AnimalMovement movement;
    protected AnimalRenderer animalRenderer;
    [SerializeField]
    protected WorldBubble speechBubble;

    protected virtual void Start()
    {
        movement = GetComponent<AnimalMovement>();
        
    }

    public void LoadRelationship(AnimalRelationshipState relationship)
    {
        this.relationship = relationship;
        animalRenderer = GetComponent<AnimalRenderer>();
        animalRenderer.RenderAnimal(relationship.age, relationship.animalType);
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

        //Set the speech bubble to true
        speechBubble.gameObject.SetActive(true);
        WorldBubble.Emote emote = WorldBubble.Emote.Thinking;

        switch (relationship.Mood)
        {
            case int n when (n >= 200):
                emote = WorldBubble.Emote.Heart;
                break;
            case int n when (n < 30):
                emote = WorldBubble.Emote.Sad;
                break;
            case int n when (n >= 30 && n < 60):
                emote = WorldBubble.Emote.BadMood;
                break;
            default:
                emote = WorldBubble.Emote.Happy;
                break;
        }
        speechBubble.Display(emote, 3f);
        Debug.Log("First conversation with " + relationship.name + " mood " + relationship.Mood);

    }
}
