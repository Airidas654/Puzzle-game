using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform obj;

    [SerializeField] float maxDist;
    [SerializeField] AnimationCurve followCurve;
    [SerializeField] float speed;
    [Space(20)]
    [SerializeField] bool usePlayerDir;
    [SerializeField] float forwardMult;

    [Space(20)]
    [SerializeField] bool useBoundingBox;
    [SerializeField] Vector2 boundingBoxSize;
    [SerializeField] Vector2 boundingBoxOffset;

    [SerializeField] bool freezeX;
    [SerializeField] bool freezeY;

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


    void Start()
    {
        if (usePlayerDir)
        {

        }
    }

    void FixedUpdate()
    {
        Vector2 thisPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 objPos = new Vector2(obj.position.x, obj.position.y);

        if (usePlayerDir)
        {
            PlayerMovement playerMovement;
            if (obj.TryGetComponent(out playerMovement))
            {
                objPos += playerMovement.moveDir * forwardMult;
            }
        }

        Vector2 newPos = transform.position;

        if (useBoundingBox)
        {
            float camSize = Camera.main.orthographicSize;
            float xMult = Screen.width / (float)Screen.height;


            objPos.y = Mathf.Max(objPos.y, boundingBoxOffset.y - boundingBoxSize.y / 2 + camSize);
            objPos.y = Mathf.Min(objPos.y, boundingBoxOffset.y + boundingBoxSize.y / 2 - camSize);

            objPos.x = Mathf.Max(objPos.x, boundingBoxOffset.x - boundingBoxSize.x / 2 + camSize * xMult);
            objPos.x = Mathf.Min(objPos.x, boundingBoxOffset.x + boundingBoxSize.x / 2 - camSize * xMult);
        }

        float dist = (thisPos - objPos).magnitude;

        if (dist > 0)
        {
            if (dist > maxDist)
            {
                newPos = objPos + (thisPos - objPos) / dist * maxDist;
            }
            else
            {
                newPos = thisPos + (objPos - thisPos) / dist * Mathf.Min(dist, followCurve.Evaluate(dist/maxDist) * speed * Time.fixedDeltaTime);
            }
        }

        if (freezeX) newPos.x = transform.position.x;
        if (freezeY) newPos.y = transform.position.y;

        transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
    }
}
