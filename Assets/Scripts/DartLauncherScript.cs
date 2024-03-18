using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartLauncherScript : MonoBehaviour
{
    public DetectionZone zone;

    public GameObject projectile;

    public Transform spawnLocation;

    public Quaternion spawnRotation;

    private float delay = 1f;

    private float timer = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (zone.detectedObjects.Count > 0)
        {
            timer += Time.deltaTime;
            if (timer >= delay)
            {
                Instantiate(projectile, spawnLocation.position, spawnRotation);
                timer = 0;
            }
        }
        else
        {
            timer = 0.5f;
        }
    }
}
