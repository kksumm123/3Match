using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    void Awake() => Instance = this;

    AudioSource audioSFX;

    void Start()
    {
        audioSFX = transform.Find("SFX").GetComponent<AudioSource>();
    }

    public void PlaySFX()
    {
        audioSFX.Play();
    }
}
