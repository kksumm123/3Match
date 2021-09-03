using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    public static TimerUI Instance;
    void Awake() => Instance = this;

    Image gauge;
    float gaugeMaxWidth;
    void Start()
    {
        gauge = transform.Find("Gauge").GetComponent<Image>();
        gaugeMaxWidth = gauge.rectTransform.sizeDelta.x;
    }

    public void SetTimer(float remainTime, float maxRemaineTime)
    {
        var sizeDelta = gauge.rectTransform.sizeDelta;
        sizeDelta.x = gaugeMaxWidth * (remainTime / maxRemaineTime);
        gauge.rectTransform.sizeDelta = sizeDelta;
    }
}
