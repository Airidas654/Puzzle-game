using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transmitter : MonoBehaviour
{
    public List<Receiver> receivers;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        foreach(Receiver i in receivers)
        {
            if (i != null)
            {
                Gizmos.DrawLine(transform.position, i.gameObject.transform.position);
            }
        }
    }

    private void OnValidate()
    {
        foreach (Receiver i in receivers)
        {
            if (i != null)
            {
                i.OnValidate();
                if (!i.connectedTransmitters.Contains(this))
                {
                    i.connectedTransmitters.Add(this);
                }
            }
        }
    }

    public virtual void transmit(bool state)
    {
        foreach(Receiver i in receivers)
        {
            if (i != null)
            {
                i.Receive(state, this);
            }
        }
    }
}
