using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Pool;
using System;

public class DartLauncherScript : LogicObject
{
    IObjectPool<GameObject> dartPool;

	[SerializeField] DetectionZone zone;

	[SerializeField] GameObject projectile;

	[SerializeField] Transform spawnLocation;

    [SerializeField] float spawnRotation;

    [SerializeField] float startDelay = 0f;
    [SerializeField] float delay = 0.5f;
    [SerializeField] float bulletSpeed = 5f; 

    private float timer = 0.5f;

    

    [SerializeField] bool constantShooting;
    [SerializeField] float zoneSizeX;
    [SerializeField] float zoneSizeY;
    [SerializeField] float zoneOffsetX;
    [SerializeField] float zoneOffsetY;

    // Start is called before the first frame update

    /// <summary>
    /// 
    /// </summary>
    protected override void Start()
    {
        dartPool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject);
        timer = startDelay + delay;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    GameObject CreatePooledItem()
    {
        GameObject temp = Instantiate(projectile,spawnLocation.position,Quaternion.Euler(0,0,spawnRotation));
        return temp;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="system"></param>

    void OnReturnedToPool(GameObject system)
    {
        system.gameObject.SetActive(false);
       
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="system"></param>
    void OnTakeFromPool(GameObject system)
    {
        system.gameObject.SetActive(true);
        
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="system"></param>
    void OnDestroyPoolObject(GameObject system)
    {
        Destroy(system.gameObject);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    public void RemoveArrow(GameObject obj)
    {
        dartPool.Release(obj);
    }
    /// <summary>
    /// 
    /// </summary>
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

    

   /// <summary>
   /// 
   /// </summary>
    void Update()
    {
        if (!state)
        {
            if (constantShooting)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    GameObject currentProjectile = dartPool.Get();
                    currentProjectile.transform.position = spawnLocation.position;
                    currentProjectile.GetComponent<DartScript>().Setup(this, bulletSpeed);
                    SoundManager.Instance.GetSound("DartShoot").PlayOneShot();
                    timer = delay;

                }
            }
            else
            {

                if (zone.detectedObjects.Count > 0)
                {
                    timer -= Time.deltaTime;
                    if (timer <= 0)
                    {
                        GameObject currentProjectile = dartPool.Get();
                        currentProjectile.transform.position = spawnLocation.position;
                        currentProjectile.GetComponent<DartScript>().Setup(this, bulletSpeed);
                        SoundManager.Instance.GetSound("DartShoot").PlayOneShot();
                        timer = delay;

                    }

                }
                else
                {
                    timer = 0f;
                }
            }
        }
    }
}
