using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("Dialogue Components")]
    public GameObject dialoguePanel;
    public Text speakerText;
    public Text dialogueText;

    Queue<DialogueLine> dialogueQueue;
    Action onDialogueEnd = null;
    bool isTyping = false;

    //implement a proper player control stop mechanism
    PlayerController playerController;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            //Set the instance to this object
            Instance = this;
        }
    }

    public void Talk(string speaker, string message)
    {
        dialoguePanel.SetActive(true);
        speakerText.text = speaker;
        speakerText.transform.parent.gameObject.SetActive(speaker != "");
        //dialogueText.text = message;
        StartCoroutine(TypeText(message));
    }

    private void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
    }

    //initiate the dialogue with a queue of dialogue lines
    public void StartDialogue(List<DialogueLine> dialogueLinesToQueue)
    {

        dialogueQueue = new Queue<DialogueLine>(dialogueLinesToQueue);
        
        if (playerController != null)
        {
            playerController.enabled = false;
        }
        
        UpdateDialogue();
    }

    //with an action to excute when the dialogue ends
    public void StartDialogue(List<DialogueLine> dialogueLinesToQueue,Action onDialogueEnd)
    {
        StartDialogue(dialogueLinesToQueue);
        this.onDialogueEnd = onDialogueEnd;
    }

    public void UpdateDialogue()
    {
        if (isTyping)
        {
            //StopAllCoroutines();
            isTyping = false;
            return;
        }

        dialogueText.text = string.Empty;
        if (dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }
        DialogueLine line = dialogueQueue.Dequeue();
        Talk(line.speaker, ParseVariables(line.message));   
    }

    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);

        if (playerController != null)
        {
            playerController.enabled = true;
        }

        onDialogueEnd?.Invoke();
        onDialogueEnd = null;
    }

    IEnumerator TypeText(string textToType)
    {
        isTyping = true;
        char[] charsToType = textToType.ToCharArray();
        for (int i = 0; i < charsToType.Length; i++)
        {
            dialogueText.text += charsToType[i];
            yield return new WaitForEndOfFrame();
            if (!isTyping)
            {
                dialogueText.text = textToType;
                break;
            }
        }
        isTyping = false;
    }
        
    public static List<DialogueLine> CreateSimpleMessage(string message)
    {
        DialogueLine messageDialogueLine = new DialogueLine("", message);
        List<DialogueLine> listToReturn = new List<DialogueLine>();
        listToReturn.Add(messageDialogueLine);
        return listToReturn;
    }

    //filter to see if there is any dialogue lines we can overwrite with
    public static List<DialogueLine> SelectDialogue(List<DialogueLine> dialogueToExcute,DialogueCondition[] conditions)
    {
        //replace the dialogue set with the highest condition score
        int highestConditionScore = -1;
        foreach (DialogueCondition condition in conditions)
        {
            //check if conditions met first
            if (condition.CheckConditions(out int score))
            {
                if (score > highestConditionScore) 
                {
                    highestConditionScore = score;
                    dialogueToExcute = condition.dialogueLine;
                    Debug.Log("Will play" + condition.id);
                }
                

            }
        }
        return dialogueToExcute;
    }


    /// <param name="message">the string to pass in </param>
    string ParseVariables(string message)
    {
        if (GameStateManager.Instance != null)
        {
            //get the blackboard
            GameBlackboard blackboard = GameStateManager.Instance.GetBlackboard();

            if (blackboard != null)
            {
                //look for strings enclosed with {}
                string pattern = @"\{([^}]+?)\}";
                //regex replacement step
                message = Regex.Replace(message, pattern, match =>
                {
                    //the variable name enclosed in the "{}"
                    string variableName = match.Groups[1].Value;

                    //if there is a string value, return it
                    if (blackboard.TryGetValueAsString(variableName, out string strValue))
                    {
                        return strValue;
                    }
                    //nothing found, so nothing is returned
                    return "";
                });
            }
        }
        return message;
    }



}
