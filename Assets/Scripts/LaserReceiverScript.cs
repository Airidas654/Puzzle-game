using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserReceiverScript : Transmitter
{
    [SerializeField] bool oneTimePower = false;

    public void SetValue(bool value)
    {
        if (oneTimePower) transmit(true); 
        else transmit(value);
    }

}
