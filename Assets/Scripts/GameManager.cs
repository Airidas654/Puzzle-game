using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager inst;
    bool dead = false;
    public bool CantDoAnything;

    bool levelEnterFreeze = true;

    void UnfreezeLevel()
    {
        levelEnterFreeze = false;
    }

    private void Awake()
    {
        if (inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(this);
        }
        CantDoAnything = false;
        DOVirtual.DelayedCall(2, UnfreezeLevel);
    }

    public void Death()
    {
        if (!dead)
        {
            dead = true;
            UImanager.StartLevelTransition(SceneManager.GetActiveScene().buildIndex, 0.5f);
            PlayerMovement.currPlayer.GetComponent<Collider2D>().isTrigger = true;
            PlayerMovement.currPlayer.GetComponent<PlayerMovement>().StopMovement();
        }
    }


    private void Update()
    {
        if (!CantDoAnything && !levelEnterFreeze)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Death();
            }
        }
    }

}
