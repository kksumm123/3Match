using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPlayModeUI : MonoBehaviour
{
    public static SelectPlayModeUI instance;
    void Awake() => instance = this;

    Slider sliderBGM;
    Slider sliderSFX;
    void Start()
    {
        transform.Find("ButtonTouch").GetComponent<Button>()
                 .onClick.AddListener(() =>
                 {
                     GameManager.instance.PlayMode = PlayModeType.TouchAndTouch;
                     CloseUI();
                 });
        transform.Find("ButtonDrag").GetComponent<Button>()
                 .onClick.AddListener(() =>
                 {
                     GameManager.instance.PlayMode = PlayModeType.Drag;
                     CloseUI();
                 });

        sliderBGM = transform.Find("SliderBGM/Slider").GetComponent<Slider>();
        sliderSFX = transform.Find("SliderSFX/Slider").GetComponent<Slider>();
    }
    void Update()
    {
        VolumeBGM();
        VolumeSFXs();
    }
    void VolumeBGM()
    {
        SoundManager.Instance.GBGMVolume = sliderBGM.value;
    }
    void VolumeSFXs()
    {
        SoundManager.Instance.GSFXVolume = sliderSFX.value;
    }

    public void CloseUI()
    {
        if (GameManager.instance.PlayMode == PlayModeType.None)
            GameManager.instance.PlayMode = originPlayMode;
        Time.timeScale = 1;
        gameObject.SetActive(false);
        SoundManager.Instance.PlayBGM_InGame();
    }
    PlayModeType originPlayMode;
    public void ShowUI()
    {
        originPlayMode = GameManager.instance.PlayMode;
        GameManager.instance.PlayMode = PlayModeType.None;
        Time.timeScale = 0;
        gameObject.SetActive(true);
        SoundManager.Instance.PlayBGM_Menu();
    }
}
