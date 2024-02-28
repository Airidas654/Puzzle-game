using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receiver : MonoBehaviour
{
    [SerializeField] private bool state;
    [SerializeField] public BasicColorsenum color;

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
        GetComponent<SpriteRenderer>().color = BasicColors.GetColorFromEnum(color);
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
