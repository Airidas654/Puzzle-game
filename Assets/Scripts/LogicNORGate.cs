using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicNORGate : LogicGate
{
    public override void CheckTransmittion()
    {
        if (connectedTransmitters.Count > 0)
        {
            state = true;
            foreach (bool tstate in transmittersStates)
            {
                if (tstate)
                {
                    state = false;
                    break;
                }
            }
            for (int i = 0; i < receivers.Count; i++)
            {
                receivers[i].Receive(state, connectedTransmitters[0]);
            }
        }
    }
}
