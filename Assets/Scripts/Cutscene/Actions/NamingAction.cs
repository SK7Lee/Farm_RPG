using UnityEngine;

public class NamingAction : CutsceneAction
{
    //the message to display
    [SerializeField]
    private string namingPrompt;
    //the key to update in the blackboard
    [SerializeField]
    private string blackboardKeyToUpdate;

    public override void Execute()
    {
        UIManager.Instance.TriggerNamingPrompt(namingPrompt, CompleteNaming);
    }
    
    void CompleteNaming(string input)
    {
        //retrieve blackboard
        GameBlackboard blackboard = GameStateManager.Instance.GetBlackboard();

        blackboard.SetValue(blackboardKeyToUpdate, input);
        onExecutionComplete?.Invoke();
    }

}
