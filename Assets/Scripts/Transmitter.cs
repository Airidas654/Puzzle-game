using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transmitter : MonoBehaviour
{
    [SerializeField] List<Receiver> receivers;
    [SerializeField] BasicColorsenum transmitionColor;

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
        GetComponent<SpriteRenderer>().color = BasicColors.GetColorFromEnum(transmitionColor);
        foreach (Receiver i in receivers)
        {
            if (i != null)
            {
                i.color = transmitionColor;
                i.OnValidate();
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
