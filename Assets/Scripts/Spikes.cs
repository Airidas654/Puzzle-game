using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : Receiver
{

    [SerializeField] float spikeTime;
    [SerializeField] LayerMask triggerMask;
    [SerializeField] bool isTimed;
    [SerializeField] bool isSpikesUp = false;
    [SerializeField] Sprite openSpikesSprite;
    [SerializeField] Sprite closedSpikesSprite;
    private bool timerOn = false;
    float tempTime;
    // Start is called before the first frame update
    void Start()
    {
        
        SpikesControl(isSpikesUp);
        
        tempTime = spikeTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTimed && timerOn)
        {
            tempTime -= Time.deltaTime;
            if (tempTime <= 0)
            {
                isSpikesUp = !isSpikesUp;
                SpikesControl(isSpikesUp);
                tempTime = spikeTime;
            }
        }
        
        
    }
    public void SpikesControl(bool control)
    {
        if (control)
        {
            SpikesOn();
            //Debug.Log("Spikehjnhuhyubhyb");
        }
        else
        {
            SpikesOff();
        }
    }
    public void SpikesOn()
    {
        var triggerCollider = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0, triggerMask);
        foreach (var col in triggerCollider)
        {
            if (!col.CompareTag("Player"))
            {
                return;
            }

        }
        GetComponent<SpriteRenderer>().sprite = openSpikesSprite;
        if (triggerCollider.Length == 1)
        {
            Debug.Log("You died");
            return;
        }
        var Collider = GetComponent<Collider2D>();
        Collider.enabled = true;
    }
    public void SpikesOff()
    {
        GetComponent<SpriteRenderer>().sprite = closedSpikesSprite;
        var Collider = GetComponent<Collider2D>();
        Collider.enabled = false;
    }
    public override void OnStateOn()
    {
        if (!isTimed)
        {
            SpikesOn();
        }
        else
        {
            timerOn = true;
        }


    }
    public override void OnStateOff()
    {
        if (!isTimed)
        {
            SpikesOff();
        }
        else
        {
            timerOn = false;
            SpikesOff();
            tempTime = spikeTime;
        }
    }

}
