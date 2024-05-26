using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : LogicObject
{
    [SerializeField] private Vector2 colliderPositionOffset;
    [SerializeField] private Vector2 colliderSize;
    public LayerMask colliderMask;

    public Sprite platePressed;
    [SerializeField] private Sprite plateUp;

    private SpriteRenderer srenderer;
    bool praeitState = false;

    protected override void Start()
    {
        base.Start();
        srenderer = GetComponent<SpriteRenderer>();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2)transform.position + colliderPositionOffset, colliderSize);
    }


    private void Update()
    {
        var hit = Physics2D.OverlapBox((Vector2)transform.position + colliderPositionOffset, colliderSize, 0,
            colliderMask);
        Transmit(hit != null);

        srenderer.sprite = hit == null ? plateUp : platePressed;

        if (praeitState != (hit == null))
        {
            SoundManager.Instance.GetSound("Switch").PlayOneShot();
            praeitState = (hit == null);
        }
    }
}