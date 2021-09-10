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
        GameState = GameStateType.Menu;

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
        transform.Find("ButtonGameExit").GetComponent<Button>()
                 .onClick.AddListener(() =>
                 {
                     Application.Quit();
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

    PlayModeType originPlayMode;
    public void ShowUI()
    {
        originPlayMode = GameManager.instance.PlayMode;
        GameManager.instance.PlayMode = PlayModeType.None;
        GameState = GameStateType.Menu;
        gameObject.SetActive(true);
        SoundManager.Instance.PlayBGM_Menu();
    }
    public void CloseUI()
    {
        if (GameManager.instance.PlayMode == PlayModeType.None)
            GameManager.instance.PlayMode = originPlayMode;
        GameState = GameStateType.Play;
        gameObject.SetActive(false);
        SoundManager.Instance.PlayBGM_InGame();
    }

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
}
public enum GameStateType
{
    None,
    Menu,
    Play,
}
