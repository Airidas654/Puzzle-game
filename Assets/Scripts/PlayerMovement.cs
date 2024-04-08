using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public static GameObject currPlayer;
    [SerializeField] float moveSpeed = 10;
    Rigidbody2D rg;
    Animator animator;
    [HideInInspector] public Vector2 moveDir;

    [HideInInspector]
    public bool cantRotateWithMove = false;
    public bool lookRight = false;
    bool canMove;



    public void StopMovement()
    {
        canMove = false;
        rg.velocity = Vector2.zero;
    }

    private void Awake()
    {
        currPlayer = gameObject;

        input = new PlayerController();
        input.Enable();
        input.Player.Enable();
    }

    public static PlayerController input;
    void Start()
    {

        rg = GetComponent<Rigidbody2D>(); 
        animator = GetComponent<Animator>();
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (GameManager.inst.CantDoAnything || PauseUi.instance.paused) return;
        if (canMove)
        {
            float horizontalRaw = input.Player.Movement.ReadValue<Vector2>().x;
            float verticalRaw = input.Player.Movement.ReadValue<Vector2>().y;


            animator.SetBool("Walking", horizontalRaw != 0 || verticalRaw != 0);

            if ((!cantRotateWithMove && horizontalRaw < 0) || (cantRotateWithMove && !lookRight))
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
            else if ((!cantRotateWithMove && horizontalRaw > 0) || (cantRotateWithMove && lookRight))
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
            Vector2 inputVector = input.Player.Movement.ReadValue<Vector2>();
            float mag = inputVector.magnitude;
            if (mag > 1)
            {
                inputVector /= mag;
            }
            moveDir = inputVector;
            rg.velocity = moveDir * moveSpeed;
        }
    }
}
