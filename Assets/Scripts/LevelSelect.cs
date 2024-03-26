using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] int levelId;
    [SerializeField] GameObject messagePrefab;
    Switch trans;
    float time;
    GameObject message;

    public void SetTimer(float time)
    {
        this.time = time;
        message.SetActive(true);
    }

    private void Start()
    {
        trans = GetComponent<Switch>();

        message = Instantiate(messagePrefab);
        message.GetComponent<TextMeshProUGUI>().text = string.Format("Level {0}", levelId - 2);
        Camera mCamera = Camera.main;
        message.transform.SetParent(GameObject.Find("Canvas").transform);

        Vector2 adjustedPosition = mCamera.WorldToScreenPoint(transform.position);
        RectTransform mCanvas = GameObject.Find("Canvas").GetComponent<RectTransform>();

        adjustedPosition.x *= mCanvas.rect.width / (float)mCamera.pixelWidth;
        adjustedPosition.y *= mCanvas.rect.height / (float)mCamera.pixelHeight;

        message.GetComponent<RectTransform>().anchoredPosition = adjustedPosition - mCanvas.sizeDelta / 2f;
        message.SetActive(false);
    }

    void Update()
    {
        if (trans.defaultState) {
            UImanager.StartLevelTransition(levelId, 0.5f);
        }
        if(time > 0)
        {
            time -= Time.deltaTime;
            if(time <= 0)
            {
                time = 0;
                message.SetActive(false);
            }
        }
    }
}
