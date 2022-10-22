using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TimerSystem
{
    MonoBehaviour owner;
    Action onGameOver;

    float remainTime;
    float addedTime;
    [SerializeField] float maxRemaineTime = 15;
    [SerializeField] float remainTimeAddValue = 0.5f;

    Coroutine timerCoHandle;

    public void InitializeOnAwake(MonoBehaviour owner, Action onGameOver)
    {
        this.owner = owner;
        this.onGameOver = onGameOver;
        remainTime = maxRemaineTime;
    }

    public void StartTimer()
    {
        timerCoHandle = WoonyMethods.StopAndStartCo(owner, timerCoHandle, TimerCo());
    }

    IEnumerator TimerCo()
    {
        var isTrue = true;
        while (isTrue)
        {
            remainTime = Mathf.Min(remainTime - Time.deltaTime + addedTime, maxRemaineTime);
            addedTime = 0;
            TimerUI.Instance.SetTimer(remainTime, maxRemaineTime);

            if (remainTime < 0)
            {
                GameOverUI.Instance.ShowUI();
                onGameOver?.Invoke();
            }
            yield return null;
        }
    }

    public void OnDestroyAnimal(int count)
    {
        addedTime = count * remainTimeAddValue;
    }
}
