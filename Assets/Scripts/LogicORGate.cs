using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicORGate : LogicObject
{
    public override void Receive(bool state, LogicObject transmitter)
    {
        bool tempVal;
        if (!transmittersStates.TryGetValue(transmitter, out tempVal) || tempVal != state)
        {
            transmittersStates[transmitter] = state;
            this.state = false;
            foreach (var tstate in transmittersStates)
                if (tstate.Value)
                {
                    this.state = true;
                    break;
                }

            Transmit(this.state);
        }
    }
}