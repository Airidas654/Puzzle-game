using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OptionsCheckbox : MonoBehaviour
{
    [SerializeField] string optionName;
    [SerializeField] GameObject messagePrefab;
    [SerializeField] Vector2 messageOffset;
    GameObject message;
    Switch trans;

    RectTransform mCanvas;
    Camera mCamera;

    public void SetValue(bool value)
    {
        if (trans == null)
        {
            trans = GetComponent<Switch>();
        }
        if (value != trans.state)
        {
            trans.Toggle();
        }
    }

    void Start()
    {
        trans = GetComponent<Switch>();
        message = Instantiate(messagePrefab);
        message.GetComponent<TextMeshProUGUI>().text = string.Format("{0}: {1}", optionName, (trans.state)?"On":"Off");
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
    
    public bool GetValue()
    {
        return trans.state;
    }
    private void Update()
    {
        message.GetComponent<TextMeshProUGUI>().text = string.Format("{0}: {1}", optionName, (trans.state) ? "On" : "Off");
    }
}
