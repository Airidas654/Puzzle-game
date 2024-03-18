using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UImanager : MonoBehaviour
{
    [SerializeField] Image dark;
    [SerializeField] float darkStartLength;
    public static UImanager Instance = null;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public static void StartLevelTransition(int sceneIndex, float length, float delay = 0)
    {
        Instance.dark.DOKill();
        Instance.dark.gameObject.SetActive(true);
        Instance.dark.DOColor(new Color(0, 0, 0, 1), length).SetDelay(delay).SetEase(Ease.InSine).OnComplete(() => SceneManager.LoadScene(sceneIndex));
    }

    private void Start()
    {
        dark.DOKill();
        dark.DOColor(new Color(0,0,0,0), darkStartLength).SetEase(Ease.OutSine).OnComplete(()=>dark.gameObject.SetActive(false));
    }
}
