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

    public void RegisterBox(GameObject box)
    {
        boxes.Add(box);
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
}
