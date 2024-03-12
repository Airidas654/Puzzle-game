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
                float tempDist = (gameObject.GetComponent<Collider2D>().ClosestPoint(closestBox.transform.position) - new Vector2(closestBox.transform.position.x, closestBox.transform.position.y)).magnitude;

                if (tempDist <= grabDist)
                {
                    staticJoint.connectedBody = closestBox.GetComponent<Rigidbody2D>();
                    staticJoint.enabled = true;
                }
            }else if (listas[0].Item2 == closestPickable)
            {
                float tempDist = (gameObject.GetComponent<Collider2D>().ClosestPoint(closestPickable.transform.position) - new Vector2(closestPickable.transform.position.x, closestPickable.transform.position.y)).magnitude;
                
                if (tempDist <= grabDist)
                {
                    springJoint.connectedBody = closestPickable.GetComponent<Rigidbody2D>();
                    springJoint.enabled = true;
                    springJoint.distance = 0.3f;
                }
            }else if(listas[0].Item2 == closestSwitch)
            {
                float tempDist = (gameObject.GetComponent<Collider2D>().ClosestPoint(closestSwitch.transform.position) - new Vector2(closestSwitch.transform.position.x, closestSwitch.transform.position.y)).magnitude;

                if (tempDist <= switchDist)
                {
                    closestSwitch.GetComponent<Switch>().Toggle();
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
