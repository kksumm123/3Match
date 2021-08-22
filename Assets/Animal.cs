using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum AnimalType
{
    Animal1 = 0,
    Animal2,
    Animal3,
    Animal4,
    Animal5,
    Animal6,
}
public class Animal : MonoBehaviour
{
    AnimalType m_animaltype;

    public AnimalType Animaltype
    {
        get => m_animaltype;
        set => m_animaltype = value;
    }

    void Start()
    {
        Animaltype = (AnimalType)Random.Range(0, 7);
        transform.name = Animaltype.ToString();
    }
}
