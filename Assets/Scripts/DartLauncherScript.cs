using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartLauncherScript : MonoBehaviour
{
    public DetectionZone zone;

    public GameObject projectile;

    public Transform spawnLocation;

    public float spawnRotation;

    private float delay = 0.5f;

    private float timer = 0.5f;

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
                Instantiate(projectile, spawnLocation.position, Quaternion.Euler(0,0,spawnRotation));
                timer = 0;
            }
        }
        else
        {
            timer = 0.5f;
        }
    }
}
