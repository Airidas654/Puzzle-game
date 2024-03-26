using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] int levelId;
    Switch trans;

    private void Start()
    {
        trans = GetComponent<Switch>();
    }

    void Update()
    {
        if (trans.defaultState) {
            UImanager.StartLevelTransition(levelId, 0.5f);
        }
    }
}
