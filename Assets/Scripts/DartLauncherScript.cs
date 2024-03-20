using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Pool;

public class DartLauncherScript : MonoBehaviour
{
    IObjectPool<GameObject> pool;

    public DetectionZone zone;

    public GameObject projectile;

    public Transform spawnLocation;

    public float spawnRotation;

    [SerializeField] float delay = 0.5f;

    private float timer = 0.5f;

    [SerializeField] bool constantShooting = true;
    public float xVariable;
    public float yVariable;
    public float offsetX;
    public float offsetY;

    // Start is called before the first frame update

    void Start()
    {
        pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject);
    }

    GameObject CreatePooledItem()
    {
        GameObject temp = Instantiate(projectile,spawnLocation.position,Quaternion.Euler(0,0,spawnRotation));
        temp.GetComponent<DartScript>().shooter = this;
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

    private void OnValidate()
    {
        if (zone == null) return;
        zone.GetComponent<BoxCollider2D>().size = new Vector2(xVariable, yVariable);
        zone.GetComponent<BoxCollider2D>().offset = new Vector2(offsetX, offsetY);
        zone.transform.SetLocalPositionAndRotation(zone.transform.position, Quaternion.Euler(0, 0, spawnRotation));
        

    }

    // Update is called once per frame
    void Update()
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
