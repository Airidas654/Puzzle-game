using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using DG.Tweening;

public class DartScript : MonoBehaviour
{
    float speed = 1f;
    [SerializeField] LayerMask ignoreCollisions = 0;

    Rigidbody2D rg;

    bool oneTime = false;
    public void Setup(DartLauncherScript shooter, float bulletSpeed)
    {
        speed = bulletSpeed;
        rg = GetComponent<Rigidbody2D>();
        this.shooter = shooter;
        rg.velocity = transform.right * speed;
        transform.localScale = Vector3.one;
        oneTime = true;
        
    }
    DartLauncherScript shooter;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("ya");
        if (ignoreCollisions == (ignoreCollisions | (1 << collision.gameObject.layer))) return;
        if (!gameObject.activeSelf || collision.gameObject == shooter.gameObject || (collision.transform.parent!=null && collision.transform.parent.gameObject == shooter.gameObject)) return;

        if (!oneTime) return;
        oneTime = false;
        //shooter.RemoveArrow(gameObject);
        Collided();
        if(collision.tag == "Player")
        {
            GameManager.inst.Death();
        }
    }

    void Collided()
    {
        transform.DOScale(new Vector3(1-Mathf.Abs(transform.right.x), 1- Mathf.Abs(transform.right.y),1), 0.5f/speed).OnComplete(()=>shooter.RemoveArrow(gameObject));
    }

    private void Update()
    {
        Debug.Log("ya2");
    }
}
