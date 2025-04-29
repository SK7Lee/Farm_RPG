using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterMovement))]
public class InteractableCharacter : InteractableObject
{
    public CharacterData characterData;

    //cache the relationship state of the character
    NPCRelationshipState relationship;

    //default rotation of the character
    Quaternion defaulRotation;
    //check if the character is turning
    bool isTurning = false;
    CharacterMovement movement;
    private void Start()
    {
        relationship = RelationshipStats.GetRelationship(characterData);

        movement = GetComponent<CharacterMovement>();
        //cache the default rotation of the character
        defaulRotation = transform.rotation;
        //add listener
        GameStateManager.Instance.onIntervalUpdate.AddListener(OnIntervalUpdate);
    }

    void OnIntervalUpdate()
    {
        //get data on its location
        NPCLocationState locationState = NPCManager.Instance.GetNPCLocation(characterData.name);
        movement.MoveTo(locationState);
        StartCoroutine(LookAt(Quaternion.Euler(locationState.facing)));
    }

    public override void Pickup()
    {
        LookAtPlayer();
        TriggerDialogue();
    }

    #region Rotation
    void LookAtPlayer()
    {
        //Get the player transform
        Transform player = FindObjectOfType<PlayerController>().transform;
        //Get vector to player
        Vector3 dir = player.position - transform.position; 
        dir.y = 0; //set y to 0
        //Convert to quaternion
        Quaternion lookRot = Quaternion.LookRotation(dir);
        //Look at player
        StartCoroutine(LookAt(lookRot));
    }

    IEnumerator LookAt(Quaternion lookRot)
    {
        //check if the coroutine is already running
        if (isTurning)
        {
            //stop corooutine
            isTurning = false;
        }
        else
        {
            isTurning = true;
        }
        while (transform.rotation != lookRot)
        {
            if (!isTurning) 
            {
                //stop corooutine

                yield break;
            }
            //dont do anyting if the character is moving
            if (!movement.IsMoving()) transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRot,Time.fixedDeltaTime *720);
            yield return new WaitForFixedUpdate();
        }
        isTurning = false;
    }

    void ResetRotation()
    {
        StartCoroutine(LookAt(defaulRotation));
    }
    #endregion
    #region Conversation-related Functions
    void TriggerDialogue()
    {
        movement.ToggleMovement(false);
        //check if player hold sth
        if (InventoryManager.Instance.SlotEquipped(InventorySlot.InventoryType.Item))
        {
            GiftDialogue();
            return;
        }
        List<DialogueLine> dialogueToHave = characterData.defaultDialogue;


        //check to determine which dialogue to have
        System.Action onDialogueEnd = () =>
        {
            //allow for movement
            movement.ToggleMovement(true);
            //continue going to its destination if it was on the way
            //reset its initial position
            OnIntervalUpdate();
        };

        //onDialogueEnd += ResetRotation;

        dialogueToHave = DialogueManager.SelectDialogue(dialogueToHave, characterData.dialogues);

        //is the player meeting for the firts time?
        if (RelationshipStats.FirstMeeting(characterData))
        {
            dialogueToHave = characterData.onFirstMeet;
            onDialogueEnd += OnFirstMeeting;
        }
        if (RelationshipStats.IsFirstConversationOfTheDay(characterData))
        {
            onDialogueEnd += OnFirstConversation;
        }

        DialogueManager.Instance.StartDialogue(dialogueToHave, onDialogueEnd);
    }

    void GiftDialogue()
    {
        if (!EligibleForGift()) return;
        //get the itemslotdata of what the player is holding
        ItemSlotData handSlot = InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Item);
        List<DialogueLine> dialogueToHave = characterData.neutralGiftDialogue;

        System.Action onDialogueEnd = () =>
        {
            //continue going to its destination if it was on the way
            //reset its initial position
            OnIntervalUpdate();
            //allow for movement
            movement.ToggleMovement(true);
            //mark gift as given for today
            relationship.giftGivenToday = true;
            //remove the item from thee player's hand
            InventoryManager.Instance.ConsumeItem(handSlot);
        };

        //onDialogueEnd += ResetRotation;

        bool isBirthday = RelationshipStats.IsBirthday(characterData);
        //friendship points to add
        int pointsToAdd = 0;
        switch (RelationshipStats.GetReactionToGift(characterData, handSlot.itemData))
        {
            case RelationshipStats.GiftReaction.Like:
                dialogueToHave = characterData.likedGiftDialogue;
                pointsToAdd = 80;
                if (isBirthday) dialogueToHave = characterData.birthdayLikedGiftDialogue;
                break;
            case RelationshipStats.GiftReaction.Dislike:
                dialogueToHave = characterData.dislikedGiftDialogue;
                pointsToAdd = -20;
                if (isBirthday) dialogueToHave = characterData.birthdayDislikedGiftDialogue;
                break;
            case RelationshipStats.GiftReaction.Neutral:
                dialogueToHave = characterData.neutralGiftDialogue;
                pointsToAdd = 20;
                if (isBirthday) dialogueToHave = characterData.birthdayNeutralGiftDialogue;
                break;
        }

        if (isBirthday)
        {
            pointsToAdd *= 8;
        }
        RelationshipStats.AddFriendshipPoints(characterData, pointsToAdd);
        DialogueManager.Instance.StartDialogue(dialogueToHave, onDialogueEnd);

    }

    bool EligibleForGift()
    {
        if (RelationshipStats.FirstMeeting(characterData))
        {
            DialogueManager.Instance.StartDialogue(DialogueManager.CreateSimpleMessage("You have not unlocked this character yet."));

            return false;
        }
        if (RelationshipStats.GiftGivenToday(characterData))
        {
            DialogueManager.Instance.StartDialogue(DialogueManager.CreateSimpleMessage($"You have already given {characterData.name} a gift today."));
            return false;
        }
        return true;
    }

    void OnFirstMeeting()
    {
        RelationshipStats.UnlockCharacter(characterData);
        relationship = RelationshipStats.GetRelationship(characterData);

    }

    void OnFirstConversation()
    {
            Debug.Log("First conversation of the day");
            RelationshipStats.AddFriendshipPoints(characterData, 20);

            //if the player has not talked to the character today, set the hasTalkedToday to true
            relationship.hasTalkedToday = true;
        
    }
    #endregion
}
