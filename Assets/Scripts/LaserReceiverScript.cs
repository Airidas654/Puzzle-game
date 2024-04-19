using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserReceiverScript : LogicObject
{
    [SerializeField] bool oneTimePower = false;

    public void SetValue(bool value)
    {
        if (oneTimePower) Transmit(true); 
        else Transmit(value);
    }

}
