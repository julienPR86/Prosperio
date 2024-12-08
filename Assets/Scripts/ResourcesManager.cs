using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesManager : MonoBehaviour
{
    //Resources data
    [SerializeField] private int woodCount = 10;
    [SerializeField] private int stoneCount = 10;
    [SerializeField] private int foodCount = 10;
    [SerializeField] private int builderCount = 5;

    //Will be used to update Building UI, at the end of day(after gathering) AND after building
    public bool HasEnoughResources(BuildingCostData costData)
    {
        return (woodCount >= costData.woodCost &&
                stoneCount >= costData.stoneCost &&
                builderCount >= costData.builderCost);
    }

    //Spend resources (building)
    public void SpendResource(BuildingCostData costData)
    {
        woodCount = Mathf.Max(0, woodCount - costData.woodCost);
        stoneCount = Mathf.Max(0, stoneCount - costData.stoneCost);
        builderCount = Mathf.Max(0, builderCount - costData.builderCost);
    }

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
        foodCount += food;
    }

    public void AddWood(int wood)
    {
        woodCount += wood;
    }

    public void AddStone(int stone)
    {
        stoneCount += stone;
    }

    //Spending resources methods
    public void CousumeFood(int food)
    {
        foodCount = Mathf.Max(0, foodCount - food);
    }

    public int GetBuilderCount()
    {
        return builderCount;
    }

    public void AddBuilder(int count)
    {
        builderCount += count;
    }

    public void RemBuilder(int count)
    {
        builderCount = Mathf.Max(0, builderCount -= count);
    }
}
