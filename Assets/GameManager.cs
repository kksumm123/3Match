using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Transform animalParent;
    readonly string animalGoString = "Animal";
    GameObject animalGo;
    float xGap;
    float yGap;
    float animalScaleX;
    int row = 10;
    int column = 6;
    ArrayList[] animals;

    void Start()
    {
        animalParent = GameObject.Find("AnimalParent").transform;
        animalGo = (GameObject)Resources.Load(animalGoString);
        animalScaleX = animalGo.transform.localScale.x;
        var animalColSize = animalGo.GetComponent<BoxCollider>().size;
        xGap = animalColSize.x;
        yGap = animalColSize.y;
        GenerateAnimals();
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
        var pos = new Vector3(x * xGap * animalScaleX + 0.01f, y * yGap + 0.01f);
        var newGo = Instantiate(animalGo, pos, Quaternion.identity, animalParent);
        return newGo;
    }

    void Update()
    {

    }
}
