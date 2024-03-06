using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    void Start()
    {
        PushableObjectManager.Instance.RegisterPickable(gameObject);
    }
}
