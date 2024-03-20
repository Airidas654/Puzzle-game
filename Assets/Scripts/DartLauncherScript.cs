using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class DartLauncherScript : MonoBehaviour
{
    public Queue<GameObject> queuedObjects;

    public DetectionZone zone;

    public GameObject projectile;

    public Transform spawnLocation;

    public float spawnRotation;

    private float delay = 0.5f;

    private float timer = 0.5f;

    [SerializeField] bool constantShooting = true;

    private float despawnTimer = 0f;
    private DartScript dartScript;

    // Start is called before the first frame update

    void Start()
    {
        queuedObjects = new Queue<GameObject>();
        dartScript = projectile.GetComponent<DartScript>();
        float maxObjects = dartScript.despawnTime / delay;
        for(int i = 0; i < maxObjects; i++)
        {
            GameObject InstantiatedProjectile = Instantiate(projectile, spawnLocation.position, Quaternion.Euler(0, 0, spawnRotation));
            queuedObjects.Enqueue(InstantiatedProjectile);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (constantShooting)
        {
            timer += Time.deltaTime;
            despawnTimer += Time.deltaTime;
            if (timer >= delay)
            {
                GameObject currentProjectile = queuedObjects.Dequeue();
                timer = 0;
                
            }
        }
        else
        {
            zone.transform.SetLocalPositionAndRotation(zone.transform.position, Quaternion.Euler(0,0,spawnRotation));
            if (zone.detectedObjects.Count > 0)
            {
                timer += Time.deltaTime;
                despawnTimer += Time.deltaTime;
                if (timer >= delay)
                {
                    GameObject currentProjectile = queuedObjects.Dequeue();
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
