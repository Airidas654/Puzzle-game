using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : LogicObject
{

    [SerializeField] float spikeTime;
    [SerializeField] bool upAndDownDiffrentTimes = false;
    [SerializeField] float upSpikeTime;
    [SerializeField] LayerMask triggerMask;
    public bool isTimed;
    public bool isSpikesUp = false;
    [SerializeField] Sprite outSpikesSprite;
    [SerializeField] Sprite inSpikesSprite;

    [Space(20)]
    [SerializeField] float delay = 0;
    
    [SerializeField] float randomDelayAdd0toX = 0;

    [Space(20)]
    [SerializeField] float blinkTransitionTime = 0.2f;
    [SerializeField] float blinkDurationBeforeOpening = 1f;
    
    float tempTime;
    // Start is called before the first frame update

    BoxCollider2D boxCollider;
    SpriteRenderer spriteRenderer;
    Vector2 baseColliderSize;

    int blockingObjectsCount = 0;
    bool playerOnSpikes = false;
    bool spikesShouldBeOut;

    HashSet<GameObject> obstacles = new HashSet<GameObject>();


    float blinkVal = 0;
    bool isBlinkOn = false;

    void Awake()
    {
        tempDelay = delay + Random.Range(0,randomDelayAdd0toX);
        if (upAndDownDiffrentTimes && isSpikesUp)
        {
            tempTime = upSpikeTime;
        }
        else
        {
            tempTime = spikeTime;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();

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
            SpikesControl(true);
            spikesShouldBeOut = isSpikesUp;
        }
        else
        {
            SpikesControl(isSpikesUp);
        }
    }

    void SetAlpha(float a)
    {
        Color col = spriteRenderer.color;
        col.a = a;
        spriteRenderer.color = col;
    }

    float tempDelay;
    void Update()
    {
        if (tempDelay > 0)
        {
            tempDelay -= Time.deltaTime;
            return;
        }

        if (isTimed && !state)
        {
            tempTime -= Time.deltaTime;
            if (tempTime <= 0)
            {
                SetAlpha(0);
                isBlinkOn = false;

                isSpikesUp = !isSpikesUp;
                SpikesControl(isSpikesUp);
                if (upAndDownDiffrentTimes && isSpikesUp)
                {
                    tempTime = upSpikeTime;
                }
                else {
                    tempTime = spikeTime;
                }
            }
            if (tempTime <= blinkDurationBeforeOpening && isBlinkOn == false && !spikesShouldBeOut)
            {
                isBlinkOn = true;
                SetAlpha(1);
            }
        }

        /*if (isBlinkOn && blinkVal > 0)
        {
            blinkVal -= (1/ blinkTransitionTime) *Time.deltaTime;

            blinkVal = Mathf.Clamp01(blinkVal);

            SetAlpha(1-blinkVal);
        }*/
    }
    public void SpikesControl(bool control)
    {
        if (control)
        {
            
            TrySpikesOn();
        }
        else
        {
            isBlinkOn = false;
            SetAlpha(0);
            SpikesOff();
        }
        spikesShouldBeOut = control;
    }
    public void TrySpikesOn()
    {
        if (blockingObjectsCount > 0)
        {
            isBlinkOn = true;
            SetAlpha(1);
            return;
        }
        spriteRenderer.sprite = outSpikesSprite;
        isBlinkOn = false;
        SetAlpha(0);
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
        spriteRenderer.sprite = inSpikesSprite;
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
