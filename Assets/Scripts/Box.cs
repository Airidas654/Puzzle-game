using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    
    private Vector3 previousPosition;
    private bool isMoving;
    private float epsilon = 0.01f;
    private void Start()
    {
        PushableObjectManager.RegisterBox(gameObject);
        previousPosition = transform.position;
    }
    private void CheckIfMoving()
    {
        Vector3 dis = transform.position - previousPosition;
        float movementMag = dis.magnitude;
        if (movementMag > epsilon)
        {
            isMoving = true;
            SoundManager.Instance.GetSound("Box").PlayWithCooldown(0.4f);
        }
        else
        {
            isMoving = false;
            SoundManager.Instance.GetSound("Box").Stop();
        }
        previousPosition = transform.position;
    }

    private void Update()
    {
        CheckIfMoving();
    }
}