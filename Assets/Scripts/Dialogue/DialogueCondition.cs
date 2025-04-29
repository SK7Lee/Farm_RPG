using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueCondition 
{
    //an identifier for the dialogue condition for easier editing
    public string id;
    [SerializeField]
    public BlackboardCondition[] conditions;
    public List<DialogueLine> dialogueLine;

    public bool CheckConditions(out int conditionsMet)
    {
        conditionsMet = 0;
        //get the game blackboard
        GameBlackboard blackboard = GameStateManager.Instance.GetBlackboard();

        //ensure every condition is met
        foreach (BlackboardCondition condition in conditions)
        {
            if (!blackboard.CompareValue(condition))
            {
                return false;
            }
            conditionsMet++;

        }
        return true;
    }

}
