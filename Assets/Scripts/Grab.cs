using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    [SerializeField] float grabDist;
    FixedJoint2D staticJoint;
    SpringJoint2D springJoint;

    PlayerMovement movement;

    void Start()
    {
        staticJoint = gameObject.GetComponent<FixedJoint2D>();
        springJoint = gameObject.GetComponent<SpringJoint2D>();
        movement = gameObject.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerMovement.input.Player.Grab.WasPressedThisFrame())
        {
            float boxDist;
            GameObject closestBox = PushableObjectManager.Instance.GetClosestBox(transform.position, out boxDist);

            float switchDist;
            GameObject closestSwitch = PushableObjectManager.Instance.GetClosestSwitch(transform.position, out switchDist);

            float pickableDist;
            GameObject closestPickable = PushableObjectManager.Instance.GetClosestPickable(transform.position, out pickableDist);

            List<(float, GameObject)> listas = new List<(float, GameObject)>();
            listas.Add((boxDist, closestBox));
            listas.Add((switchDist, closestSwitch));
            listas.Add((pickableDist, closestPickable));
            listas.Sort();
            //Debug.Log(closestPickable);
            if (listas[0].Item2==null) return;
            if (listas[0].Item2==closestBox)
            {
                Vector2 closestOnBox = closestBox.GetComponent<Collider2D>().ClosestPoint((Vector2)transform.position + GetComponent<Collider2D>().offset);
                Vector2 closestOnPlayer = gameObject.GetComponent<Collider2D>().ClosestPoint(closestOnBox);
                float tempDist = (closestOnBox - closestOnPlayer).magnitude;
                
                if (tempDist <= grabDist)
                {
                    staticJoint.connectedBody = closestBox.GetComponent<Rigidbody2D>();
                    staticJoint.enabled = true;

                    /*if (closestBox.transform.position.x > transform.position.x)
                    {
                        movement.lookRight = false;
                    }
                    else
                    {
                        movement.lookRight = true;
                    }
                    movement.cantRotateWithMove = true;*/
                }

                
            }else if (listas[0].Item2 == closestPickable)
            {
                Vector2 closestOnPickable = closestPickable.GetComponent<Collider2D>().ClosestPoint((Vector2)transform.position + GetComponent<Collider2D>().offset);
                Vector2 closestOnPlayer = gameObject.GetComponent<Collider2D>().ClosestPoint(closestOnPickable);
                float tempDist = (closestOnPickable - closestOnPlayer).magnitude;
               

                if (tempDist <= grabDist)
                {
                    springJoint.connectedBody = closestPickable.GetComponent<Rigidbody2D>();
                    springJoint.enabled = true;
                    springJoint.distance = 0.3f;
                }
            }else if(listas[0].Item2 == closestSwitch)
            {
                Vector2 closestOnSwitch = closestSwitch.GetComponent<Collider2D>().ClosestPoint((Vector2)transform.position + GetComponent<Collider2D>().offset);
                Vector2 closestOnPlayer = gameObject.GetComponent<Collider2D>().ClosestPoint(closestOnSwitch);
                float tempDist = (closestOnSwitch - closestOnPlayer).magnitude;

                if (tempDist <= switchDist)
                {
                    closestSwitch.GetComponent<Switch>().Toggle();
                }
            }
        }
        if (PlayerMovement.input.Player.Grab.WasReleasedThisFrame())
        {
            staticJoint.enabled = false;
            springJoint.enabled = false;

            movement.cantRotateWithMove = false;
        }
    }
}
