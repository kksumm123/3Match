using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    private const string highScoreKey = "HighScoreKey";
    public static ScoreUI Instance;
    void Awake() => Instance = this;

    int score;
    int highScore;
    public int Score => score;
    public int HighScore => highScore;
    Text value;
    Text highScoreValue;
    void Start()
    {
        score = 0;
        value = transform.Find("Value").GetComponent<Text>();
        value.text = "0";
        highScoreValue = transform.Find("HighScoreValue").GetComponent<Text>();
        highScore = PlayerPrefs.HasKey(highScoreKey) == true
                    ? PlayerPrefs.GetInt(highScoreKey) : 0;
        highScoreValue.text = highScore.ToString(); ;
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetInt(highScoreKey, highScore);
    }

    float scoreAnimTime = 0.3f;
    int scoreValue = 10;

    internal void AddScore(int count)
    {
        int oldScore = score;
        score += scoreValue * count;
        DOTween.To(() => oldScore, (x) => value.text = x.ToString()
                    , score, scoreAnimTime).SetLink(gameObject).SetUpdate(true);
        if (score > highScore)
        {
            int oldHighScore = highScore;
            highScore = score;
            DOTween.To(() => oldHighScore, (x) => highScoreValue.text = x.ToString()
                    , highScore, scoreAnimTime).SetLink(gameObject).SetUpdate(true);
        }
    }
}
