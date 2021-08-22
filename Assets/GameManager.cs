using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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
    public ArrayList[] animals;
    List<Animal> toDestroyAnimals = new List<Animal>();

    bool isPlaying = false;
    IEnumerator Start()
    {
        animalParent = GameObject.Find("AnimalParent").transform;
        animalGo = (GameObject)Resources.Load(animalGoString);

        animalScaleX = animalGo.transform.localScale.x;
        var animalColSize = animalGo.GetComponent<BoxCollider>().size;
        xGap = animalColSize.x;
        yGap = animalColSize.y;
        GenerateAnimals();

        isPlaying = true;
        yield return null;
        while (isPlaying)
        {
            while (IsMoving() == false)
            {
                toDestroyAnimals.Clear();
                IsMatchedVertical();
                IsMatchedHorizon();
                DestroyAnimals();

                // Wait 0.5f, cuz DestroyAnimation Lengh = 0.5f
                yield return new WaitForSeconds(0.5f);
                RemoveNull();
            }
            yield return null;
        }
    }

    private void RemoveNull()
    {
        for (int x = 0; x < animals.Length; x++)
            for (int y = 0; y < animals[x].Count; y++)
                if (animals[x][y] == null)
                    animals[x].RemoveAt(y);
    }

    void DestroyAnimals()
    {
        toDestroyAnimals.ForEach((x) => x.Destroy());
    }

    void IsMatchedVertical()
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
                    AddtoDestroyAnimals(first, second, third);
            }
        }
    }
    void IsMatchedHorizon()
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
                    AddtoDestroyAnimals(first, second, third);
            }
        }
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
        animals = new ArrayList[column];
        for (int x = 0; x < column; x++)
        {
            animals[x] = new ArrayList();
            for (int y = 0; y < row; y++)
            {
                animals[x].Add(CreateAnimal(x, y));
            }
        }
    }

    GameObject CreateAnimal(int x, int y)
    {
        var pos = new Vector3(x * xGap * animalScaleX + 0.1f, y * yGap + 0.1f);
        var newGo = Instantiate(animalGo, pos, Quaternion.identity, animalParent);
        newGo.GetComponent<Animal>().Index = x;
        return newGo;
    }

    Animal GetAnimal(int x, int y)
    {
        if (animals.Length > x && animals[x].Count > y)
            return ((GameObject)animals[x][y]).GetComponent<Animal>();

        Debug.LogWarning("�ε��� ������ ����� ��");
        return null;
    }

    public void Remove(Animal animal, int index)
    {
        animals[index].Remove(animal);
    }
    public void Reborn(int index)
    {
        animals[index].Add(CreateAnimal(index, animals[index].Count));
    }
}
