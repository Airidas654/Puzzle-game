using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float movespeed;
    [SerializeField] float epsilon = 0.4f;

    public List<Vector2> positions;

    Rigidbody2D rb;


    public bool isCycle;
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
            nextIndex = reversed ? positions.Count - 1 : 1;
        }
        srenderer = GetComponent<SpriteRenderer>();
        //nextPoint = positions[1];
    }
    int index = 0;
    int count = 0;
    int nextIndex = 0;
    bool isForward = true;
    // Update is called once per frame
    void FixedUpdate()
    {

        rb.velocity = (positions[nextIndex] - (Vector2)transform.position).normalized*movespeed;

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
            if (isForward == true && Vector2.Distance(transform.position, positions[nextIndex]) <= epsilon)
            {
                count++;
                
                if (count != positions.Count - 1)
                {
                    index++;
                    nextIndex = index + 1;
                }

                if (isForward == true && count == positions.Count - 1)
                {
                    isForward = false;
                    count = 0;
                    nextIndex = index;
                    index++;

                }
            }
            if (isForward == false && Vector2.Distance(transform.position, positions[nextIndex]) <= epsilon)
            {

                count++;
                
                if (count != positions.Count - 1)
                {
                    index--;
                    nextIndex = index - 1;
                }

                if (isForward == false && count == positions.Count - 1)
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
            if (Vector2.Distance(transform.position, positions[nextIndex]) <= epsilon)
            {
                //count++;
                if (!reversed)
                    nextIndex = (nextIndex + 1) % positions.Count;
                else
                    nextIndex = ((nextIndex - 1) + positions.Count) % positions.Count;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GameManager.inst.Death();
        }
    }
    private void OnDrawGizmos()
    {
        for (int i = 0; i < positions.Count - 1; i++)
        {
            Gizmos.DrawLine(positions[i], positions[i + 1]);
        }
        if (isCycle)
        {
            Gizmos.DrawLine(positions[positions.Count-1], positions[0]);
        }
    }
}
