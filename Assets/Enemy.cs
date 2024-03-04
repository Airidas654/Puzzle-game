using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float movespeed;

    public List<Vector2> positions;

    Rigidbody2D rb;

    Vector2 currentPoint;
    Vector2 nextPoint;

    bool isCycle;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentPoint = positions[0];
        nextPoint = positions[1];
    }
    int index = 0;
    int count = 0;
    bool isForward = true;
    // Update is called once per frame
    void FixedUpdate()
    {

        //////////// TODO enemy goes to next point
        if (currentPoint == positions[index])
        {
            rb.velocity = (nextPoint - (Vector2)transform.position).normalized*movespeed;
        }
        
        
        

        if(isForward == true && Vector2.Distance(transform.position, nextPoint) <= 0.5)
        {
            count++;
            currentPoint = nextPoint;
            if (count != positions.Count - 1)
            {
                index++;
                nextPoint = positions[index + 1];
            }
            
            if (isForward == true && count == positions.Count - 1 && currentPoint == positions[positions.Count - 1])
            {
                isForward = false;
                count = 0;
                nextPoint = positions[index];
                index++;

            }
        }
        if(isForward == false && Vector2.Distance(transform.position, nextPoint) <= 0.5)
        {
            
            count++;
            currentPoint = nextPoint;
            if (count != positions.Count - 1)
            {
                index--;
                nextPoint = positions[index - 1];
            }
            
            if (isForward == false && count == positions.Count - 1 && currentPoint == positions[0])
            {
                isForward = true;
                count = 0;
                nextPoint = positions[index];
                index--;

            }
        }

    }
    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < positions.Count - 1; i++)
        {
            Gizmos.DrawLine(positions[i], positions[i + 1]);
        }
    }
}
