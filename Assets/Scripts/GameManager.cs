using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager inst;
    bool dead = false;

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
        if (Input.GetKeyDown(KeyCode.R))
        {
            Death();
        }
    }

}
