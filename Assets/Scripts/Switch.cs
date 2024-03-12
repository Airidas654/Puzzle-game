using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : Transmitter
{
    [SerializeField] Sprite SwitchOn;
    [SerializeField] Sprite SwitchOff;
    [SerializeField] bool defaultState;
    private void Start()
    {
        PushableObjectManager.Instance.RegisterSwitch(gameObject);
        transmit(defaultState);
        if (defaultState)
        {
            GetComponent<SpriteRenderer>().sprite = SwitchOn;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = SwitchOff;
        }
    }

    public void Toggle()
    {
        defaultState = !defaultState;
        transmit(defaultState);
        if (defaultState)
        {
            GetComponent<SpriteRenderer>().sprite = SwitchOn;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = SwitchOff;
        }
    }
}
