using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager inst;
    public bool dead { get; private set; }
    public bool CantDoAnything;

    public bool levelEnterFreeze { get; private set; }

    void UnfreezeLevel()
    {
        levelEnterFreeze = false;
    }

    private void Awake()
    {
        dead = false;
        levelEnterFreeze = true;
        if (inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(this);
        }
        CantDoAnything = false;
        DOVirtual.DelayedCall(1.5f, UnfreezeLevel);
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
        if (!CantDoAnything && !levelEnterFreeze && !dead)
        {
            if (PlayerMovement.input.Player.Restart.WasPressedThisFrame())
            {
                Death();
            }
        }
    }

}
