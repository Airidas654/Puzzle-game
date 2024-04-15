using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicNANDGate : LogicGate
{
    public override void CheckTransmittion()
    {
        state = false;
        foreach (bool tstate in transmittersStates)
        {
            if (!tstate)
            {
                state = true;
                break;
            }
        }
        for (int i = 0; i < receivers.Count; i++)
        {
            receivers[i].Receive(state, connectedTransmitters[0]);
        }
    }
}
