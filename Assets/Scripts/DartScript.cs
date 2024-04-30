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
    BoxCollider2D col;

    bool oneTime = false;

    bool oneTimeInLife = true;
    float size;
    void OneTimeSetup()
    {
        if (oneTimeInLife)
        {
            oneTimeInLife = false;
            rg = GetComponent<Rigidbody2D>();
            col = GetComponent<BoxCollider2D>();

            size = col.size.x;
        }
        
    }

    public void Setup(DartLauncherScript shooter, float bulletSpeed)
    {
        speed = bulletSpeed;
        OneTimeSetup();
        
        this.shooter = shooter;
        rg.velocity = transform.right * speed;
        transform.localScale = Vector3.one;
        oneTime = true;

        RaycastHit2D[] results;
        //col.Cast(new Vector2(0, 0), results);
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
        rg.velocity = Vector2.zero;
        float val=0;
        Vector2 pradPos = transform.position;
        Vector2 pabPos = transform.position + transform.right * (size / 2);
        DOTween.To(()=>val, (x)=> { 
            val = x;
            transform.localScale = new Vector3(Mathf.Lerp(1,0,val),1,1);
            transform.position = Vector2.Lerp(pradPos, pabPos, val);
        }, 1, size/speed).OnComplete(() => shooter.RemoveArrow(gameObject));
        //transform.DOScale(new Vector3(0,1,1), size/speed).OnComplete(()=>shooter.RemoveArrow(gameObject));
    }

    private void Update()
    {
        Debug.Log("ya2");
    }
}
