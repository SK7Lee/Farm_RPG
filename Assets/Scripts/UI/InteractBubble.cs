using UnityEngine;
using UnityEngine.UI;

public class InteractBubble : WorldUI
{
    [SerializeField]
    Text messageText;
    public void Display(string message)
    {
        messageText.text = message;
    }
}
