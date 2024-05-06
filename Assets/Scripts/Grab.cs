using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    [SerializeField] private float grabDist;
    private FixedJoint2D staticJoint;
    private SpringJoint2D springJoint;

    private PlayerMovement movement;

    private void Start()
    {
        staticJoint = gameObject.GetComponent<FixedJoint2D>();
        springJoint = gameObject.GetComponent<SpringJoint2D>();
        movement = gameObject.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (PlayerMovement.input.Player.Grab.WasPressedThisFrame())
        {
            float boxDist;
            var closestBox = PushableObjectManager.GetClosestBox(transform.position, out boxDist);

            float switchDist;
            var closestSwitch = PushableObjectManager.GetClosestSwitch(transform.position, out switchDist);

            float pickableDist;
            var closestPickable = PushableObjectManager.GetClosestPickable(transform.position, out pickableDist);

            var listas = new List<(float, GameObject)>();
            listas.Add((boxDist, closestBox));
            listas.Add((switchDist, closestSwitch));
            listas.Add((pickableDist, closestPickable));
            listas.Sort();
            //Debug.Log(closestPickable);
            if (listas[0].Item2 == null) return;
            if (listas[0].Item2 == closestBox)
            {
                var closestOnBox = closestBox.GetComponent<Collider2D>()
                    .ClosestPoint((Vector2)transform.position + GetComponent<Collider2D>().offset);
                var closestOnPlayer = gameObject.GetComponent<Collider2D>().ClosestPoint(closestOnBox);
                var tempDist = (closestOnBox - closestOnPlayer).magnitude;

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
            }
            else if (listas[0].Item2 == closestPickable)
            {
                var closestOnPickable = closestPickable.GetComponent<Collider2D>()
                    .ClosestPoint((Vector2)transform.position + GetComponent<Collider2D>().offset);
                var closestOnPlayer = gameObject.GetComponent<Collider2D>().ClosestPoint(closestOnPickable);
                var tempDist = (closestOnPickable - closestOnPlayer).magnitude;


                if (tempDist <= grabDist)
                {
                    springJoint.connectedBody = closestPickable.GetComponent<Rigidbody2D>();
                    springJoint.enabled = true;
                    springJoint.distance = 0.3f;
                }
            }
            else if (listas[0].Item2 == closestSwitch)
            {
                var closestOnSwitch = closestSwitch.GetComponent<Collider2D>()
                    .ClosestPoint((Vector2)transform.position + GetComponent<Collider2D>().offset);
                var closestOnPlayer = gameObject.GetComponent<Collider2D>().ClosestPoint(closestOnSwitch);
                var tempDist = (closestOnSwitch - closestOnPlayer).magnitude;

                if (tempDist <= grabDist) closestSwitch.GetComponent<Switch>().Toggle();
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