using SoCollection;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Cutscene", menuName = "Cutscene/Cutscene")]
public class Cutscene : ScriptableObject, IConditional
{
    public BlackboardCondition[] conditions;
    //whether this event cutscene can play again oonce it has occurred
    public bool recurring;

    public SoCollection<CutsceneAction> action;

    //check if the condition is met
    public bool CheckConditions(out int score)
    {
        IConditional conditional = this;
        return conditional.CheckConditions(conditions, out score);
    }

}
