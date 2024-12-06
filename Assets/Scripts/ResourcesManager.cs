using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager : MonoBehaviour
{
    [SerializeField] private int woodCount = 10;
    [SerializeField] private int stoneCount = 10;
    [SerializeField] private int foodCount = 10;

    //Getting resources methods
    public int GetFoodCount()
    {
        return foodCount;
    }

    public int GetWoodCount()
    {
        return woodCount;
    }

    public int GetStoneCount()
    {
        return stoneCount;
    }

    //Adding resources methods
    public void AddFood(int food)
    {
        foodCount++;
    }

    public void AddWood(int wood)
    {
        woodCount++;
    }

    public void AddStone(int stone)
    {
        stoneCount++;
    }

    //Spending resources methods
    public void TakeFood(int food)
    {
        foodCount--;
    }

    public void TakeWood(int wood)
    {
        woodCount--;
    }

    public void TakeStone(int stone)
    {
        stoneCount--;
    }
}
