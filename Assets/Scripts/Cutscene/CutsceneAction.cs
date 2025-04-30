using System;
using UnityEngine;

[Serializable]
public class CutsceneAction 
{
   public enum ActionState
    {
        Ready,
        Running,    
        Finised
    }

    public ActionState state;

    public void Excute()
    {
        //set state to run
        state = ActionState.Running;
    }
}
