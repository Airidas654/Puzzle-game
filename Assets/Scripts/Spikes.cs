using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : Receiver
{

    [SerializeField] float spikeTime;
    [SerializeField] LayerMask triggerMask;
    [SerializeField] bool state = false;
    [SerializeField] bool isTimed;
    float tempTime;
    // Start is called before the first frame update
    void Start()
    {
        tempTime = spikeTime;
    }

    // Update is called once per frame
    void Update()
    {
        tempTime -= Time.deltaTime;
        if(tempTime <= 0)
        {
            state = !state;
            tempTime = spikeTime;
        }
    }
    public void SpikesControl(bool control)
    {
        if (control)
        {

        }
    }

    public override void OnStateOn()
    {
        var triggerCollider = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0, triggerMask);
        foreach (var col in triggerCollider)
        {
            if (!col.CompareTag("Player"))
            {
                return;
            }
        }
        if(triggerCollider.Length == 1) {
            Debug.Log("You died");
        }
        var Collider = GetComponent<Collider2D>();
        Collider.enabled = true;


    }
    public override void OnStateOff()
    {
        var Collider = GetComponent<Collider2D>();
        Collider.enabled = false;
    }

}
