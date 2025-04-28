using UnityEngine;

[RequireComponent(typeof(CharacterMovement), typeof(Animator))]
public class CharacterRenderer : MonoBehaviour
{
    CharacterMovement movement;
    Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<CharacterMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("Walk", movement.IsMoving());
    }
}
