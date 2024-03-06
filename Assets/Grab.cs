using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    [SerializeField] float grabDist;
    FixedJoint2D staticJoint;
    SpringJoint2D springJoint;

    void Start()
    {
        staticJoint = gameObject.GetComponent<FixedJoint2D>();
        springJoint = gameObject.GetComponent<SpringJoint2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            float boxDist;
            GameObject closestBox = PushableObjectManager.Instance.GetClosestBox(transform.position, out boxDist);

            float pickableDist;
            GameObject closestPickable = PushableObjectManager.Instance.GetClosestPickable(transform.position, out pickableDist);
            //Debug.Log(closestPickable);
            if (boxDist < pickableDist && closestBox != null)
            {
                float tempDist = (gameObject.GetComponent<Collider2D>().ClosestPoint(closestBox.transform.position) - new Vector2(closestBox.transform.position.x, closestBox.transform.position.y)).magnitude;

                if (tempDist <= grabDist)
                {
                    staticJoint.connectedBody = closestBox.GetComponent<Rigidbody2D>();
                    staticJoint.enabled = true;
                }
            }else if (boxDist > pickableDist && closestPickable != null)
            {
                float tempDist = (gameObject.GetComponent<Collider2D>().ClosestPoint(closestPickable.transform.position) - new Vector2(closestPickable.transform.position.x, closestPickable.transform.position.y)).magnitude;
                
                if (tempDist <= grabDist)
                {
                    springJoint.connectedBody = closestPickable.GetComponent<Rigidbody2D>();
                    springJoint.enabled = true;
                    springJoint.distance = 0.3f;
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            staticJoint.enabled = false;
            springJoint.enabled = false;
        }
    }
}
