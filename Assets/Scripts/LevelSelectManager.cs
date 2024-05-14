using System.Collections;
using System.Collections.Generic;
using Codice.Client.BaseCommands;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class LevelSelectManager : MonoBehaviour
{
    
    const int maxLevels = 21;
    public static bool toggled;
    [SerializeField] private List<Transform> levels;
    public static bool[] done;


    public static void Check()
    {
        if (done == null)
        {
            done = new bool[maxLevels];
        }
    }

    public static void DoneLevel(int index)
    {
        Check();

        done[index] = true;
    }


    private void Start()
    {
        toggled = false;
        Check();
        for (int i = 0;i < levels.Count;i++)
        {
            
                if (done[i])
                {
                    levels[i].gameObject.GetComponent<LevelSelect>().NonPlayerToggle();
                }
        }
    }

    private void Update()
    {
        if(PlayerMovement.input.Player.Cheat.WasReleasedThisFrame())
        {
            Check();
            for (int i = 0; i < done.Length; i++)
            {
                done[i] = true;
            }
        }
    }

}
