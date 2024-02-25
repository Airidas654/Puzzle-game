using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receiver : MonoBehaviour
{
    private bool currState;
    private void Awake()
    {
        currState = false;
    }

    public virtual void Receive(bool state) {
        if (state != currState)
        {
            if (state)
            {
                OnStateOn();
            }
            else
            {
                OnStateOff();
            }
            currState = state;
        }
    }

    public virtual void OnStateOn() { }
    public virtual void OnStateOff() { }

}
