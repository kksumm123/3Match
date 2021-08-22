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

    int row = 10;
    int column = 6;
    List<List<GameObject>> animals = new List<List<GameObject>>();


    void Start()
    {
        animalParent = GameObject.Find("AnimalParent").transform;
        animalGo = (GameObject)Resources.Load(animalGoString);
        var colSize = animalGo.GetComponent<BoxCollider>().size;
        xGap = colSize.x;
        yGap = colSize.y;
        GenerateAnimals();
    }
    void GenerateAnimals()
    {
        for (int x = 0; x < column; x++)
        {
            for (int y = 0; y < row; y++)
            {
                animals[x][y] = CreateAnimal(x, y);
            }
        }
    }

    GameObject CreateAnimal(int x, int y)
    {
        var pos = new Vector3(x * xGap, y * yGap);
        var newGo = Instantiate(animalGo, pos, Quaternion.identity, animalParent);
        return newGo;
    }

    void Update()
    {

    }
}
