using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PushableObjectManager
{
    public static List<GameObject> boxes = new();
    public static List<GameObject> switches = new();
    public static List<GameObject> pickableObjs = new();

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
        var minDist = Mathf.Infinity;
        GameObject ansObj = null;
        foreach (var ob in boxes)
            //Debug.Log((new Vector2(ob.transform.position.x, ob.transform.position.y) - posToCompare).sqrMagnitude);
            if ((new Vector2(ob.transform.position.x, ob.transform.position.y) - posToCompare).sqrMagnitude < minDist)
            {
                minDist = (new Vector2(ob.transform.position.x, ob.transform.position.y) - posToCompare).sqrMagnitude;
                ansObj = ob;
            }

        if (minDist != Mathf.Infinity)
            dist = Mathf.Sqrt(minDist);
        else
            dist = minDist;

        return ansObj;
    }

    public static GameObject GetClosestSwitch(Vector2 posToCompare, out float dist)
    {
        var minDist = Mathf.Infinity;
        GameObject ansObj = null;
        foreach (var ob in switches)
            if ((new Vector2(ob.transform.position.x, ob.transform.position.y) - posToCompare).sqrMagnitude < minDist)
            {
                minDist = (new Vector2(ob.transform.position.x, ob.transform.position.y) - posToCompare).sqrMagnitude;
                ansObj = ob;
            }

        if (minDist != Mathf.Infinity)
            dist = Mathf.Sqrt(minDist);
        else
            dist = minDist;
        return ansObj;
    }

    public static GameObject GetClosestPickable(Vector2 posToCompare, out float dist)
    {
        var minDist = Mathf.Infinity;
        GameObject ansObj = null;
        foreach (var ob in pickableObjs)
            if ((new Vector2(ob.transform.position.x, ob.transform.position.y) - posToCompare).sqrMagnitude < minDist)
            {
                minDist = (new Vector2(ob.transform.position.x, ob.transform.position.y) - posToCompare).sqrMagnitude;
                ansObj = ob;
            }

        if (minDist != Mathf.Infinity)
            dist = Mathf.Sqrt(minDist);
        else
            dist = minDist;
        return ansObj;
    }
}