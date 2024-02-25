using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transmitter : MonoBehaviour
{
    [SerializeField] List<Receiver> receivers;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        foreach(Receiver i in receivers)
        {
            Gizmos.DrawLine(transform.position, i.gameObject.transform.position);
        }
    }


    public virtual void transmit(bool state)
    {
        foreach(Receiver i in receivers)
        {
            i.Receive(state);
        }
    }
}
