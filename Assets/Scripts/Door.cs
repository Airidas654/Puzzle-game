using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Receiver 
{
    [SerializeField] Sprite OpenedDoorSprite;
    [SerializeField] Sprite ClosedDoorSprite;
    bool opened;


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

    public override void OnStateOn()
    {
        GetComponent<SpriteRenderer>().sprite = OpenedDoorSprite;
        opened = true;
    }

    public override void OnStateOff()
    {
        GetComponent<SpriteRenderer>().sprite = ClosedDoorSprite;
        opened = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && opened)
        {
            Debug.Log("Next Level!!");
        }
    }

}
