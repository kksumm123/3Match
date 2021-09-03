using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    void Awake() => Instance = this;

    AudioClip bgm_InGame;
    readonly string bgm_InGameString = "BGM_InGame";
    AudioClip bgm_Menu;
    readonly string bgm_MenuString = "BGM_Menu";

    AudioSource audioBGM;
    AudioSource audioSFX;

    float gBGMVolume = 1;
    float gSFXVolume = 1;
    float originBGMVolume;
    float originEnvSFXVolume;
    public float GBGMVolume
    {
        get => gBGMVolume;
        set
        {
            gBGMVolume = value;
            audioBGM.volume = originBGMVolume * gBGMVolume;
        }
    }

    public float GSFXVolume
    {
        get => gSFXVolume;
        set
        {
            gSFXVolume = value;
            audioSFX.volume = originEnvSFXVolume * gSFXVolume;
        }
    }
    void Start()
    {
        bgm_InGame = Resources.Load<AudioClip>(bgm_InGameString);
        bgm_Menu = Resources.Load<AudioClip>(bgm_MenuString);

        audioBGM = transform.Find("BGM").GetComponent<AudioSource>();
        audioSFX = transform.Find("SFX").GetComponent<AudioSource>();
        originBGMVolume = audioBGM.volume;
        originEnvSFXVolume = audioSFX.volume;

        audioBGM.clip = bgm_Menu;
        audioBGM.Play();
    }

    public void PlayBGM_InGame()
    {
        audioBGM.clip = bgm_InGame;
        audioBGM.Play();
    }
    public void PlayBGM_Menu()
    {
        audioBGM.clip = bgm_Menu;
        audioBGM.Play();
    }

    public void PlaySFX()
    {
        audioSFX.Play();
    }
}
