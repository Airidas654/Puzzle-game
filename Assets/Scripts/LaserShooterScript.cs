using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShooterScript : MonoBehaviour
{
    [SerializeField] private float maxRayDistance = 15;
    public LineRenderer lineRenderer;
    public LayerMask layerMask;
    public int reflections;


    private void Start()
    {
        Physics2D.queriesStartInColliders = false;
    }
    private void Update()
    {
        ShootLaser();
    }

    void ShootLaser()
    {
        lineRenderer.positionCount = 1;
        Draw2DRay(0, transform.position);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, maxRayDistance, layerMask);

        bool isMirror = false;
        Vector2 mirrorHitPoint = Vector2.zero;
        Vector2 mirrorHitNormal = Vector2.zero;

        for(int i = 0; i < reflections; i++)
        {
            lineRenderer.positionCount += 1;

            if(hit.collider != null)
            {
                Draw2DRay(lineRenderer.positionCount - 1, hit.point);
                isMirror = false;
                if (hit.collider.CompareTag("Mirror"))
                {
                    mirrorHitPoint = (Vector2)hit.point;
                    mirrorHitNormal = (Vector2)hit.normal;
                    hit = Physics2D.Raycast((Vector2)hit.point, Vector2.Reflect(hit.point, hit.normal), maxRayDistance, layerMask);
                    isMirror = true;
                }
                else
                {
                    break;
                }
            }
            else
            {
                if (isMirror)
                {
                    Draw2DRay(lineRenderer.positionCount - 1, Vector2.Reflect(mirrorHitPoint, mirrorHitNormal) * maxRayDistance);
                    break;
                }
                else
                {
                    Draw2DRay(lineRenderer.positionCount - 1, transform.position + transform.right*maxRayDistance);
                    break;
                }
            }

        }
    }
    void Draw2DRay(int index, Vector2 pos)
    {
        lineRenderer.SetPosition(index, pos);
        
    }
}
