using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    [SerializeField] private float messageDist;
    [SerializeField] private float timeToDisappear;


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, messageDist);
    }

    private void Update()
    {
        var hit = Physics2D.OverlapCircleAll(transform.position, messageDist);
        foreach (var h in hit)
        {
            LevelSelect ls;
            if (h.TryGetComponent(out ls)) ls.SetTimer(timeToDisappear);
        }
    }
}