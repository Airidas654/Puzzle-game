using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public static GameObject currPlayer;
    public static bool isCool = false;
    [SerializeField] private float moveSpeed = 10;
    private Rigidbody2D rg;
    private Animator animator;
    [HideInInspector] public Vector2 moveDir;

    [SerializeField] AnimatorController coolAnimator;

    [HideInInspector] public bool cantRotateWithMove = false;
    public bool lookRight = false;
    private bool canMove;


    public void StepSound()
    {
        SoundManager.Instance.GetSound("Step").PlayOneShot();
    }

    public void StopMovement()
    {
        canMove = false;
        if (rg != null)
        {
            rg.velocity = Vector2.zero;
        }
    }

    private void Awake()
    {
        currPlayer = gameObject;

        input = new PlayerController();
        input.Enable();
        input.Player.Enable();
    }

    public static PlayerController input;

    private void Start()
    {
        rg = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        canMove = true;

        if (isCool)
        {
            animator.runtimeAnimatorController = coolAnimator;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (PlayerMovement.input.Player.Cheat.WasPressedThisFrame())
        {
            isCool = true;
        }

        if (GameManager.inst.CantDoAnything || PauseUi.instance.paused) return;
        if (canMove)
        {
            var horizontalRaw = input.Player.Movement.ReadValue<Vector2>().x;
            var verticalRaw = input.Player.Movement.ReadValue<Vector2>().y;


            animator.SetBool("Walking", horizontalRaw != 0 || verticalRaw != 0);

            if ((!cantRotateWithMove && horizontalRaw < 0) || (cantRotateWithMove && !lookRight))
                GetComponent<SpriteRenderer>().flipX = false;
            else if ((!cantRotateWithMove && horizontalRaw > 0) || (cantRotateWithMove && lookRight))
                GetComponent<SpriteRenderer>().flipX = true;
            var inputVector = input.Player.Movement.ReadValue<Vector2>();
            var mag = inputVector.magnitude;
            if (mag > 1) inputVector /= mag;
            moveDir = inputVector;
            rg.velocity = moveDir * moveSpeed;
        }
    }
}