using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OptionsSlider : MonoBehaviour
{
    [SerializeField] private string optionName;
    [SerializeField] private Vector2 range;
    [SerializeField] private bool XAxis;
    [SerializeField] private GameObject messagePrefab;
    [SerializeField] private Vector2 messageOffset;
    private GameObject message;
    public float value { get; private set; }

    private RectTransform mCanvas;
    private Camera mCamera;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (XAxis)
        {
            Gizmos.DrawWireSphere(new Vector2(range.x, transform.position.y), 0.1f);
            Gizmos.DrawWireSphere(new Vector2(range.y, transform.position.y), 0.1f);
        }
        else
        {
            Gizmos.DrawWireSphere(new Vector2(transform.position.x, range.x), 0.1f);
            Gizmos.DrawWireSphere(new Vector2(transform.position.x, range.y), 0.1f);
        }
    }

    public void SetValue(float value)
    {
        this.value = value;
        if (XAxis)
            transform.position = new Vector2(Mathf.Lerp(range.x, range.y, value), transform.position.y);
        else
            transform.position = new Vector2(transform.position.x, Mathf.Lerp(range.x, range.y, value));
    }

    private void Start()
    {

        if (XAxis)
            value = Mathf.InverseLerp(range.x, range.y, transform.position.x);
        else
            value = Mathf.InverseLerp(range.x, range.y, transform.position.y);

        message = Instantiate(messagePrefab);
        message.GetComponent<TextMeshProUGUI>().text = string.Format("{0}: {1}%", optionName, (int)(value * 100));
        message.GetComponent<TMP_Text>().alpha = 1;
        mCamera = Camera.main;
        message.transform.SetParent(GameObject.Find("Canvas").transform);

        Vector2 adjustedPosition = mCamera.WorldToScreenPoint((Vector2)transform.position);
        mCanvas = GameObject.Find("Canvas").GetComponent<RectTransform>();

        adjustedPosition.x *= mCanvas.rect.width / (float)mCamera.pixelWidth;
        adjustedPosition.y *= mCanvas.rect.height / (float)mCamera.pixelHeight;

        message.GetComponent<RectTransform>().anchoredPosition = adjustedPosition - mCanvas.sizeDelta / 2f;
        message.transform.localScale = Vector3.one;
        message.GetComponent<RectTransform>().anchoredPosition += messageOffset;
    }

    // Update is called once per frame
    private void Update()
    {
        if (XAxis)
        {
            value = Mathf.InverseLerp(range.x, range.y, transform.position.x);
            transform.position = new Vector2(Mathf.Clamp(transform.position.x, range.x, range.y), transform.position.y);
        }
        else
        {
            value = Mathf.InverseLerp(range.x, range.y, transform.position.y);
            transform.position = new Vector2(transform.position.x, Mathf.Clamp(transform.position.y, range.x, range.y));
        }

        message.GetComponent<TextMeshProUGUI>().text = string.Format("{0}: {1}%", optionName, (int)(value * 100));

        Vector2 adjustedPosition = mCamera.WorldToScreenPoint((Vector2)transform.position);
        adjustedPosition.x *= mCanvas.rect.width / (float)mCamera.pixelWidth;
        adjustedPosition.y *= mCanvas.rect.height / (float)mCamera.pixelHeight;
        message.GetComponent<RectTransform>().anchoredPosition = adjustedPosition - mCanvas.sizeDelta / 2f;
        message.GetComponent<RectTransform>().anchoredPosition += messageOffset;
    }
}