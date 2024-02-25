using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform obj;

    [SerializeField] float maxDist;
    [SerializeField] AnimationCurve followCurve;
    [SerializeField] float speed;

    [Space(20)]
    [SerializeField] bool useBoundingBox;
    [SerializeField] Vector2 boundingBoxSize;
    [SerializeField] Vector2 boundingBoxOffset;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(obj.position, maxDist);

        if (useBoundingBox)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(boundingBoxOffset, boundingBoxSize);
        }
    }


    void FixedUpdate()
    {
        Vector2 thisPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 objPos = new Vector2(obj.position.x, obj.position.y);

        float dist = (thisPos - objPos).magnitude;

        Vector2 newPos = transform.position;

        if (dist > 0)
        {



            if (dist > maxDist)
            {
                newPos = objPos + (thisPos - objPos) / dist * maxDist;
            }
            else
            {
                newPos = thisPos + (objPos - thisPos) / dist * Mathf.Min(dist, followCurve.Evaluate(dist / maxDist) * speed * Time.fixedDeltaTime);
            }


        }

        if (useBoundingBox)
        {
            float camSize = Camera.main.orthographicSize;
            float xMult = Screen.width / (float)Screen.height;

            newPos.y = Mathf.Max(newPos.y, boundingBoxOffset.y - boundingBoxSize.y / 2 + camSize);
            newPos.y = Mathf.Min(newPos.y, boundingBoxOffset.y + boundingBoxSize.y / 2 - camSize);

            newPos.x = Mathf.Max(newPos.x, boundingBoxOffset.x - boundingBoxSize.x / 2 + camSize * xMult);
            newPos.x = Mathf.Min(newPos.x, boundingBoxOffset.x + boundingBoxSize.x / 2 - camSize * xMult);
        }

        transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
    }
}
