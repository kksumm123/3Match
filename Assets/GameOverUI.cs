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
    void Start()
    {
        yourScoreValue = transform.Find("YourScoreValue").GetComponent<Text>();
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gameObject.SetActive(false);
            SceneManager.LoadScene(0);
        }
    }

    float scoreAnimTime = 1f;
    public void ShowUI()
    {
        int score = ScoreUI.Instance.Score;
        GameManager.instance.PlayMode = PlayModeType.None;
        SelectPlayModeUI.instance.GameState = GameStateType.Menu;
        gameObject.SetActive(true);
        SoundManager.Instance.PlayBGM_Menu();
        DOTween.To(() => 0, (x) => yourScoreValue.text = x.ToString()
                    , score, scoreAnimTime).SetLink(gameObject).SetUpdate(true);
    }
}
