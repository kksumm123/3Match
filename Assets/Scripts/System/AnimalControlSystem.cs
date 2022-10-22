using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimalControlSystem
{
    public Vector2 AnimalGap => _animalGap;
    public int ToDestroyAnimalCount => toDestroyAnimals.Count;
    public bool IsSwitching => animalMoveSystem.IsSwitching;

    Vector2 _animalGap;
    [SerializeField] int row = 10;
    [SerializeField] int column = 6;

    [SerializeField] AnimalGenerateSystem animalGenerateSystem = new AnimalGenerateSystem();
    MatchSystem matchSystem = new MatchSystem();
    AnimalDestroySystem animalDestroySystem = new AnimalDestroySystem();
    [SerializeField] AnimalMoveSystem animalMoveSystem = new AnimalMoveSystem();

    List<List<GameObject>> animalsList = new List<List<GameObject>>();
    List<Animal> toDestroyAnimals = new List<Animal>();

    public void Initialize(GameManager gameManager, Action<int> onDestroyAnimal)
    {
        animalGenerateSystem.Initialize(gameManager,
                                        out _animalGap.x,
                                        out _animalGap.y,
                                        row,
                                        column,
                                        GetAnimal);
        matchSystem.Initialize(row,
                               column,
                               GetAnimal,
                               animalDestroySystem.AddtoDestroyAnimals);
        animalDestroySystem.Initialize(toDestroyAnimals, onDestroyAnimal);
        animalMoveSystem.Initialize(GetAnimal, IsMacthed);
    }

    public void GenerateAnimals()
    {
        animalGenerateSystem.GenerateAnimals(animalsList);
    }

    public void IsMacthAndDestroy()
    {
        matchSystem.IsMatchAndDestroy();
        animalDestroySystem.DestroyAnimals();
    }

    public bool IsMacthed()
    {
        return matchSystem.IsMatchedVertical(MatchMode.Check)
               || matchSystem.IsMatchedHorizon(MatchMode.Check);
    }

    Animal GetAnimal(int x, int y)
    {
        if (animalsList.Count > x && animalsList[x].Count > y)
            return animalsList[x][y].GetComponent<Animal>();

        Debug.LogWarning("인덱스 없으면 여기로 옴");
        return null;
    }

    public void OnCompleteDestroyAnimal(GameObject animal, int index)
    {
        animalGenerateSystem.OnCompleteDestroyAnimal(animalsList, animal, index);
    }

    public bool IsMoving()
    {
        return animalMoveSystem.IsMoving(row, column);
    }

    public void SwitchingAnimal(Transform animal1, Transform animal2)
    {
        animalMoveSystem.SwitchingAnimal(animalsList, animal1, animal2);
    }
}
