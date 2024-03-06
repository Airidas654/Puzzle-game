using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10;
    Rigidbody2D rg;
    Animator animator;
    [HideInInspector] public Vector2 moveDir;
    void Start()
    {
        rg = GetComponent<Rigidbody2D>(); 
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalRaw = Input.GetAxisRaw("Horizontal");
        float verticalRaw = Input.GetAxisRaw("Vertical");

        animator.SetBool("Walking", horizontalRaw!=0||verticalRaw!=0);

        if (horizontalRaw < 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (horizontalRaw > 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }

        Vector2 inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        float mag = inputVector.magnitude;
        if (mag > 1)
        {
            inputVector /= mag;
        }
        moveDir = inputVector;
        rg.velocity = moveDir * moveSpeed;
    }
}
