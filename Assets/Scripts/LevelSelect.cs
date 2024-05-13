using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelSelect : Switch
{
    [SerializeField] private int levelId;
    [SerializeField] private GameObject messagePrefab;
    [SerializeField] private Vector2 messageOffset;
    private Switch trans;
    private float time;
    private GameObject message;

    public void SetTimer(float time)
    {
        if (this.time == 0)
        {
            message.GetComponent<TMP_Text>().DOKill();
            message.GetComponent<TMP_Text>().DOFade(1, 0.5f).SetEase(Ease.InOutQuad);
        }

        this.time = time;
    }

    protected override void Start()
    {
        base.Start();
        trans = GetComponent<Switch>();

        message = Instantiate(messagePrefab);
        message.GetComponent<TextMeshProUGUI>().text = $"Level {levelId - 3}";
        var mCamera = Camera.main;
        message.transform.SetParent(GameObject.Find("Canvas").transform);

        Vector2 adjustedPosition = mCamera.WorldToScreenPoint((Vector2)transform.position);
        var mCanvas = GameObject.Find("Canvas").GetComponent<RectTransform>();

        adjustedPosition.x *= mCanvas.rect.width / mCamera.pixelWidth;
        adjustedPosition.y *= mCanvas.rect.height / mCamera.pixelHeight;

        message.GetComponent<RectTransform>().anchoredPosition = adjustedPosition - mCanvas.sizeDelta / 2f;
        message.transform.localScale = Vector3.one;
        message.GetComponent<RectTransform>().anchoredPosition += messageOffset;
        //message.SetActive(false);
    }

    public void NonPlayerToggle()
    {
        base.Toggle();
    }

    public override void Toggle()
    {
        if (!LevelSelectManager.toggled)
        {
            UImanager.StartLevelTransition(levelId, 0.5f);
            LevelSelectManager.toggled = true;
        }
    }

    private void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                time = 0;
                message.GetComponent<TMP_Text>().DOKill();
                message.GetComponent<TMP_Text>().DOFade(0, 0.5f).SetEase(Ease.InOutQuad);
            }
        }
    }
}