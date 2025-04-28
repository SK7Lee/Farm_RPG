using System;
using UnityEngine;
using UnityEngine.UI ;
using TMPro;

public class YesNoPrompt : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI promptText;
    Action onYesSelected;

    public void CreatePrompt(string message, Action onYesSelected)
    {
        this.onYesSelected = onYesSelected;
        promptText.text = message;
    }
    
    public void Answer(bool yes)
    {
        if (yes && onYesSelected != null)
        {
            onYesSelected();
        }
        onYesSelected = null;
        gameObject.SetActive(false);
    }
}
