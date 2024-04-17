using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicGate : Receiver
{
    public List<Receiver> receivers;
    
    
    public bool[] transmittersStates;

    private void Start()
    {
        transmittersStates = new bool[connectedTransmitters.Count];
        for(int i = 0;i < connectedTransmitters.Count;i++)
        {
            transmittersStates[i] = false;
        }
        CheckTransmittion();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        foreach (Receiver i in receivers)
        {
            if (i != null)
            {
                Gizmos.DrawLine(transform.position, i.gameObject.transform.position);
            }
        }
    }

    public override void OnValidate() {
        base.OnValidate();
    }

    public override void Receive(bool state, Transmitter transmitterId)
    {
        int index = connectedTransmitters.IndexOf(transmitterId);
        if (index >= 0 && index < transmittersStates.Length)
        {
            transmittersStates[index] = state;
        }
        CheckTransmittion();
    }

    public virtual void CheckTransmittion() { }

}
