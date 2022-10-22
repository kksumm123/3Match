using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalDestroySystem
{
    List<Animal> toDestroyAnimals;
    Action<int> onDestroyAnimal;

    public void Initialize(List<Animal> toDestroyAnimals, Action<int> onDestroyAnimal)
    {
        this.toDestroyAnimals = toDestroyAnimals;
        this.onDestroyAnimal = onDestroyAnimal;
    }

    public void AddtoDestroyAnimals(Animal first, Animal second, Animal third)
    {
        _AddtoDestroyAnimals(first, second, third);

    }

    void _AddtoDestroyAnimals(params Animal[] animals)
    {
        foreach (var item in animals)
        {
            if (toDestroyAnimals.Contains(item) == false)
                toDestroyAnimals.Add(item);
        }
    }


    public void DestroyAnimals()
    {
        var count = toDestroyAnimals.Count;
        if (count > 0)
        {
            onDestroyAnimal?.Invoke(count);
            SoundManager.Instance.PlaySFX();
            ScoreUI.Instance.AddScore(count);
            toDestroyAnimals.ForEach((x) => x.Destroy());
            toDestroyAnimals.Clear();
        }
    }
}
