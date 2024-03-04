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

    bool isCycle;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentPoint = positions[0];
    }
    int index = 0;
    int count = 0;
    bool isForward = true;
    // Update is called once per frame
    void FixedUpdate()
    {

        //////////// TODO enemy goes to next point
        if (currentPoint == positions[index] && isForward == true)
        {
            rb.velocity = (positions[index+1]-currentPoint).normalized*movespeed;
        }
        else if(currentPoint == positions[index] && isForward == false)
        {
            rb.velocity = new Vector2((positions[index - 1].x - currentPoint.x), (positions[index - 1].y - currentPoint.y));
        }
        
        

        if(isForward == true && Vector2.Distance(transform.position, currentPoint) < movespeed && currentPoint == positions[index + 1])
        {
            currentPoint = positions[index + 1];
            index++;
            count++;
            if (count == positions.Count - 1 && currentPoint == positions[positions.Count - 1] && isForward == true)
            {
                isForward = false;
                count = 0;
                
            }
        }
        else if(isForward == false && Vector2.Distance(transform.position, currentPoint) < movespeed && currentPoint == positions[index - 1])
        {
            currentPoint = positions[index - 1];
            index--;
            count++;
            if (count == positions.Count - 1 && currentPoint == positions[0] && isForward == false)
            {
                isForward = true;
                count = 0;
                
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
