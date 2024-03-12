using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    
    void Start()
    {
        PushableObjectManager.Instance.RegisterBox(gameObject);
    }
}
