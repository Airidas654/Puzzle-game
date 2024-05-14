using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SecretLevel : MonoBehaviour
{
    CameraFollow cameraFollow;
    GameObject player;

    [SerializeField] Vector2 secretWallPoint;
    [SerializeField] float distance;
    [SerializeField] float minDist;
    [SerializeField] Transform secretObjParent;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(secretWallPoint, distance);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(secretWallPoint, minDist);
    }
    void Start()
    {
        cameraFollow = GetComponent<CameraFollow>();
        player = PlayerMovement.currPlayer;
    }

    void SetTransparency(float val)
    {
        for (int i = 0; i < secretObjParent.childCount; i++)
        {
            SpriteRenderer spriteRenderer;
            if (secretObjParent.GetChild(i).TryGetComponent(out spriteRenderer))
            {
                Color col = spriteRenderer.color;
                col.a = val;
                spriteRenderer.color = col;
            }
        }
    }

    void Update()
    {
        float dist = ((Vector2)player.transform.position - secretWallPoint).magnitude;

        

        float val = Mathf.Max(0,Mathf.Min(1, Mathf.InverseLerp(minDist, distance, dist)));

        SetTransparency(val);
        Camera.main.transform.position = new Vector3(-2.5f*(1-val), 0,-10);

    }
}
