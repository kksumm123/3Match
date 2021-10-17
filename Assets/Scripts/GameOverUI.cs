using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public static GameOverUI Instance;
    void Awake() => Instance = this;

    Text yourScoreValue;
    Text highScoreValue;

    void Start()
    {
        yourScoreValue = transform.Find("YourScoreValue").GetComponent<Text>();
        highScoreValue = transform.Find("HighScoreValue").GetComponent<Text>();
        gameObject.SetActive(false);
    }

    bool isShowAds = false;
    float restartableTime;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (restartableTime > Time.realtimeSinceStartup)
                return;

            isShowAds = true;
            if (Random.Range(0, 1f) < 0.6f)
            {
                AdsManager.Instance.ShowAds();
            }
            if (isShowAds)
            {
                enabled = false;
                ReloadScene();
            }
        }
    }

    public void ReloadScene()
    {
        gameObject.SetActive(false);
        SceneManager.LoadScene(0);
    }

    float scoreAnimTime = 1f;
    public void ShowUI()
    {
        restartableTime = Time.realtimeSinceStartup + 1.5f;

        int score = ScoreUI.Instance.Score;
        highScoreValue.text = ScoreUI.Instance.HighScore.ToString();

        GameManager.instance.PlayMode = PlayModeType.None;
        SelectPlayModeUI.instance.GameState = GameStateType.Menu;
        SoundManager.Instance.PlayBGM_Menu();
        gameObject.SetActive(true);

        DOTween.To(() => 0, (x) => yourScoreValue.text = x.ToString()
                    , score, scoreAnimTime).SetLink(gameObject).SetUpdate(true);
    }
}
