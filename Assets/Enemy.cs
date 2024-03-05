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

    Vector2 currentPoint;
    //Vector2 nextPoint;

    public bool isCycle;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentPoint = positions[0];
        nextIndex = 1;
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


        if (!isCycle)
        {
            if (isForward == true && Vector2.Distance(transform.position, positions[nextIndex]) <= epsilon)
            {
                count++;
                //currentPoint = positions[nextIndex];
                if (count != positions.Count - 1)
                {
                    index++;
                    nextIndex = index + 1;
                }

                if (isForward == true && count == positions.Count - 1)//&& currentPoint == positions[positions.Count - 1])
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
                //currentPoint = positions[nextIndex];
                if (count != positions.Count - 1)
                {
                    index--;
                    nextIndex = index - 1;
                }

                if (isForward == false && count == positions.Count - 1)//&& currentPoint == positions[0])
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
                count++;
                if (count != positions.Count - 1)
                {
                    index++;
                    nextIndex = index + 1;
                }
                else
                {
                    index = -1;
                    count = -1;
                    nextIndex = 0;
                }
            }
        }

    }
    private void OnDrawGizmos()
    {
        for (int i = 0; i < positions.Count - 1; i++)
        {
            Gizmos.DrawLine(positions[i], positions[i + 1]);
        }
    }
}
