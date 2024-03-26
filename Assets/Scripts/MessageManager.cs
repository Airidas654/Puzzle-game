using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    [SerializeField] float messageDist;
    [SerializeField] float timeToDisappear;
    
    

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,messageDist);
    }

    void Update()
    {
        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, messageDist);
        foreach(Collider2D h in hit)
        {
            LevelSelect ls;
            if (h.TryGetComponent(out ls))
            {
                ls.SetTimer(timeToDisappear);
            }
        }
    }
}
