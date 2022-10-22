using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TimerSystem
{
    float remainTime;
    float addedTime;

    [SerializeField] float maxRemaineTime = 15;
    [SerializeField] float remainTimeAddValue = 0.5f;
    Action onGameOver;

    public void InitializeOnAwake()
    {
        remainTime = maxRemaineTime;
    }

    void Timer()
    {
        remainTime = Mathf.Min(remainTime - Time.deltaTime + addedTime, maxRemaineTime);
        addedTime = 0;
        TimerUI.Instance.SetTimer(remainTime, maxRemaineTime);

        if (remainTime < 0)
        {
            GameOverUI.Instance.ShowUI();
            onGameOver?.Invoke();
        }
    }

    public void OnDestroyAnimal(int count)
    {
        addedTime = count * remainTimeAddValue;
    }
}
