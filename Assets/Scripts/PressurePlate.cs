using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : Transmitter
{
    [SerializeField] Vector2 colliderPositionOffset;
    [SerializeField] Vector2 colliderSize;
    public LayerMask colliderMask;

    public Sprite platePressed;
    [SerializeField] Sprite plateUp;

    SpriteRenderer srenderer;

    private void Start()
    {
        srenderer = GetComponent<SpriteRenderer>();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2)transform.position + colliderPositionOffset,colliderSize);
    }


    private void Update()
    {
        Collider2D hit = Physics2D.OverlapBox((Vector2)transform.position + colliderPositionOffset, colliderSize, 0, colliderMask);
        transmit(hit!=null);

        srenderer.sprite = hit==null? plateUp : platePressed;
    }
}
