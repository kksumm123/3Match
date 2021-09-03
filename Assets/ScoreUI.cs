using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    public static ScoreUI Instance;
    void Awake() => Instance = this;

    int score;
    Text value;
    void Start()
    {
        score = 0;
        value = transform.Find("Value").GetComponent<Text>();
        value.text = "0";
    }

    float scoreAnimTime = 0.3f;
    int scoreValue = 10;
    internal void AddScore(int count)
    {
        int oldScore = score;
        score += scoreValue * count;
        DOTween.To(() => oldScore, (x) => value.text = x.ToString()
                    , score, scoreAnimTime).SetLink(gameObject);
    }
}
