using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueCondition : IConditional
{
    //an identifier for the dialogue condition for easier editing
    public string id;
    [SerializeField]
    public BlackboardCondition[] conditions;
    public List<DialogueLine> dialogueLine;

    public bool CheckConditions(out int conditionsMet)
    {
        IConditional conditionChecker = this;
        return conditionChecker.CheckConditions(conditions, out conditionsMet);
    }

}
