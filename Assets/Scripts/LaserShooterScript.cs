using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class LaserShooterScript : Receiver
{
    List<GameObject> list = new List<GameObject>();
    [SerializeField] private float maxRayDistance = 15000;
    public LayerMask layerMask;
    ObjectPool<GameObject> pool;
    public GameObject laserBeam;
    public int reflections;

    GameObject lastReceiver = null;

    void ClearLasers()
    {
        foreach (GameObject go in list)
        {
            pool.Release(go);
        }
        list.Clear();
    }

    private void Start()
    {
        pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject);
    }
    private void Update()
    {
        if (!state)
            FirstLaserShot();
        else if (list.Count > 0)
        {
            ClearLasers();
        }
    }
    void FirstLaserShot()
    {
        ClearLasers();
        ShootLaser(reflections, transform.position, transform.right);
    }

    void ShootLaser(int reflectionsLeft, Vector2 origin, Vector2 dir)
    {
        if(reflectionsLeft == 0) { return; }

        RaycastHit2D hit = Physics2D.Raycast(origin, dir, maxRayDistance, layerMask);
        float dist = hit.distance;

        if (hit.collider == null)
        {
            dist = maxRayDistance;
        }
        
        Draw2DRay(origin, origin + dir * dist);
        GameObject tempReceiver = null;
        bool isLast = true;
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Mirror"))
            {
                Vector2 norm = (hit.point - (Vector2)hit.collider.transform.position).normalized;
                ShootLaser(reflectionsLeft - 1, hit.point - dir * 0.001f, Vector2.Reflect(dir, norm));
                isLast = false;
            }else if (hit.collider.CompareTag("LaserReceiver"))
            {
                tempReceiver = hit.collider.gameObject;
            }
        }

        if (isLast)
        {
            if (tempReceiver != lastReceiver)
            {
                if (lastReceiver != null)
                {
                    lastReceiver.GetComponent<LaserReceiverScript>().SetValue(false);
                }
                if (tempReceiver != null)
                {
                    tempReceiver.GetComponent<LaserReceiverScript>().SetValue(true);

                }
                lastReceiver = tempReceiver;
            }
        }

    }
    void Draw2DRay(Vector2 startPos, Vector2 hitPos)
    {
        Vector2 middle = (hitPos - startPos) / 2 + startPos;

        float dis = (startPos - hitPos).magnitude;

        Vector2 norm = (hitPos- startPos)/dis;
        Quaternion rot = Quaternion.Euler(0,0,Mathf.Atan2(norm.y,norm.x)*Mathf.Rad2Deg);

        GameObject laser = pool.Get();
        list.Add(laser);

        laser.transform.position = middle;

        laser.transform.rotation = rot;

        laser.GetComponent<SpriteRenderer>().size = new Vector2(dis, 0.5f);
 
    }





    GameObject CreatePooledItem()
    {
        GameObject temp = Instantiate(laserBeam, Vector2.zero, Quaternion.identity);
        
        return temp;
    }


    void OnReturnedToPool(GameObject system)
    {
        system.gameObject.SetActive(false);
    }
    void OnTakeFromPool(GameObject system)
    {
        system.gameObject.SetActive(true);
    }
    void OnDestroyPoolObject(GameObject system)
    {
        Destroy(system.gameObject);
    }

    public void RemoveLaser(GameObject obj)
    {
        pool.Release(obj);
    }

}
