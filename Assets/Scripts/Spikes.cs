using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : Receiver
{

    [SerializeField] float spikeTime;
    [SerializeField] LayerMask triggerMask;
    public bool isTimed;
    [SerializeField] bool isSpikesUp = false;
    [SerializeField] Sprite outSpikesSprite;
    [SerializeField] Sprite inSpikesSprite;
    
    float tempTime;
    // Start is called before the first frame update

    BoxCollider2D boxCollider;
    Vector2 baseColliderSize;

    int blockingObjectsCount = 0;
    bool playerOnSpikes = false;
    bool spikesShouldBeOut;

    HashSet<GameObject> obstacles = new HashSet<GameObject>();

    void Awake()
    {
        tempTime = spikeTime;

        boxCollider = GetComponent<BoxCollider2D>();
        baseColliderSize = boxCollider.size;

        var triggerCollider = Physics2D.OverlapBoxAll(transform.position, baseColliderSize - new Vector2(0.05f, 0.05f), 0, triggerMask);
        foreach(var i in triggerCollider)
        {
            if (i.gameObject.CompareTag("Player"))
            {
                playerOnSpikes = true;
                continue;
            }
            obstacles.Add(i.gameObject);
        }
        blockingObjectsCount = obstacles.Count;

        spikesShouldBeOut = isSpikesUp;
        if (blockingObjectsCount > 0)
        {
            SpikesControl(false);
            spikesShouldBeOut = isSpikesUp;
        }
        else
        {
            SpikesControl(isSpikesUp);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isTimed && !state)
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
            
            TrySpikesOn();
        }
        else
        {
            SpikesOff();
        }
        spikesShouldBeOut = control;
    }
    public void TrySpikesOn()
    {
        if (blockingObjectsCount > 0)
        {
            
            return;
        }
        GetComponent<SpriteRenderer>().sprite = outSpikesSprite;
        if (playerOnSpikes)
        {
            GameManager.inst.Death();
            return;
        }

        boxCollider.isTrigger = false;
        boxCollider.size = baseColliderSize;
        boxCollider.includeLayers = 0;
    }
    public void SpikesOff()
    {
        GetComponent<SpriteRenderer>().sprite = inSpikesSprite;
        boxCollider.isTrigger = true;
        boxCollider.size = baseColliderSize - new Vector2(0.05f, 0.05f);
        boxCollider.includeLayers = int.MaxValue;
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOnSpikes = true;
            return;
        }
        if (((1<<collision.gameObject.layer) & triggerMask) != 0 && !obstacles.Contains(collision.gameObject))
        {
            obstacles.Add(collision.gameObject);
            blockingObjectsCount++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOnSpikes = false;
            return;
        }
        if (((1 << collision.gameObject.layer) & triggerMask) != 0)
        {
            obstacles.Remove(collision.gameObject);
            blockingObjectsCount--;
        }
        if (spikesShouldBeOut)
            TrySpikesOn();
    }

    public override void OnStateOn()
    {
        
        if (!isTimed)
        {
            SpikesControl(true);
        }
        else
        {
            SpikesControl(false);
            tempTime = spikeTime;
        }
    }
    public override void OnStateOff()
    {
        if (!isTimed)
        {
            SpikesControl(false);
        }
    }

}
