using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageAnimation : MonoBehaviour
{
    [SerializeField] List<Sprite> sprites = new List<Sprite>();

    Image image;
    float delay = 0.1f;
    IEnumerator Start()
    {
        Debug.Assert(sprites.Count > 0, "여기엔 스프라이트가 있어야해");
        image = GetComponent<Image>();
        while (true)
        {
            while (SelectPlayModeUI.instance.GameState == GameStateType.Play)
            {
                image.sprite = sprites[Random.Range(0, sprites.Count)];
                yield return new WaitForSeconds(delay);
            }
        }
    }
}
