using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CantMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Update()
    {
        GetComponent<PlayerMovement>().StopMovement();
    }

}
