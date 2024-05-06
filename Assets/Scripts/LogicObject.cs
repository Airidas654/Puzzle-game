using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicObject : MonoBehaviour
{
    public bool state;
    public List<LogicObject> receivers;

    public Dictionary<LogicObject, bool> transmittersStates = new();


    protected virtual void Start()
    {
        UpdateReceivers();
    }

    public void UpdateReceivers()
    {
        foreach (var lo in receivers)
            if (lo != null)
            {
                lo.Receive(state, this);
                Transmit(state);
            }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        foreach (var i in receivers)
            if (i != null)
                Gizmos.DrawLine(transform.position, i.gameObject.transform.position);
    }

    public virtual void Receive(bool state, LogicObject transmitter)
    {
        if (this.state != state)
        {
            transmittersStates[transmitter] = state;
            if (state)
                OnStateOn();
            else
                OnStateOff();
            this.state = state;
        }

        OnReceived();
    }

    public virtual void OnReceived()
    {
    }

    public virtual void OnStateOn()
    {
    }

    public virtual void OnStateOff()
    {
    }

    public virtual void Transmit(bool state)
    {
        foreach (var i in receivers)
            if (i != null)
                i.Receive(state, this);
    }
}