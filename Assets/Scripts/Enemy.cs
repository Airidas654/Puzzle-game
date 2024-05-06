using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float movespeed;
    [SerializeField] float epsilon = 0.4f;

    public List<Vector2> patrolPositions = new List<Vector2>();

    Rigidbody2D rb;


    [SerializeField] bool isCycle;
    [SerializeField] bool reversed;
    SpriteRenderer srenderer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!isCycle)
        {
            nextIndex = 1;
        }
        else
        {
            nextIndex = reversed ? patrolPositions.Count - 1 : 1;
        }
        srenderer = GetComponent<SpriteRenderer>();
        //nextPoint = positions[1];
    }
    int index = 0;
    int count = 0;
    int nextIndex = 0;
    bool isForward = true;
    // FixedUpdate is called once per fixed frame
    void FixedUpdate()
    {
        if (nextIndex >= 0 && nextIndex < patrolPositions.Count)
        {
            rb.velocity = (patrolPositions[nextIndex] - (Vector2)transform.position).normalized * movespeed;
        }

        if (rb.velocity.x < 0)
        {
            srenderer.flipX = true;
        }
        else
        {
            srenderer.flipX = false;
        }

        if (!isCycle)
        {
            if (isForward == true && Vector2.Distance(transform.position, patrolPositions[nextIndex]) <= epsilon)
            {
                count++;
                
                if (count != patrolPositions.Count - 1)
                {
                    index++;
                    nextIndex = index + 1;
                }

                if (isForward == true && count == patrolPositions.Count - 1)
                {
                    isForward = false;
                    count = 0;
                    nextIndex = index;
                    index++;

                }
            }
            if (isForward == false && Vector2.Distance(transform.position, patrolPositions[nextIndex]) <= epsilon)
            {

                count++;
                
                if (count != patrolPositions.Count - 1)
                {
                    index--;
                    nextIndex = index - 1;
                }

                if (isForward == false && count == patrolPositions.Count - 1)
                {
                    isForward = true;
                    count = 0;
                    nextIndex = index;
                    index--;

                }
            }
        }
        else
        {
            if (Vector2.Distance(transform.position, patrolPositions[nextIndex]) <= epsilon)
            {
                //count++;
                if (!reversed)
                    nextIndex = (nextIndex + 1) % patrolPositions.Count;
                else
                    nextIndex = ((nextIndex - 1) + patrolPositions.Count) % patrolPositions.Count;
               /* if (count != positions.Count - 1)
                {
                    index++;
                    nextIndex = index + 1;
                }
                else
                {
                    index = -1;
                    count = -1;
                    nextIndex = 0;
                }*/
            }
        }

    }

    /// <summary>
    /// Called when object collides with something
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (GameManager.inst != null)
            {
                GameManager.inst.Death();
            }
            else
            {
                Debug.LogError("PLAYER CANT DIE, GameManager.inst = null");
            }
        }
    }

    /// <summary>
    /// Called on gizmos update
    /// </summary>
    private void OnDrawGizmos()
    {
        for (int i = 0; i < patrolPositions.Count - 1; i++)
        {
            Gizmos.DrawLine(patrolPositions[i], patrolPositions[i + 1]);
        }
        if (isCycle)
        {
            Gizmos.DrawLine(patrolPositions[patrolPositions.Count-1], patrolPositions[0]);
        }
    }
}
