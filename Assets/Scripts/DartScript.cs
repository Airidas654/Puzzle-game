using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using DG.Tweening;

public class DartScript : MonoBehaviour
{
    private float speed = 1f;
    [SerializeField] private LayerMask ignoreCollisions = 0;

    [SerializeField] private LayerMask startCollisionDetection;

    private Rigidbody2D rg;
    private BoxCollider2D col;

    private bool oneTime = false;

    private bool oneTimeInLife = true;
    private float size;

    private void OneTimeSetup()
    {
        if (oneTimeInLife)
        {
            oneTimeInLife = false;
            rg = GetComponent<Rigidbody2D>();
            col = GetComponent<BoxCollider2D>();

            size = col.size.x;
        }
    }

    private HashSet<Collider2D> collidersStartHash = new();

    public void Setup(DartLauncherScript shooter, float bulletSpeed)
    {
        speed = bulletSpeed;
        OneTimeSetup();

        this.shooter = shooter;
        rg.velocity = transform.right * speed;
        transform.localScale = Vector3.one;
        oneTime = true;

        collidersStartHash.Clear();
        var results = Physics2D.BoxCastAll(transform.position, col.size + new Vector2(0.1f, 0.1f), 0,
            new Vector2(0, 1f), 0.001f, startCollisionDetection);
        //Debug.Log(results[0].collider.gameObject.name);
        foreach (var i in results) collidersStartHash.Add(i.collider);
    }

    private DartLauncherScript shooter;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ignoreCollisions == (ignoreCollisions | (1 << collision.gameObject.layer))) return;
        if (!gameObject.activeSelf || collision.gameObject == shooter.gameObject ||
            (collision.transform.parent != null && collision.transform.parent.gameObject == shooter.gameObject)) return;

        var player = collision.tag == "Player";

        if (!player && collidersStartHash.Contains(collision)) return;

        if (!oneTime) return;
        oneTime = false;
        //shooter.RemoveArrow(gameObject);
        Collided();
        if (player) GameManager.inst.Death();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collidersStartHash.Remove(collision);
    }

    private void Collided()
    {
        rg.velocity = Vector2.zero;
        float val = 0;
        Vector2 pradPos = transform.position;
        Vector2 pabPos = transform.position + transform.right * (size / 2);
        DOTween.To(() => val, (x) =>
        {
            val = x;
            transform.localScale = new Vector3(Mathf.Lerp(1, 0, val), 1, 1);
            transform.position = Vector2.Lerp(pradPos, pabPos, val);
        }, 1, size / speed).OnComplete(() => shooter.RemoveArrow(gameObject)).SetId(69);
        //transform.DOScale(new Vector3(0,1,1), size/speed).OnComplete(()=>shooter.RemoveArrow(gameObject));
    }

    private void OnDestroy()
    {
        DOTween.Kill(69);
    }
}