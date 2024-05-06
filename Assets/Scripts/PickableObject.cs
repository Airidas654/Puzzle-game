using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    private void Start()
    {
        PushableObjectManager.RegisterPickable(gameObject);
    }
}