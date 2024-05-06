using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    private void Start()
    {
        PushableObjectManager.RegisterBox(gameObject);
    }
}