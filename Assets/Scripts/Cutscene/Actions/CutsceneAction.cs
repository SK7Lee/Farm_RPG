using System;
using UnityEngine;

public abstract class CutsceneAction : ScriptableObject
{
    protected Action onExecutionComplete; // Change from private to protected
    public void Init(Action onExecutionComplete)
    {
        this.onExecutionComplete = onExecutionComplete;
    }
    public abstract void Execute();
    public void Complete()
    {
        onExecutionComplete?.Invoke();
    }
}
