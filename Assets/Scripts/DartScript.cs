using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class DartScript : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [System.NonSerialized] public DartLauncherScript shooter;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!gameObject.activeSelf || collision.gameObject == shooter.gameObject || (collision.transform.parent!=null && collision.transform.parent.gameObject == shooter.gameObject)) return;
        shooter.RemoveArrow(gameObject);
        if(collision.tag == "Player")
        {
            GameManager.inst.Death();
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += speed * Time.deltaTime * transform.right;
    }
}
