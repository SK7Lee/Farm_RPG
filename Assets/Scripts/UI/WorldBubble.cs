using System.Collections;
using UnityEngine;

public class WorldBubble : MonoBehaviour
{
    Transform cameraPos;
    [SerializeField]
    Animator speechAnimator;

    public enum Emote
    {
        Happy, BadMood, Heart, Thinking, Sad
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraPos = FindObjectOfType<CameraController>().transform;
    }

    public void Display(Emote mood)
    {
        ResetAnimator();
        speechAnimator.SetBool(mood.ToString(), true);
    }

    public void Display(Emote mood, float time)
    {
        Display(mood);
        StartCoroutine(Delay(time));
    }

    IEnumerator Delay(float time)
    {
        yield return new WaitForSeconds(time);
        ResetAnimator();
        gameObject.SetActive(false);
    }

    void ResetAnimator()
    {
        foreach (AnimatorControllerParameter param in speechAnimator.parameters)
        {
            speechAnimator.SetBool(param.name, false);
        }
    }

    private void OnEnable()
    {
        ResetAnimator();
    }

    // Update is called once per frame
    void Update()
    {
        //look at camera
        transform.rotation = cameraPos.rotation;
    }
}
