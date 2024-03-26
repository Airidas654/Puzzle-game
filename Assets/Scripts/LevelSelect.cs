using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] int levelId;
    [SerializeField] GameObject messagePrefab;
    [SerializeField] Vector2 messageOffset;
    Switch trans;
    float time;
    GameObject message;

    public void SetTimer(float time)
    {
        if (this.time == 0)
        {
            message.GetComponent<TMP_Text>().DOKill();
            message.GetComponent<TMP_Text>().DOFade(1, 0.5f).SetEase(Ease.InOutQuad);
        }
        this.time = time;
    }

    private void Start()
    {
        trans = GetComponent<Switch>();

        message = Instantiate(messagePrefab);
        message.GetComponent<TextMeshProUGUI>().text = string.Format("Level {0}", levelId - 2);
        Camera mCamera = Camera.main;
        message.transform.SetParent(GameObject.Find("Canvas").transform);

        Vector2 adjustedPosition = mCamera.WorldToScreenPoint((Vector2)transform.position);
        RectTransform mCanvas = GameObject.Find("Canvas").GetComponent<RectTransform>();

        adjustedPosition.x *= mCanvas.rect.width / (float)mCamera.pixelWidth;
        adjustedPosition.y *= mCanvas.rect.height / (float)mCamera.pixelHeight;

        message.GetComponent<RectTransform>().anchoredPosition = adjustedPosition - mCanvas.sizeDelta / 2f;
        message.transform.localScale = Vector3.one;
        message.GetComponent<RectTransform>().anchoredPosition += messageOffset;
        //message.SetActive(false);
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
                message.GetComponent<TMP_Text>().DOKill();
                message.GetComponent<TMP_Text>().DOFade(0, 0.5f).SetEase(Ease.InOutQuad);
            }
        }
    }
}
