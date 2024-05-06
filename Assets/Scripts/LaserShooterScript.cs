using System.Collections.Generic;
using UnityEngine;

public class LaserShooterScript : LogicObject
{
    List<GameObject> list = new List<GameObject>();
    [SerializeField] private float maxRayDistance = 15000;
    [SerializeField] float raySpread = 0;
    [SerializeField] LayerMask layerMask;
    [SerializeField] LayerMask playerMask;
    UnityEngine.Pool.ObjectPool<GameObject> pool;
    public GameObject laserBeam;
    public int reflections;

    GameObject lastReceiver = null;

    ParticleSystem particleSystemt;

    void ClearLasers()
    {
        foreach (GameObject go in list)
        {
            pool.Release(go);
        }
        list.Clear();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Vector2 a = (Vector2)transform.position + Vector2.Perpendicular(transform.right) * raySpread / 2;
        Vector2 b = (Vector2)transform.position - Vector2.Perpendicular(transform.right) * raySpread / 2;
        Gizmos.DrawLine(a, b);
    }

    protected override void Start()
    {
        pool = new UnityEngine.Pool.ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject);
        particleSystemt = transform.GetChild(0).GetComponent<ParticleSystem>();
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

    bool particlesPlaying = false;
    Vector2 lastHitPoint;
    Vector2 lastDir;
    void SetParticles(bool val)
    {
        particleSystemt.transform.position = lastHitPoint;
        particleSystemt.transform.rotation = Quaternion.Euler(0,0,Mathf.Atan2(-lastDir.y, -lastDir.x)*Mathf.Rad2Deg);
        if (val && !particlesPlaying)
        {
            particlesPlaying = true;
            particleSystemt.Play(true);
        }else if (!val && particlesPlaying)
        {
            particlesPlaying= false;
            particleSystemt.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }

    void ShootLaser(int reflectionsLeft, Vector2 origin, Vector2 dir)
    {
        if (reflectionsLeft == 0) { return; }

        RaycastHit2D hit = Physics2D.Raycast(origin, dir, maxRayDistance, layerMask);

        float dist = hit.collider == null ? maxRayDistance : hit.distance;

        

        if (dist > Mathf.Epsilon)
        {
            Draw2DRay(origin, origin + dir * dist);
        }
        GameObject tempReceiver = null;
        bool isLast = true;
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Mirror"))
            {
                
                Vector2 norm = (hit.point - (Vector2)hit.collider.transform.position).normalized;

                //Debug.DrawRay(hit.point, Vector2.Reflect(dir, norm), Color.blue, 1);

                ShootLaser(reflectionsLeft - 1, hit.point-dir*0.001f, Vector2.Reflect(dir, norm));
                isLast = false;
                
            }
            else if (hit.collider.CompareTag("LaserReceiver"))
            {
                tempReceiver = hit.collider.gameObject;
            }
        }

        if (isLast)
        {
            if (hit.collider != null)
            {
                lastHitPoint = hit.point;
                lastDir = dir;
                SetParticles(true);
            }
            else
            {
                SetParticles(false);
            }
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

        Vector2 norm = (hitPos - startPos) / dis;

        float rot = Mathf.Atan2(norm.y, norm.x) * Mathf.Rad2Deg;
        Quaternion quat = Quaternion.Euler(0, 0, rot);

        GameObject laser = pool.Get();
        list.Add(laser);

        laser.transform.position = middle;

        laser.transform.rotation = quat;

        laser.GetComponent<SpriteRenderer>().size = new Vector2(dis, 0.5f);

        
        RaycastHit2D hit = Physics2D.BoxCast(middle, new Vector2(dis, raySpread), rot, Vector2.zero, 0.1f, playerMask);
        
        if (hit.collider != null)
        {
            GameManager.inst.Death();
        }
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
