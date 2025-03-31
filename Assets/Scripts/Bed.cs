using UnityEngine;

public class Bed : InteractableObject
{
    public override void Pickup()
    {
        Debug.Log("Pickup() called!");
        UIManager.Instance.TriggerYesNoPrompt("Do you want to sleep?", GameStateManager.Instance.Sleep);
    }
}
