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
    /// Starts and creates a dart object pool
    /// </summary>
    protected override void Start()
    {
        dartPool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject);
        timer = startDelay;
    }

    /// <summary>
    /// Creates a dart object into the dart pool
    /// </summary>
    /// <returns></returns>
    GameObject CreatePooledItem()
    {
        if (projectile != null && spawnLocation != null)
        {
            GameObject temp = Instantiate(projectile, spawnLocation.position, Quaternion.Euler(0, 0, spawnRotation));
            return temp;
        }
        return null;
    }

    /// <summary>
    /// Returns the pooled dart object into the pool
    /// </summary>
    /// <param name="system"></param>

    void OnReturnedToPool(GameObject system)
    {
        system.gameObject.SetActive(false);
       
    }
    /// <summary>
    /// Gets a dart object from available pool objects
    /// </summary>
    /// <param name="system"></param>
    void OnTakeFromPool(GameObject system)
    {
        system.gameObject.SetActive(true);
        
    }
    /// <summary>
    /// Destroys pooled dart object 
    /// </summary>
    /// <param name="system"></param>
    void OnDestroyPoolObject(GameObject system)
    {
        Destroy(system.gameObject);
    }
    /// <summary>
    /// Takes ant puts dart object into the pool
    /// </summary>
    /// <param name="obj"></param>
    public void RemoveArrow(GameObject obj)
    {
        dartPool.Release(obj);
    }
    /// <summary>
    /// Validates any changes when the script is loaded or changed in the inspector
    /// For the detection zone size changes to be seen
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
   /// Every frame update function
   /// Gets darts from the pool and makes it move
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
                    DartScript dartScript;
                    if (spawnLocation != null && currentProjectile.TryGetComponent<DartScript>(out dartScript))
                    {
                        currentProjectile.transform.position = spawnLocation.position;
                        dartScript.Setup(this, bulletSpeed);
                    }

                    if (SoundManager.Instance != null) {
                        SoundManager.Instance.GetSound("DartShoot").PlayOneShot(); 
                    }
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
                        DartScript dartScript;
                        if (spawnLocation != null && currentProjectile.TryGetComponent<DartScript>(out dartScript))
                        {
                            currentProjectile.transform.position = spawnLocation.position;
                            dartScript.Setup(this, bulletSpeed);
                        }

                        if (SoundManager.Instance != null)
                        {
                            SoundManager.Instance.GetSound("DartShoot").PlayOneShot();
                        }
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
