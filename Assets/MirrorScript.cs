using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorScript : MonoBehaviour
{
    [SerializeField] Sprite UpperDiagonalMirrorSprite;
    [SerializeField] Sprite LowerDiagonalMirrorSprite;
    float timer;
    int index = 0;
    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<SpriteRenderer>().sprite = default;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > 4f && index%2==0)
        {
            timer = 0;
            GetComponent<SpriteRenderer>().sprite = UpperDiagonalMirrorSprite;
            index++;
        }
        if (timer > 4f && index % 2 == 1)
        {
            timer = 0;
            GetComponent<SpriteRenderer>().sprite = LowerDiagonalMirrorSprite;
            index++;
        }

    }
}
