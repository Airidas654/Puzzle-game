using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : LogicObject
{
    [SerializeField] Sprite SwitchOn;
    [SerializeField] Sprite SwitchOff;
    protected override void Start()
    {
        base.Start();
        PushableObjectManager.RegisterSwitch(gameObject);
        if (state)
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
        state = !state;
        Transmit(state);
        if (state)
        {
            GetComponent<SpriteRenderer>().sprite = SwitchOn;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = SwitchOff;
        }
    }
}
