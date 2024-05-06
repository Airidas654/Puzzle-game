using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform obj;

    [SerializeField] private float maxDist;
    [SerializeField] private AnimationCurve followCurve;
    [SerializeField] private float speed;
    [Space(20)] [SerializeField] private bool usePlayerDir;
    [SerializeField] private float forwardMult;

    [Space(20)] [SerializeField] private bool useBoundingBox;
    [SerializeField] private Vector2 boundingBoxSize;
    [SerializeField] private Vector2 boundingBoxOffset;

    [SerializeField] private bool freezeX;
    [SerializeField] private bool freezeY;

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


    private void Start()
    {
        if (usePlayerDir)
        {
        }
    }

    private void FixedUpdate()
    {
        var thisPos = new Vector2(transform.position.x, transform.position.y);
        var objPos = new Vector2(obj.position.x, obj.position.y);

        if (usePlayerDir)
        {
            PlayerMovement playerMovement;
            if (obj.TryGetComponent(out playerMovement)) objPos += playerMovement.moveDir * forwardMult;
        }

        Vector2 newPos = transform.position;

        if (useBoundingBox)
        {
            var camSize = Camera.main.orthographicSize;
            var xMult = Screen.width / (float)Screen.height;


            objPos.y = Mathf.Max(objPos.y, boundingBoxOffset.y - boundingBoxSize.y / 2 + camSize);
            objPos.y = Mathf.Min(objPos.y, boundingBoxOffset.y + boundingBoxSize.y / 2 - camSize);

            objPos.x = Mathf.Max(objPos.x, boundingBoxOffset.x - boundingBoxSize.x / 2 + camSize * xMult);
            objPos.x = Mathf.Min(objPos.x, boundingBoxOffset.x + boundingBoxSize.x / 2 - camSize * xMult);
        }

        var dist = (thisPos - objPos).magnitude;

        if (dist > 0)
        {
            if (dist > maxDist)
                newPos = objPos + (thisPos - objPos) / dist * maxDist;
            else
                newPos = thisPos + (objPos - thisPos) / dist * Mathf.Min(dist,
                    followCurve.Evaluate(dist / maxDist) * speed * Time.fixedDeltaTime);
        }

        if (freezeX) newPos.x = transform.position.x;
        if (freezeY) newPos.y = transform.position.y;

        transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
    }
}