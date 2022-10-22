using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public enum PlayModeType
{
    None,
    TouchAndTouch,
    Drag,
}
public enum MatchMode
{
    Check,
    CheckAndDestroy,
}
public class GameManager : Singleton<GameManager>
{
    public float xGap => animalControlSystem.AnimalGap.x;
    public float yGap => animalControlSystem.AnimalGap.y;
    public bool IsSwipping => animalControlSystem.IsSwipping;

    [SerializeField] TimerSystem timerSystem = new TimerSystem();
    ESCMenuSystem escMenuSystem = new ESCMenuSystem();
    [SerializeField] AnimalControlSystem animalControlSystem = new AnimalControlSystem();

    PlayModeType playMode;
    public PlayModeType PlayMode
    {
        get => playMode;
        set => playMode = value;
    }

    void Awake()
    {
        timerSystem.InitializeOnAwake(this, OnGameOver);
        escMenuSystem.Initialize(OnESCMenu: ClearTouchInfo);
        animalControlSystem.Initialize(this, OnDestroyAnimals);
    }

    readonly string touchEffectString = "TouchEffect";
    GameObject touchEffectGo;
    LayerMask animalLayer;

    bool isPlaying = false;
    bool isMoveable = false;
    IEnumerator Start()
    {
        escMenuSystem.OnWaitSelectPlayMode();
        yield return new WaitUntil(() => IsSelectPlayMode());
        escMenuSystem.OnPlayGame();
        OnStartPlayGame();
        yield return new WaitForSeconds(1);
        while (isPlaying)
        {
            while (animalControlSystem.IsMoving() == false)
            {
                animalControlSystem.IsMacthAndDestroy();

                isMoveable = true;
                // Wait 1f, cuz DestroyAnimation Lengh = 0.5f
                yield return new WaitForSeconds(1);
                isMoveable = false;
            }
            yield return null;
        }
    }

    private void OnStartPlayGame()
    {
        touchEffectGo = (GameObject)Resources.Load(touchEffectString);
        animalLayer = 1 << LayerMask.NameToLayer("Animal");

        animalControlSystem.GenerateAnimals();

        isPlaying = true;
        isMoveable = true;
        timerSystem.StartTimer();
    }

    private bool IsSelectPlayMode()
    {
        return playMode != PlayModeType.None;
    }

    void Update()
    {
        TouchAndMove();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            escMenuSystem.ESCMenu();
        }
    }

    void OnGameOver()
    {
        enabled = false;
    }

    #region TouchAndMove
    bool firstTouch = true;
    GameObject touchedEffect;
    void TouchAndMove()
    {
        if (PlayMode == PlayModeType.None)
            return;

        if (isMoveable == false)
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
    Transform touchedAnimal;
    void Method_TouchAndTouch()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (animalControlSystem.IsMoving())
                return;

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, 30, animalLayer);
            if (hit.transform)
            {
                if (touchedAnimal == null)
                {
                    ClearTouchInfo();
                    touchedAnimal = hit.transform;
                    touchedEffect = Instantiate(touchEffectGo, touchedAnimal.position, Quaternion.identity);
                }
                else if (touchedAnimal != hit.transform)
                {
                    if (Vector3.Distance(touchedAnimal.position, hit.transform.position) <= Mathf.Max(xGap, yGap) + 0.01f)
                    {
                        animalControlSystem.SwitchingAnimal(pressedAnimal, releasedAnimal);
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

    public Transform pressedAnimal;
    public Transform releasedAnimal;
    void Method_Drag()
    {
        if (Input.GetMouseButton(0) && IsSwipping == false)
        {
            if (animalControlSystem.IsMoving())
                return;

            if (pressedAnimal)
            {
                if (firstTouch == true)
                {
                    touchedEffect = Instantiate(touchEffectGo, pressedAnimal.position, Quaternion.identity);
                    firstTouch = false;
                }
            }
        }
        else if (pressedAnimal != null && releasedAnimal != null
                && pressedAnimal != releasedAnimal
                && Vector3.Distance(pressedAnimal.position, releasedAnimal.position) <= Mathf.Max(xGap, yGap) + 0.01f
                && IsSwipping == false)
        {
            firstTouch = true;
            animalControlSystem.SwitchingAnimal(pressedAnimal, releasedAnimal);
        }
        else
            ClearTouchInfo();
    }
    #endregion TouchAndMove

    #region Methods

    private void ClearTouchInfo()
    {
        firstTouch = true;
        touchedAnimal = null;
        pressedAnimal = null;
        releasedAnimal = null;
        Destroy(touchedEffect);
    }

    private void OnDestroyAnimals(int count)
    {
        ClearTouchInfo();
        timerSystem.OnDestroyAnimal(count);
    }

    public void OnCompleteDestroyAnimal(GameObject animal, int index)
    {
        animalControlSystem.OnCompleteDestroyAnimal(animal, index);
    }
    #endregion Methods
}
