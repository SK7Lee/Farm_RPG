using UnityEngine;

public interface IConditional
{
    public bool CheckConditions(BlackboardCondition[] conditions, out int conditionsMet)
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
