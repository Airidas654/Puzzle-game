using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicANDGate : LogicObject
{
    public override void Receive(bool state, LogicObject transmitter)
    {
        bool tempVal;
        if (!transmittersStates.TryGetValue(transmitter, out tempVal) || tempVal != state)
        {
            transmittersStates[transmitter] = state;
            this.state = true;
            foreach (var tstate in transmittersStates)
            {
                if (!tstate.Value)
                {
                    this.state = false;
                    break;
                }
            }
            Transmit(this.state);
        }
    }
}
