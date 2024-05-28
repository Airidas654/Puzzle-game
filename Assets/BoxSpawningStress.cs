using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawningStress : MonoBehaviour
{
    [SerializeField] Vector2 leftDown;
    [SerializeField] Vector2 rightUp;
    [SerializeField] int boxCount;
    [SerializeField] GameObject boxPrefab;
    [SerializeField] Transform boxParent;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < boxCount; i++)
        {
            Instantiate(boxPrefab, new Vector3(Random.Range(leftDown.x, rightUp.x), Random.Range(leftDown.y, rightUp.y), 0), Quaternion.identity, boxParent);
        }
    }

    
}
