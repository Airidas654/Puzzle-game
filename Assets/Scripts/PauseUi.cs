using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;
using UnityEngine.UI;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

public class PauseUi : MonoBehaviour
{
    [SerializeField] Volume volume;
    [SerializeField] Image blackScreen;
    [SerializeField] RectTransform list;
    [SerializeField] LayerMask postProcessMask;

    float defaultY;

    public static PauseUi instance;

    void LerpPause(float val)
    {
        val = Mathf.Clamp01(val);

        color.contrast.Interp(4, 20, val);
        color.saturation.Interp(0, -30, val);
        depthOfField.focusDistance.Interp(3,0.1f,val);
        blackScreen.color = Color.Lerp(new Color(0,0,0,0), new Color(0,0,0, 146f/255f), val);

        list.anchoredPosition = new Vector2(0,Mathf.Lerp(-defaultY, defaultY, val));

    }

    ColorAdjustments color;
    DepthOfField depthOfField;
    void Start()
    {
        defaultY = list.anchoredPosition.y;
        volume.profile.TryGet(out color);
        volume.profile.TryGet(out depthOfField);

        

        SetPauseInstantly(false);
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
        else
        {
            Destroy(this);
        }
    }

    void SetPauseInstantly(bool val)
    {
        transitioning = false;
        if (val)
        {
            if (!GameManager.inst.CantDoAnything)
                Time.timeScale = 0;
            paused = true;
            pauseVal = 1;
            blackScreen.gameObject.SetActive(true);
            list.gameObject.SetActive(true);
            LerpPause(1f);
        }
        else
        {
            if (!GameManager.inst.CantDoAnything)
                Time.timeScale = 1;
            paused = false;
            pauseVal= 0;
            blackScreen.gameObject.SetActive(false);
            list.gameObject.SetActive(false);
            LerpPause(0f);
        }
    } 

    void SetPause(bool val)
    {
        if (!transitioning && !GameManager.inst.dead && !GameManager.inst.CantDoAnything && !GameManager.inst.levelEnterFreeze)
        {
            transitioning= true;
            DOTween.Kill(555);
            if (val)
            {
                Time.timeScale = 0;
                paused = true;
                blackScreen.gameObject.SetActive(true);
                list.gameObject.SetActive(true);
                DOTween.To(() => pauseVal, (x) => { pauseVal = x; LerpPause(pauseVal); }, 1, 0.5f).SetId(555).OnComplete(() => { SetPauseInstantly(true); transitioning = false; }).SetUpdate(true).SetUpdate(UpdateType.Normal, true);
            }
            else
            {
                
                DOTween.To(() => pauseVal, (x) => { pauseVal = x; LerpPause(pauseVal); }, 0, 0.5f).SetId(555).OnComplete(() => { SetPauseInstantly(false); transitioning = false; }).SetUpdate(true).SetUpdate(UpdateType.Normal, true);
            }
        }
    }

    bool transitioning;
    float pauseVal;
    public bool paused { get; private set; }

    public void Resume()
    {
        if (paused && !transitioning)
            SetPause(!paused);
    }

    public void Menu()
    {
        if (paused && !transitioning)
        {
            SetPause(false);
            GameManager.inst.CantDoAnything = true;
            UImanager.StartLevelTransition(0, 0.5f, 0.25f);
        }
    }

    public void ResetLevel()
    {
        if (paused && !transitioning)
        {
            SetPause(false);
            GameManager.inst.CantDoAnything = true;
            UImanager.StartLevelTransition(SceneManager.GetActiveScene().buildIndex, 0.5f, 0.25f);
        }
    }

    void Update()
    {
        if (!GameManager.inst.CantDoAnything && (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)))
        {
            SetPause(!paused);
        }
    }
}
