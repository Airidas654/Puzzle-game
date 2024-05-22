using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableMirror : MonoBehaviour
{
    [SerializeField] GameObject trailsPrefab;
    [SerializeField] GameObject sliderJointMiddlePrefab;
    [SerializeField] Vector2 trailMinMax;
    [SerializeField] bool yAxis = false;

    [SerializeField] Sprite spriteXTop;
    [SerializeField] Sprite spriteYTop;
    [SerializeField] Sprite spriteXBase;
    [SerializeField] Sprite spriteYBase;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        if (!yAxis)
        {
            Gizmos.DrawLine(transform.position + new Vector3(trailMinMax.x, 0, 0), transform.position + new Vector3(trailMinMax.y, 0, 0));
        }
        else
        {
            Gizmos.DrawLine(transform.position + new Vector3(0,trailMinMax.x, 0), transform.position + new Vector3(0,trailMinMax.y, 0));
        }
    }

    Rigidbody2D rb;
    SliderJoint2D slidJoint;

    private void Start()
    {
        GameObject trailObj = Instantiate(trailsPrefab);
        rb = GetComponent<Rigidbody2D>();

        slidJoint = GetComponent<SliderJoint2D>();

        float dist = Mathf.Abs(trailMinMax.x - trailMinMax.y);

        JointTranslationLimits2D lim = new JointTranslationLimits2D();
        lim.min = -dist / 2;
        lim.max = dist / 2;

        slidJoint.limits = lim;

        GameObject middleObj = Instantiate(sliderJointMiddlePrefab);

        if (!yAxis)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

            Vector3 pos1 = transform.position + new Vector3(trailMinMax.x, 0, 0);
            Vector3 pos2 = transform.position + new Vector3(trailMinMax.y, 0, 0);

            GetComponent<SpriteRenderer>().sprite = spriteXBase;

            BoxCollider2D boxColl = GetComponent<BoxCollider2D>();
            boxColl.size = new Vector2(0.94f, 0.4713664f);
            boxColl.offset = new Vector2(0, 0);
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = spriteXTop;
            transform.GetChild(0).GetComponent<CircleCollider2D>().offset = new Vector2(0, 0.3f);

            trailObj.transform.position = (pos1 + pos2) / 2;
            trailObj.transform.rotation = Quaternion.Euler(0, 0, 0);
            trailObj.GetComponent<SpriteRenderer>().size = new Vector2(Mathf.Abs(trailMinMax.x-trailMinMax.y)+1,0.5f);

            middleObj.transform.position = new Vector3(transform.position.x + (trailMinMax.x + trailMinMax.y) / 2, transform.position.y, transform.position.z);
            slidJoint.angle = 0;
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

            Vector3 pos1 = transform.position + new Vector3(0, trailMinMax.x, 0);
            Vector3 pos2 = transform.position + new Vector3(0, trailMinMax.y, 0);

            GetComponent<SpriteRenderer>().sprite = spriteYBase;

            BoxCollider2D boxColl = GetComponent<BoxCollider2D>();
            boxColl.size = new Vector2(0.625f, 0.88f);
            boxColl.offset = new Vector2(0, -0.03f);
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = spriteYTop;
            transform.GetChild(0).GetComponent<CircleCollider2D>().offset = new Vector2(0,0.06f);

            trailObj.transform.position = (pos1 + pos2) / 2;
            trailObj.transform.rotation = Quaternion.Euler(0, 0, 90);
            trailObj.GetComponent<SpriteRenderer>().size = new Vector2(Mathf.Abs(trailMinMax.x - trailMinMax.y)+1, 0.5f);

            middleObj.transform.position = new Vector3(transform.position.x, transform.position.y+(trailMinMax.x + trailMinMax.y) / 2, transform.position.z);
            slidJoint.angle = 90;
        }
        slidJoint.connectedBody = middleObj.GetComponent<Rigidbody2D>();

        PushableObjectManager.RegisterBox(gameObject);
    }
}
