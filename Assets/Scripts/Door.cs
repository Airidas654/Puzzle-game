using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : Receiver 
{
    [SerializeField] Sprite OpenedDoorSprite;
    [SerializeField] Sprite ClosedDoorSprite;
    bool opened;
    bool goToIncrimentedLevel;
    int exactLevel;


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
            if (goToIncrimentedLevel)
            {
                UImanager.StartLevelTransition(SceneManager.GetActiveScene().buildIndex+1, 0.5f);
            }
            else
            {
                UImanager.StartLevelTransition(exactLevel, 0.5f);
            }
        }
    }

}
