using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimalGenerateSystem
{
    float xGap;
    float yGap;
    [SerializeField] Transform animalParent;
    [SerializeField] Animal animalPrefab;
    float animalScaleX;
    int row;
    int column;
    Func<int, int, Animal> getAnimal;

    public void Initialize(GameManager gameManager, out float xGap, out float yGap, int row, int column, Func<int, int, Animal> getAnimal)
    {
        WoonyMethods.Asserts(gameManager,
                             (animalParent, nameof(animalParent)),
                             (animalPrefab, nameof(animalPrefab)));

        animalScaleX = animalPrefab.transform.localScale.x;
        var animalColSize = animalPrefab.GetComponent<BoxCollider>().size;
        xGap = animalColSize.x * animalScaleX + 0.01f;
        yGap = animalColSize.y + 0.01f;

        this.xGap = xGap;
        this.yGap = yGap;
        this.row = row;
        this.column = column;
        this.getAnimal = getAnimal;
    }

    public void GenerateAnimals(List<List<GameObject>> animalsList)
    {
        animalsList.Clear();
        for (int x = 0; x < column; x++)
        {
            animalsList.Add(new List<GameObject>(row));

            for (int y = 0; y < row; y++)
                animalsList[x].Add(InstantiateAnimal(x, x * xGap, y * yGap));
        }
    }

    GameObject InstantiateAnimal(int x, float posX, float posY)
    {
        var pos = new Vector3(posX, posY);
        var newGo = GameObject.Instantiate(animalPrefab, pos, Quaternion.identity, animalParent);
        newGo.Index = x;
        return newGo.gameObject;
    }

    public void OnCompleteDestroyAnimal(List<List<GameObject>> animalsList, GameObject animal, int index)
    {
        animalsList[index].Remove(animal);
        Reborn(animalsList, index);
    }

    void Reborn(List<List<GameObject>> animalsList, int index)
    {
        var newY = getAnimal?.Invoke(index, animalsList[index].Count - 1).transform.position.y;
        animalsList[index].Add(InstantiateAnimal(index, index * xGap, (float)newY + (yGap * 2)));
    }
}
