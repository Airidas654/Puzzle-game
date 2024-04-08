using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PushableObjectManager
{
    static public List<GameObject> boxes = new List<GameObject>();
    static public List<GameObject> switches = new List<GameObject>();
    static public List<GameObject> pickableObjs = new List<GameObject>();

    public static void RegisterPickable(GameObject pickable)
    {
        pickableObjs.Add(pickable);
    }
    public static void RegisterBox(GameObject box)
    {
        boxes.Add(box);
    }

    public static void RegisterSwitch(GameObject switchpr)
    {
        switches.Add(switchpr);
    }

    public static GameObject GetClosestBox(Vector2 posToCompare, out float dist)
    {
        float minDist = Mathf.Infinity;
        GameObject ansObj = null;
        foreach (GameObject ob in boxes)
        {
            //Debug.Log((new Vector2(ob.transform.position.x, ob.transform.position.y) - posToCompare).sqrMagnitude);
            if ((new Vector2(ob.transform.position.x, ob.transform.position.y) - posToCompare).sqrMagnitude < minDist)
            {
                minDist = (new Vector2(ob.transform.position.x, ob.transform.position.y) - posToCompare).sqrMagnitude;
                ansObj = ob;
            }
        }
        if (minDist != Mathf.Infinity)
        {
            dist = Mathf.Sqrt(minDist);
        }
        else
        {
            dist = minDist;
        }
        
        return ansObj;
    }

    public static GameObject GetClosestSwitch(Vector2 posToCompare, out float dist)
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
        if (minDist != Mathf.Infinity)
        {
            dist = Mathf.Sqrt(minDist);
        }
        else
        {
            dist = minDist;
        }
        return ansObj;
    }
    public static GameObject GetClosestPickable(Vector2 posToCompare, out float dist)
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
        if (minDist != Mathf.Infinity)
        {
            dist = Mathf.Sqrt(minDist);
        }
        else
        {
            dist = minDist;
        }
        return ansObj;
    }
}
