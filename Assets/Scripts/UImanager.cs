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
    [SerializeField] Material trasitionMat;
    [SerializeField] GameObject player;
    [SerializeField] Vector2 playerOffset;

    int transitionTweenId = 134;
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
    bool oneTime = false;
    public static void StartLevelTransition(int sceneIndex, float length, float delay = 0)
    {
        if (Instance.oneTime) return;
        Instance.oneTime = true;
        //Instance.dark.DOKill();
        //Instance.dark.gameObject.SetActive(true);
        //Instance.dark.DOColor(new Color(0, 0, 0, 1), length).SetDelay(delay).SetEase(Ease.InSine).OnComplete(() => SceneManager.LoadScene(sceneIndex));

        DOTween.Kill(Instance.transitionTweenId);
        value = 1.2f;

        DOTween.To(() => value, (x) => {
            value = x;
            Vector2 offset = Camera.main.WorldToViewportPoint((Vector2)Instance.player.transform.position+Instance.playerOffset);
            Instance.trasitionMat.SetVector(offsetId, offset);
            Instance.trasitionMat.SetFloat(valueId, x);
        }, 0, length).SetId(Instance.transitionTweenId).SetEase(Ease.OutSine).SetDelay(delay).OnComplete(() => SceneManager.LoadScene(sceneIndex));
    }

    static float value;
    static int valueId;
    static int offsetId;

    private void Start()
    {
        valueId = Shader.PropertyToID("_Value");
        offsetId = Shader.PropertyToID("_Offset");

        //dark.DOKill();
        //dark.DOColor(new Color(0,0,0,0), darkStartLength).SetEase(Ease.OutSine).OnComplete(()=>dark.gameObject.SetActive(false));
        DOTween.Kill(transitionTweenId);
        value = 0;

        DOTween.To(() => value, (x) => { 
            value = x;
            Vector2 offset = Camera.main.WorldToViewportPoint((Vector2)player.transform.position + playerOffset);
            trasitionMat.SetVector(offsetId, offset);
            trasitionMat.SetFloat(valueId, x);
        }, 1.2f, darkStartLength).SetId(transitionTweenId).SetEase(Ease.OutSine);
    }

    private void Update()
    {
        
    }
}
