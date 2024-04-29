using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Pool;
using System;

public class DartLauncherScript : LogicObject
{
    IObjectPool<GameObject> pool;

    public DetectionZone zone;

    public GameObject projectile;

    public Transform spawnLocation;

    public float spawnRotation;

    [SerializeField] float delay = 0.5f;

    private float timer = 0.5f;

    [SerializeField] bool constantShooting;
    public float zoneSizeX;
    public float zoneSizeY;
    public float zoneOffsetX;
    public float zoneOffsetY;

    // Start is called before the first frame update

    protected override void Start()
    {
        pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject);
    }

    GameObject CreatePooledItem()
    {
        GameObject temp = Instantiate(projectile,spawnLocation.position,Quaternion.Euler(0,0,spawnRotation));
        temp.GetComponent<DartScript>().Setup(this);
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

    public void RemoveArrow(GameObject obj)
    {
        pool.Release(obj);
    }

    public void OnValidate()
    {
        if (zone == null) return;
        if (zoneSizeX == 0.5 && zoneSizeY == 0.5)
        {
            zone.GetComponent<BoxCollider2D>().size = new Vector2(zoneSizeX, zoneSizeY);
            zone.GetComponent<BoxCollider2D>().offset = new Vector2(zoneOffsetX, zoneOffsetY);
            zone.transform.SetLocalPositionAndRotation(new Vector3(0, 0, 0), Quaternion.Euler(0, 0, spawnRotation));
        }
        else if (zoneSizeX > 0.5 && zoneSizeY == 0.5)
        {
            zone.GetComponent<BoxCollider2D>().size = new Vector2(zoneSizeX, zoneSizeY);
            zone.GetComponent<BoxCollider2D>().offset = new Vector2((zoneOffsetX+zoneSizeX)/2, zoneOffsetY);
            zone.transform.SetLocalPositionAndRotation(new Vector3(0, 0, 0), Quaternion.Euler(0, 0, spawnRotation));
        }
        else if (zoneSizeY > 0.5 && zoneSizeX == 0.5)
        {
            zone.GetComponent<BoxCollider2D>().size = new Vector2(zoneSizeX, zoneSizeY);
            zone.GetComponent<BoxCollider2D>().offset = new Vector2(zoneOffsetX, (zoneOffsetY+zoneSizeY)/2);
            zone.transform.SetLocalPositionAndRotation(new Vector3(0, 0, 0), Quaternion.Euler(0, 0, spawnRotation));
        }
        else if (zoneSizeX < -0.5 && zoneSizeY == 0.5)
        {
            zone.GetComponent<BoxCollider2D>().size = new Vector2(Math.Abs(zoneSizeX), zoneSizeY);
            zone.GetComponent<BoxCollider2D>().offset = new Vector2((zoneOffsetX+zoneSizeX)/2, zoneOffsetY);
            zone.transform.SetLocalPositionAndRotation(new Vector3(0, 0, 0), Quaternion.Euler(0, 0, spawnRotation));
        }
        else if (zoneSizeX == 0.5 && zoneSizeY < -0.5)
        {
            zone.GetComponent<BoxCollider2D>().size = new Vector2(zoneSizeX, Math.Abs(zoneSizeY));
            zone.GetComponent<BoxCollider2D>().offset = new Vector2(zoneOffsetX, (zoneOffsetY+zoneSizeY)/2);
            zone.transform.SetLocalPositionAndRotation(new Vector3(0, 0, 0), Quaternion.Euler(0, 0, spawnRotation));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!state)
        {
            if (constantShooting)
            {
                timer += Time.deltaTime;
                if (timer >= delay)
                {
                    GameObject currentProjectile = pool.Get();
                    currentProjectile.transform.position = spawnLocation.position;
                    timer = 0;

                }
            }
            else
            {

                if (zone.detectedObjects.Count > 0)
                {
                    timer += Time.deltaTime;
                    if (timer >= delay)
                    {
                        GameObject currentProjectile = pool.Get();
                        currentProjectile.transform.position = spawnLocation.position;
                        timer = 0;

                    }

                }
                else
                {
                    timer = 0.5f;
                }
            }
        }
    }
}
