using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField] private Sprite OpenedDoorSprite;
    [SerializeField] private Sprite ClosedDoorSprite;
    [SerializeField] private Vector2 openingColliderOffset;
    [SerializeField] private float lengthToOpen;
    [SerializeField] private LayerMask layersToOpen;
    public bool goToIncrimentedLevel;
    public int exactLevel;
    private bool opened;

    private void Start()
    {
        opened = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + openingColliderOffset, lengthToOpen);
    }

    private void Open()
    {
        GetComponent<SpriteRenderer>().sprite = OpenedDoorSprite;
    }

    private void Close()
    {
        GetComponent<SpriteRenderer>().sprite = ClosedDoorSprite;
    }

    private void Update()
    {
        var hit = Physics2D.OverlapCircle((Vector2)transform.position + openingColliderOffset, lengthToOpen,
            layersToOpen);
        if (hit != null && !opened)
        {
            Open();
            opened = true;
        }
        else if (hit == null && opened)
        {
            Close();
            opened = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (goToIncrimentedLevel)
                UImanager.StartLevelTransition(SceneManager.GetActiveScene().buildIndex + 1, 0.5f);
            else
                UImanager.StartLevelTransition(exactLevel, 0.5f);
        }
    }
}