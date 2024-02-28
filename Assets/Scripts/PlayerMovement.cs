using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10;
    Rigidbody2D rg;
    [HideInInspector] public Vector2 moveDir;
    void Start()
    {
        rg = GetComponent<Rigidbody2D>(); 
    }

    // Update is called once per frame
    void Update()
    {
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
