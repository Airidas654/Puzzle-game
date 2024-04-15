using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicNOTGate : LogicGate
{
    public override void CheckTransmittion()
    {
        if (connectedTransmitters.Count > 0)
        {
            state = !transmittersStates[0];
            foreach (Receiver i in receivers)
            {
                i.Receive(!transmittersStates[0], connectedTransmitters[0]);
            }
        }
    }
}
