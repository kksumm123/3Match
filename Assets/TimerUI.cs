using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    Image gauge;
    float gaugeMaxWidth;
    void Start()
    {
        gauge = transform.Find("Gauge").GetComponent<Image>();
        gaugeMaxWidth = gauge.rectTransform.sizeDelta.x;
    }

    void Update()
    {
        
    }
}
