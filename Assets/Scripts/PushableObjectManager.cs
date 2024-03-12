using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableObjectManager : MonoBehaviour
{
    public static PushableObjectManager Instance = null;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    List<GameObject> boxes = new List<GameObject>();
    List<GameObject> switches = new List<GameObject>();
    List<GameObject> pickableObjs = new List<GameObject>();

    public void RegisterPickable(GameObject pickable)
    {
        pickableObjs.Add(pickable);
    }
    public void RegisterBox(GameObject box)
    {
        boxes.Add(box);
    }

    public void RegisterSwitch(GameObject switchpr)
    {
        switches.Add(switchpr);
    }

    public GameObject GetClosest(Vector2 posToCompare, out float dist)
    {
        GameObject ansObj = null;
        dist = Mathf.Infinity;

        float tempDist;
        GameObject tempObj = GetClosestBox(posToCompare, out tempDist);

        if (tempDist < dist)
        {
            dist = tempDist;
            ansObj = tempObj;
        }

        tempObj = GetClosestPickable(posToCompare, out tempDist);

        if (tempDist < dist)
        {
            dist = tempDist;
            ansObj = tempObj;
        }
        return ansObj;
    }

    public GameObject GetClosestBox(Vector2 posToCompare, out float dist)
    {
        float minDist = Mathf.Infinity;
        GameObject ansObj = null;
        foreach (GameObject ob in boxes)
        {
            if ((new Vector2(ob.transform.position.x, ob.transform.position.y) - posToCompare).sqrMagnitude < minDist)
            {
                minDist = (new Vector2(ob.transform.position.x, ob.transform.position.y) - posToCompare).sqrMagnitude;
                ansObj = ob;
            }
        }
        dist = minDist;
        return ansObj;
    }

    public GameObject GetClosestSwitch(Vector2 posToCompare, out float dist)
    {
        float minDist = Mathf.Infinity;
        GameObject ansObj = null;
        foreach (GameObject ob in switches)
        {
            if ((new Vector2(ob.transform.position.x, ob.transform.position.y) - posToCompare).sqrMagnitude < minDist)
            {
                minDist = (new Vector2(ob.transform.position.x, ob.transform.position.y) - posToCompare).sqrMagnitude;
                ansObj = ob;
            }
        }
        dist = minDist;
        return ansObj;
    }
    public GameObject GetClosestPickable(Vector2 posToCompare, out float dist)
    {
        float minDist = Mathf.Infinity;
        GameObject ansObj = null;
        foreach (GameObject ob in pickableObjs)
        {
            if ((new Vector2(ob.transform.position.x, ob.transform.position.y) - posToCompare).sqrMagnitude < minDist)
            {
                minDist = (new Vector2(ob.transform.position.x, ob.transform.position.y) - posToCompare).sqrMagnitude;
                ansObj = ob;
            }
        }
        dist = minDist;
        return ansObj;
    }
}
