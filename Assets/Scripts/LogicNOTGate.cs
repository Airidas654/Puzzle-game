using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicNOTGate : LogicObject
{
    public override void Receive(bool state, LogicObject transmitter)
    {
        if (this.state != !state)
        {
            this.state = !state;
            transmittersStates[transmitter] = this.state;
            Transmit(this.state);
        }
    }
}