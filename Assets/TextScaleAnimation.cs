using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TextScaleAnimation : MonoBehaviour
{
    [SerializeField] float maxScale = 1.2f;
    [SerializeField] float duration = 0.6f;
    void Start()
    {
        transform.DOScale(maxScale, duration)
                 .SetLoops(-1, LoopType.Yoyo)
                 .SetUpdate(true)
                 .SetLink(gameObject);
    }
}
