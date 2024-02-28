using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    [SerializeField] float grabDist;
    FixedJoint2D join;

    void Start()
    {
        join = gameObject.GetComponent<FixedJoint2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            float boxDist;
            GameObject closestBox = PushableObjectManager.Instance.GetClosestBox(transform.position, out boxDist);
            if (closestBox != null)
            {
                float tempDist = (gameObject.GetComponent<Collider2D>().ClosestPoint(closestBox.transform.position) - new Vector2(closestBox.transform.position.x, closestBox.transform.position.y)).magnitude;

                if (tempDist <= grabDist)
                {
                    join.connectedBody = closestBox.GetComponent<Rigidbody2D>();
                    join.enabled = true;
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            join.enabled = false;
        }
    }
}
