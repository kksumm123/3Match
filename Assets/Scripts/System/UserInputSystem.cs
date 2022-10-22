using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserInputSystem
{
    public PlayModeType PlayMode => _playMode;

    Func<bool> isMoving;
    Func<Vector2> getAnimapGap;
    Action<Transform, Transform> switchingAnimal;
    Func<bool> isSwitching;

    PlayModeType _playMode;

    LayerMask animalLayer;
    [SerializeField] string animalLayerName = "Animal";
    [SerializeField] GameObject touchEffectPrefab;
    GameObject touchedEffect;
    Transform touchedAnimal;
    Transform pressedAnimal;
    Transform releasedAnimal;

    bool firstTouch = true;
    bool isTouchable = false;
    Vector2 animalGap;

    public void Initialize(GameManager gameManager, Func<bool> isMoving, Func<Vector2> getAnimapGap, Action<Transform, Transform> switchingAnimal, Func<bool> isSwitching)
    {
        this.isMoving = isMoving;
        this.getAnimapGap = getAnimapGap;
        this.switchingAnimal = switchingAnimal;
        this.isSwitching = isSwitching;

        WoonyMethods.Asserts(gameManager, (touchEffectPrefab, nameof(touchEffectPrefab)));

        animalLayer = 1 << LayerMask.NameToLayer(animalLayerName);
    }

    public bool IsSelectPlayMode()
    {
        return _playMode != PlayModeType.None;
    }

    public void OnStartPlayGame()
    {
        isTouchable = true;
    }

    public IEnumerator WaitUserInput()
    {
        isTouchable = true;
        // Wait 1f, cuz DestroyAnimation Lengh = 0.5f
        yield return new WaitForSeconds(1);
        isTouchable = false;
    }

    private float GetMaxDistance()
    {
        return Mathf.Max((float)(getAnimapGap?.Invoke().x), (float)(getAnimapGap?.Invoke().y)) + 0.01f;
    }

    public void SelectPlayModeType(PlayModeType playModeType)
    {
        _playMode = playModeType;
    }

    public void OnMouseDown(Transform targetAnimal)
    {
        pressedAnimal = targetAnimal;
    }

    public void OnMouseOver(Transform targetAnimal)
    {
        releasedAnimal = targetAnimal;
    }

    public void TouchAndMove()
    {
        if (_playMode == PlayModeType.None)
            return;

        if (isTouchable == false)
            return;

        switch (PlayMode)
        {
            case PlayModeType.TouchAndTouch:
                // Touch and Touch
                Method_TouchAndTouch();
                break;
            case PlayModeType.Drag:
                // Drag
                Method_Drag();
                break;
        }
    }

    void Method_TouchAndTouch()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if ((bool)(isMoving?.Invoke())) return;

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, 30, animalLayer);
            if (hit.transform)
            {
                if (touchedAnimal == null)
                {
                    ClearTouchInfo();
                    touchedAnimal = hit.transform;
                    touchedEffect = GameObject.Instantiate(touchEffectPrefab, touchedAnimal.position, Quaternion.identity);
                }
                else if (touchedAnimal != hit.transform)
                {
                    if (Vector3.Distance(touchedAnimal.position, hit.transform.position) <= GetMaxDistance())
                    {
                        switchingAnimal?.Invoke(pressedAnimal, releasedAnimal);
                    }
                    ClearTouchInfo();
                }
                else
                    ClearTouchInfo();
            }
            else
                ClearTouchInfo();
        }
    }

    void Method_Drag()
    {
        if (Input.GetMouseButton(0) && isSwitching?.Invoke() == false)
        {
            if ((bool)(isMoving?.Invoke())) return;

            if (pressedAnimal)
            {
                if (firstTouch == true)
                {
                    touchedEffect = GameObject.Instantiate(touchEffectPrefab, pressedAnimal.position, Quaternion.identity);
                    firstTouch = false;
                }
            }
        }
        else if (pressedAnimal != null && releasedAnimal != null
                && pressedAnimal != releasedAnimal
                && Vector3.Distance(pressedAnimal.position, releasedAnimal.position) <= GetMaxDistance()
                && isSwitching?.Invoke() == false)
        {
            firstTouch = true;
            switchingAnimal?.Invoke(pressedAnimal, releasedAnimal);
        }
        else
            ClearTouchInfo();
    }

    public void ClearTouchInfo()
    {
        firstTouch = true;
        touchedAnimal = null;
        pressedAnimal = null;
        releasedAnimal = null;
        GameObject.Destroy(touchedEffect);
    }
}
