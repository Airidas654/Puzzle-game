using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    private float epsilon = 0.01f;
    Rigidbody2D rg;
    private void Start()
    {
        rg = GetComponent<Rigidbody2D>();
        PushableObjectManager.RegisterBox(gameObject);
    }
    private void CheckIfMoving()
    {
        float movementMag = rg.velocity.sqrMagnitude;
        if (movementMag > epsilon)
        {
            SoundManager.Instance.GetSound("Box").PlayWithCooldown(0.4f);
        }
    }

    private void FixedUpdate()
    {
        CheckIfMoving();
    }
}