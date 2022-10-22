using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchSystem
{
    Func<int, int, Animal> getAnimal;
    Action<Animal, Animal, Animal> addtoDestroyAnimals;
    int row;
    int column;

    public void Initialize(int row, int column, Func<int, int, Animal> getAnimal, Action<Animal, Animal, Animal> addtoDestroyAnimals)
    {
        this.row = row;
        this.column = column;
        this.getAnimal = getAnimal;
        this.addtoDestroyAnimals = addtoDestroyAnimals;
    }

    public void IsMatchAndDestroy()
    {
        IsMatchedVertical(MatchMode.CheckAndDestroy);
        IsMatchedHorizon(MatchMode.CheckAndDestroy);
    }

    public bool IsMatchedVertical(MatchMode matchmode)
    {
        Animal first, second, third;
        for (int x = 0; x < column; x++)
        {
            for (int y = 0; y < row - 2; y++)
            {
                first = getAnimal?.Invoke(x, y);
                second = getAnimal?.Invoke(x, y + 1);
                third = getAnimal?.Invoke(x, y + 2);

                if (first.name == second.name && second.name == third.name)
                {
                    switch (matchmode)
                    {
                        case MatchMode.Check:
                            return true;
                        case MatchMode.CheckAndDestroy:
                            addtoDestroyAnimals?.Invoke(first, second, third);
                            break;
                    }
                }
            }
        }

        return false;
    }

    public bool IsMatchedHorizon(MatchMode matchmode)
    {
        Animal first, second, third;
        for (int x = 0; x < column - 2; x++)
        {
            for (int y = 0; y < row; y++)
            {
                first = getAnimal?.Invoke(x, y);
                second = getAnimal?.Invoke(x + 1, y);
                third = getAnimal?.Invoke(x + 2, y);

                if (first.name == second.name && second.name == third.name)
                {
                    switch (matchmode)
                    {
                        case MatchMode.Check:
                            return true;
                        case MatchMode.CheckAndDestroy:
                            addtoDestroyAnimals?.Invoke(first, second, third);
                            break;
                    }
                }
            }
        }
        return false;
    }
}
