using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NamingPrompt : MonoBehaviour
{
    [SerializeField]
    Text promptText;
    [SerializeField]
    TMP_InputField inputField;

    Action<string> onConfirm;
    Action onPromptComplete;
    public void CreatePrompt(string message, Action<string> onConfirm)
    {
        this.onConfirm = onConfirm;
        promptText.text = message;
    }

    //Queue the prompt action to be called when the player presses the confirm button
    public void QueuePromptAction(Action action)
    {
        onPromptComplete += action;
    }

    //When the player presses the confirm button, this method is called
    public void Confirm()
    {
        onConfirm?.Invoke(inputField.text);
        //Reset the input field
        onConfirm = null;
        inputField.text = "";
        gameObject.SetActive(false);
        onPromptComplete?.Invoke();
        onPromptComplete = null;
    }

}
