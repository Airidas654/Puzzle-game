using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartScript : MonoBehaviour
{
    public float speed = 1f;

    private float timeSinceSpawn = 0f;

    private float despawnTime = 3f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceSpawn += Time.deltaTime;
        transform.position += speed * transform.right * Time.deltaTime;
        if(timeSinceSpawn >= despawnTime)
        {
            Destroy(gameObject);
        }
    }
}
