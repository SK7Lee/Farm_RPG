using System;
using System.Collections.Generic;
using UnityEngine;

public class SpeechAction : CutsceneAction
{
    public List<DialogueLine> dialogueLines;

    public override void Execute()
    {
        DialogueManager.Instance.StartDialogue(dialogueLines, onExecutionComplete);
    }
}
