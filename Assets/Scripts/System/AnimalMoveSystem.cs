using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimalMoveSystem
{
    public bool IsSwipping => _isSwipping;

    Func<int, int, Animal> getAnimal;
    Func<bool> isMacthed;
    [SerializeField] float tweenMoveTime = 0.3f;
    bool _isSwipping = false;

    public void Initialize(Func<int, int, Animal> getAnimal, Func<bool> isMacthed)
    {
        this.getAnimal = getAnimal;
        this.isMacthed = isMacthed;
    }

    public bool IsMoving(int row, int column)
    {
        for (int x = 0; x < column; x++)
        {
            for (int y = 0; y < row; y++)
            {
                if (getAnimal?.Invoke(x, y).IsMoveing == true)
                    return true;
            }
        }
        return false;
    }

    public void SwitchingAnimal(List<List<GameObject>> animalsList, Transform animal1, Transform animal2)
    {
        SwitchAnimalsIndex(animalsList, animal1, animal2);

        var isMatched = isMacthed?.Invoke();
        if (isMatched == false)
        {
            SwitchAnimalsIndex(animalsList, animal1, animal2);
        }

        MovePosition(animal1, animal2, (bool)isMatched);
        MovePosition(animal2, animal1, (bool)isMatched);
    }

    void SwitchAnimalsIndex(List<List<GameObject>> animalsList, Transform animal1, Transform animal2)
    {
        _isSwipping = true;
        var animal1Index = animal1.GetComponent<Animal>().Index;
        var animal2Index = animal2.GetComponent<Animal>().Index;
        int animal1Y = animalsList[animal1Index].IndexOf(animal1.gameObject);
        int animal2Y = animalsList[animal2Index].IndexOf(animal2.gameObject);

        var temp = animalsList[animal1Index][animal1Y];
        animalsList[animal1Index][animal1Y] = animalsList[animal2Index][animal2Y];
        animalsList[animal2Index][animal2Y] = temp;
        getAnimal.Invoke(animal1Index, animal1Y).Index = animal1Index;
        getAnimal.Invoke(animal2Index, animal2Y).Index = animal2Index;
    }

    void MovePosition(Transform _transform, Transform target, bool isMatched)
    {
        _transform.DOMove(target.position, tweenMoveTime)
                  .SetLoops(isMatched == true ? 1 : 2, LoopType.Yoyo)
                  .SetEase(Ease.OutBounce)
                  .SetLink(_transform.gameObject)
                  .OnComplete(() => _isSwipping = false);
    }
}
