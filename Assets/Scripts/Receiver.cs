using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receiver : MonoBehaviour
{
    [SerializeField] protected bool state;
    public List<Transmitter> connectedTransmitters;

    private void Start()
    {
        if (state)
        {
            OnStateOn();
        }
        else
        {
            OnStateOff();
        }
    }

    public virtual void OnValidate()
    {
        for(int i = 0;i < connectedTransmitters.Count;i++)
        {
            if (connectedTransmitters[i] != null && !connectedTransmitters[i].receivers.Contains(this))
            {
                connectedTransmitters[i] = connectedTransmitters[connectedTransmitters.Count - 1];
                connectedTransmitters.RemoveAt(connectedTransmitters.Count - 1);
            }
        }
    }

    public virtual void Receive(bool state, Transmitter transmitterId) {
        if (this.state != state)
        {
            if (state)
            {
                OnStateOn();
            }
            else
            {
                OnStateOff();
            }
            this.state = state;
        }
    }

    public virtual void OnStateOn() { }
    public virtual void OnStateOff() { }

}
