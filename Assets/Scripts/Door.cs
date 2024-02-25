using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Receiver
{
    public override void OnStateOn()
    {
        gameObject.SetActive(true);
    }
    public override void OnStateOff()
    {
        gameObject.SetActive(false);
    }
}
