using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;

    private float moveSpeed = 4f;

    [Header("Movement System")]
    public float walkSpeed = 4f;
    public float runSpeed = 8f;
    private float gravity = 9.81f;

    PlayerInteraction playerInteraction;

    [Header("Footstep SFX")]
    public float footstepInterval = 0.1f;    // khoảng cách giữa 2 bước chân (giây)
    private float footstepTimer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        playerInteraction = GetComponentInChildren<PlayerInteraction>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!controller.enabled) return; // Ensure the controller is active before proceeding

        Move();

        Interact();

        if (Input.GetKey(KeyCode.RightBracket))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                //advance the entire day
                for (int i = 0; i < 60 + 24; i++)
                {
                    TimeManager.Instance.Tick();
                }
            }
            else
            {
                TimeManager.Instance.Tick();
            }
            
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            UIManager.Instance.ToggleRelationshipPanel();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            SceneTransitionManager.Location location = LocationManager.GetNextLocation
            (SceneTransitionManager.Location.PlayerHome, SceneTransitionManager.Location.Farm);
            Debug.Log(location);
        }
    }

    public void Interact()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            //Interact
            playerInteraction.Interact();
        }

        if (Input.GetButtonDown("Fire2"))
        {
            playerInteraction.ItemInteract();
        }
        if (Input.GetButtonDown("Fire3"))
        {
            playerInteraction.ItemKeep();
        }
    }

    public void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(horizontal, 0f, vertical).normalized;
        Vector3 velocity = moveSpeed * Time.deltaTime * dir;

        if (controller.isGrounded)
        {
            velocity.y = 0;
        }
        velocity.y -= Time.deltaTime * gravity;
        if (Input.GetButton("Sprint"))
        {
            moveSpeed = runSpeed;
            animator.SetBool("Running", true);
        }
        else
        {
            moveSpeed = walkSpeed;
            animator.SetBool("Running", false);

        }

        if (dir.magnitude >= 0.1f)
        {
            //Setting transform rotate
            transform.rotation = Quaternion.LookRotation(dir);

            //move if allowed
            if (controller.enabled)
            {
                controller.Move(velocity);
            }

        }

        animator.SetFloat("Speed", dir.magnitude);
        if (dir.magnitude >= 0.1f && controller.isGrounded)
        {
            SFXManager.instance?.StartWalk(); // bật loop bước chân
        }
        else
        {
            SFXManager.instance?.StopWalk(); // tắt loop khi dừng lại
        }

    }
}
