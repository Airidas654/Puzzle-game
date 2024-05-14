using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserReceiverScript : LogicObject
{
    [SerializeField] private bool oneTimePower = false;
    [SerializeField] private Sprite receiverOn;
    [SerializeField] private Sprite receiverOff;

    public void SetValue(bool value)
    {
        if (oneTimePower)
        {
         Transmit(true);
            gameObject.GetComponent<SpriteRenderer>().sprite=receiverOn;
        }
        else
        {
            Transmit(value);
			gameObject.GetComponent<SpriteRenderer>().sprite =(value)? receiverOn: receiverOff;
		}
    }
}