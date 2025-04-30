using UnityEngine;

[CreateAssetMenu(fileName = "Cutscene", menuName = "Cutscene/Cutscene")]
public class Cutscene : ScriptableObject
{
    public BlackboardCondition[] conditions;
    //whether this event cutscene can play again oonce it has occurred
    public bool recurring;
    public CutsceneProcessData process; 

}
