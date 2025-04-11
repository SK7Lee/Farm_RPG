using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableCharacter : InteractableObject
{
    public CharacterData characterData;
    NPCRelationshipState relationship;
    Quaternion defaulRotation;
    bool isTurning = false;
    private void Start()
    {
        relationship = RelationshipStats.GetRelationship(characterData);
        defaulRotation = transform.rotation;
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
        if (isTurning)
        {
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
                yield break;
            }
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRot,Time.fixedDeltaTime *720);
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
        List<DialogueLine> dialogueToHave = characterData.defaultDialogue;
        //check to determine which dialogue to have
        System.Action onDialogueEnd = null;

        onDialogueEnd += ResetRotation;
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
