using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : LogicObject
{
    [SerializeField] private Sprite SwitchOn;
    [SerializeField] private Sprite SwitchOff;

    protected override void Start()
    {
        base.Start();
        PushableObjectManager.RegisterSwitch(gameObject);
        if (state)
            GetComponent<SpriteRenderer>().sprite = SwitchOn;
        else
            GetComponent<SpriteRenderer>().sprite = SwitchOff;
    }

    public virtual void Toggle()
    {
        state = !state;
        Transmit(state);
        SoundManager.Instance.GetSound("Switch").PlayOneShot();
        if (state)
            GetComponent<SpriteRenderer>().sprite = SwitchOn;
        else
            GetComponent<SpriteRenderer>().sprite = SwitchOff;
    }
}