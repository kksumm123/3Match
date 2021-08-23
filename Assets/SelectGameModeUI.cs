using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectGameModeUI : MonoBehaviour
{
    void Start()
    {
        transform.Find("ButtonTouch").GetComponent<Button>()
                 .onClick.AddListener(() =>
                 {
                     GameManager.instance.PlayMode = GameManager.PlayModeType.TouchAndTouch;
                     gameObject.SetActive(false);
                 });
        transform.Find("ButtonDrag").GetComponent<Button>()
            .onClick.AddListener(() =>
            {
                GameManager.instance.PlayMode = GameManager.PlayModeType.Drag;
                gameObject.SetActive(false);
            });
    }
}
