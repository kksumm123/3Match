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
public class GameManager : MonoBehaviour
{
    PlayModeType playMode;
    public PlayModeType PlayMode
    {
        get => playMode;
        set => playMode = value;
    }

    public static GameManager instance;
    void Awake()
    {
        instance = this;
    }

    Transform animalParent;
    readonly string animalGoString = "Animal";
    GameObject animalGo;
    float xGap;
    float yGap;
    float animalScaleX;
    int row = 10;
    int column = 6;
    List<List<GameObject>> animalsList;
    List<Animal> toDestroyAnimals = new List<Animal>();
    readonly string touchEffectString = "TouchEffect";
    GameObject touchEffectGo;
    LayerMask animalLayer;

    bool isPlaying = false;
    bool m_isSwipping = false;
    public bool IsSwipping => m_isSwipping;
    bool isMoveable = false;
    IEnumerator Start()
    {
        firstPlay = true;

        while (PlayMode == PlayModeType.None)
            yield return null;
        firstPlay = false;

        animalParent = GameObject.Find("AnimalParent").transform;
        animalGo = (GameObject)Resources.Load(animalGoString);
        touchEffectGo = (GameObject)Resources.Load(touchEffectString);
        animalLayer = 1 << LayerMask.NameToLayer("Animal");

        animalScaleX = animalGo.transform.localScale.x;
        var animalColSize = animalGo.GetComponent<BoxCollider>().size;
        xGap = animalColSize.x * animalScaleX + 0.01f;
        yGap = animalColSize.y + 0.01f;
        GenerateAnimals();

        isPlaying = true;
        isMoveable = true;
        yield return new WaitForSeconds(1);
        while (isPlaying)
        {
            while (IsMoving() == false)
            {
                toDestroyAnimals.Clear();
                IsMatchedVertical(MatchMode.CheckAndDestroy);
                IsMatchedHorizon(MatchMode.CheckAndDestroy);
                DestroyAnimals();

                isMoveable = true;
                // Wait 1f, cuz DestroyAnimation Lengh = 0.5f
                yield return new WaitForSeconds(1f);
                isMoveable = false;
            }
            yield return null;
        }
    }
    void Update()
    {
        TouchAndMove();
        ESCMenu();
    }
    bool firstPlay;
    void ESCMenu()
    {
        if (firstPlay == true)
            return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClearTouchInfo();
            if (SelectPlayModeUI.instance.gameObject.activeSelf == false)
                SelectPlayModeUI.instance.ShowUI();
            else
                SelectPlayModeUI.instance.CloseUI();
        }
    }

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
            if (IsMoving() == true)
                return;

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, 30, animalLayer);
            if (hit.transform)
            {
                if (touchedAnimal == null)
                {
                    touchedAnimal = hit.transform;
                    touchedEffect = Instantiate(touchEffectGo, touchedAnimal.position, Quaternion.identity);
                }
                else if (touchedAnimal != hit.transform)
                {
                    if (Vector3.Distance(touchedAnimal.position, hit.transform.position) <= Mathf.Max(xGap, yGap) + 0.01f)
                    {
                        m_isSwipping = true;
                        SwipAnimals(touchedAnimal, hit.transform);
                        bool isRePosition = IsMatchedVertical(MatchMode.Check) == true || IsMatchedHorizon(MatchMode.Check) == true;
                        if (isRePosition == false)
                            SwipAnimals(touchedAnimal, hit.transform);

                        MovePosition(touchedAnimal, hit.transform, isRePosition);
                        MovePosition(hit.transform, touchedAnimal, isRePosition);
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
        if (Input.GetMouseButton(0) && m_isSwipping == false)
        {
            if (IsMoving() == true)
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
                && m_isSwipping == false)
        {
            firstTouch = true;
            m_isSwipping = true;
            SwipAnimals(pressedAnimal, releasedAnimal);
            bool isRePosition = IsMatchedVertical(MatchMode.Check) == true || IsMatchedHorizon(MatchMode.Check) == true;
            if (isRePosition == false)
                SwipAnimals(pressedAnimal, releasedAnimal);

            MovePosition(pressedAnimal, releasedAnimal, isRePosition);
            MovePosition(releasedAnimal, pressedAnimal, isRePosition);
        }
        else
            ClearTouchInfo();
    }

    [SerializeField] float tweenMoveTime = 0.3f;
    void MovePosition(Transform _transform, Transform target, bool isMatched)
    {
        _transform.DOMove(target.position, tweenMoveTime)
                  .SetLoops(isMatched == true ? 1 : 2, LoopType.Yoyo)
                  .SetEase(Ease.OutBounce)
                  .SetLink(_transform.gameObject)
                  .OnComplete(() => m_isSwipping = false);
    }

    enum MatchMode
    {
        Check,
        CheckAndDestroy,
    }
    bool IsMatchedVertical(MatchMode matchmode)
    {
        Animal first, second, third;
        for (int x = 0; x < column; x++)
        {
            for (int y = 0; y < row - 2; y++)
            {
                first = GetAnimal(x, y);
                second = GetAnimal(x, y + 1);
                third = GetAnimal(x, y + 2);

                if (first.name == second.name && second.name == third.name)
                {
                    switch (matchmode)
                    {
                        case MatchMode.Check:
                            return true;
                        case MatchMode.CheckAndDestroy:
                            AddtoDestroyAnimals(first, second, third);
                            break;
                    }
                }
            }
        }

        return false;
    }
    bool IsMatchedHorizon(MatchMode matchmode)
    {
        Animal first, second, third;
        for (int x = 0; x < column - 2; x++)
        {
            for (int y = 0; y < row; y++)
            {
                first = GetAnimal(x, y);
                second = GetAnimal(x + 1, y);
                third = GetAnimal(x + 2, y);

                if (first.name == second.name && second.name == third.name)
                {
                    switch (matchmode)
                    {
                        case MatchMode.Check:
                            return true;
                        case MatchMode.CheckAndDestroy:
                            AddtoDestroyAnimals(first, second, third);
                            break;
                    }
                }
            }
        }
        return false;
    }
    void DestroyAnimals()
    {
        toDestroyAnimals.ForEach((x) => StartCoroutine(x.Destroy()));
    }

    #region Methods
    private void ClearTouchInfo()
    {
        firstTouch = true;
        touchedAnimal = null;
        pressedAnimal = null;
        releasedAnimal = null;
        Destroy(touchedEffect);
    }

    private void SwipAnimals(Transform animal1, Transform animal2)
    {
        var animal1Index = animal1.GetComponent<Animal>().Index;
        var animal2Index = animal2.GetComponent<Animal>().Index;
        int animal1Y = animalsList[animal1Index].IndexOf(animal1.gameObject);
        int animal2Y = animalsList[animal2Index].IndexOf(animal2.gameObject);

        var temp = animalsList[animal1Index][animal1Y];
        animalsList[animal1Index][animal1Y] = animalsList[animal2Index][animal2Y];
        animalsList[animal2Index][animal2Y] = temp;
        GetAnimal(animal1Index, animal1Y).Index = animal1Index;
        GetAnimal(animal2Index, animal2Y).Index = animal2Index;
    }

    void AddtoDestroyAnimals(params Animal[] _animals)
    {
        foreach (var item in _animals)
        {
            if (toDestroyAnimals.Contains(item) == false)
                toDestroyAnimals.Add(item);
        }
    }

    bool IsMoving()
    {
        for (int x = 0; x < column; x++)
        {
            for (int y = 0; y < row; y++)
            {
                if (GetAnimal(x, y).IsMoveing == true)
                    return true;
            }
        }
        return false;
    }

    void GenerateAnimals()
    {
        animalsList = new List<List<GameObject>>(column);
        for (int x = 0; x < column; x++)
        {
            animalsList.Add(new List<GameObject>(row));

            for (int y = 0; y < row; y++)
                animalsList[x].Add(CreateAnimal(x, x * xGap, y * yGap));
        }
    }

    GameObject CreateAnimal(int x, float posX, float posY)
    {
        var pos = new Vector3(posX, posY);
        var newGo = Instantiate(animalGo, pos, Quaternion.identity, animalParent);
        newGo.GetComponent<Animal>().Index = x;
        return newGo;
    }

    Animal GetAnimal(int x, int y)
    {
        if (animalsList.Count > x && animalsList[x].Count > y)
            return animalsList[x][y].GetComponent<Animal>();

        Debug.LogWarning("ÀÎµ¦½º ¾øÀ¸¸é ¿©±â·Î ¿È");
        return null;
    }

    public void Remove(GameObject animal, int index)
    {
        animalsList[index].Remove(animal);
    }
    public void Reborn(int index)
    {
        var newY = GetAnimal(index, animalsList[index].Count - 1).transform.position.y;
        animalsList[index].Add(CreateAnimal(index, index * xGap, newY + (yGap * 2)));
    }
    #endregion Methods
}
