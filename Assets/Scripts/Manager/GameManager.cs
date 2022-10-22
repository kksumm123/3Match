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
    public bool IsSwitching => animalControlSystem.IsSwitching;
    public PlayModeType PlayMode
    {
        get => userInputSystem.PlayMode;
        set => SelectPlayModeType(value);
    }

    [SerializeField] TimerSystem timerSystem = new TimerSystem();
    ESCMenuSystem escMenuSystem = new ESCMenuSystem();
    [SerializeField] AnimalControlSystem animalControlSystem = new AnimalControlSystem();
    [SerializeField] UserInputSystem userInputSystem = new UserInputSystem();
    bool isPlaying = false;

    void Awake()
    {
        timerSystem.InitializeOnAwake(this, OnGameOver);
        escMenuSystem.Initialize(OnESCMenu: userInputSystem.ClearTouchInfo);
        animalControlSystem.Initialize(this, OnDestroyAnimals);
        userInputSystem.Initialize(this,
                                   animalControlSystem.IsMoving,
                                   () => animalControlSystem.AnimalGap,
                                   animalControlSystem.SwitchingAnimal,
                                   () => animalControlSystem.IsSwitching);
    }

    private IEnumerator Start()
    {
        escMenuSystem.OnWaitSelectPlayMode();
        yield return new WaitUntil(() => userInputSystem.IsSelectPlayMode());
        escMenuSystem.OnPlayGame();
        OnStartPlayGame();

        yield return new WaitForSeconds(1);

        while (isPlaying)
        {
            while (animalControlSystem.IsMoving() == false)
            {
                animalControlSystem.IsMacthAndDestroy();

                yield return userInputSystem.WaitUserInput();
            }
            yield return null;
        }
    }

    private void OnStartPlayGame()
    {
        animalControlSystem.GenerateAnimals();
        userInputSystem.OnStartPlayGame();

        isPlaying = true;
        timerSystem.StartTimer();
    }

    private void Update()
    {
        userInputSystem.TouchAndMove();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            escMenuSystem.ESCMenu();
        }
    }

    private void OnGameOver()
    {
        enabled = false;
    }

    private void OnDestroyAnimals(int count)
    {
        userInputSystem.ClearTouchInfo();
        timerSystem.OnDestroyAnimal(count);
    }

    public void OnCompleteDestroyAnimal(GameObject animal, int index)
    {
        animalControlSystem.OnCompleteDestroyAnimal(animal, index);
    }

    public void SelectPlayModeType(PlayModeType playModeType)
    {
        userInputSystem.SelectPlayModeType(playModeType);
    }

    public void OnMouseDown(Transform targetAnimal)
    {
        userInputSystem.OnMouseDown(targetAnimal);
    }

    public void OnMouseOver(Transform targetAnimal)
    {
        userInputSystem.OnMouseOver(targetAnimal);
    }
}
