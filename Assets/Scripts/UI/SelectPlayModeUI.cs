using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameStateType
{
    None,
    Menu,
    Play,
}

public class SelectPlayModeUI : Singleton<SelectPlayModeUI>
{
    PlayModeType originPlayMode;

    GameStateType m_gameState = GameStateType.None;

    public GameStateType GameState
    {
        get => m_gameState;
        set
        {
            if (m_gameState == value)
                return;

            switch (value)
            {
                case GameStateType.Menu:
                    Time.timeScale = 0;
                    break;
                case GameStateType.Play:
                    Time.timeScale = 1;
                    break;
            }
            print($"{m_gameState} -> {value}, TimeScale : {Time.timeScale}");
            m_gameState = value;
        }
    }

    Slider sliderBGM;
    Slider sliderSFX;
    string sliderBGMKey = "SliderBGM";
    string sliderSFXKey = "SliderSFX";

    void Start()
    {
        GameState = GameStateType.Menu;
        Initialize();
        LoadData();
    }

    private void Initialize()
    {
        transform.Find("ButtonTouch").GetComponent<Button>()
                         .onClick.AddListener(() =>
                         {
                             GameManager.Instance.PlayMode = PlayModeType.TouchAndTouch;
                             CloseUI();
                         });
        transform.Find("ButtonDrag").GetComponent<Button>()
                 .onClick.AddListener(() =>
                 {
                     GameManager.Instance.PlayMode = PlayModeType.Drag;
                     CloseUI();
                 });
        transform.Find("ButtonGameExit").GetComponent<Button>()
                 .onClick.AddListener(() =>
                 {
                     Application.Quit();
                 });

        sliderBGM = transform.Find("SliderBGM/Slider").GetComponent<Slider>();
        sliderSFX = transform.Find("SliderSFX/Slider").GetComponent<Slider>();

        sliderBGM.onValueChanged.AddListener((x) => OnChangeBGM(x));
        sliderSFX.onValueChanged.AddListener((x) => OnChangeSFX(x));
    }

    void OnChangeBGM(float value)
    {
        SoundManager.Instance.GBGMVolume = value;
    }

    void OnChangeSFX(float value)
    {
        SoundManager.Instance.GSFXVolume = value;
    }

    private void LoadData()
    {
        if (PlayerPrefs.HasKey(sliderBGMKey))
            sliderBGM.value = PlayerPrefs.GetFloat(sliderBGMKey);
        if (PlayerPrefs.HasKey(sliderSFXKey))
            sliderSFX.value = PlayerPrefs.GetFloat(sliderSFXKey);
    }

    private void SaveData()
    {
        PlayerPrefs.SetFloat(sliderBGMKey, sliderBGM.value);
        PlayerPrefs.SetFloat(sliderSFXKey, sliderSFX.value);
    }

    void OnDestroy()
    {
        SaveData();
    }

    public void ShowUI()
    {
        originPlayMode = GameManager.Instance.PlayMode;
        GameManager.Instance.PlayMode = PlayModeType.None;
        GameState = GameStateType.Menu;
        gameObject.SetActive(true);
        SoundManager.Instance.PlayBGM_Menu();
    }

    public void CloseUI()
    {
        if (GameManager.Instance.PlayMode == PlayModeType.None)
            GameManager.Instance.PlayMode = originPlayMode;
        GameState = GameStateType.Play;
        gameObject.SetActive(false);
        SoundManager.Instance.PlayBGM_InGame();
    }
}
