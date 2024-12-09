using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesManager : MonoBehaviour
{
    //Resources data
    [SerializeField] private int woodCount = 10;
    [SerializeField] private int stoneCount = 10;
    [SerializeField] private int foodCount = 10;
    [SerializeField] private int builderCount = 5;
    public TextMeshProUGUI foodtext;
    public TextMeshProUGUI woodtext;
    public TextMeshProUGUI stonetext;

    /// <summary>
    /// Will be used to update Building UI, at the end of day(after gathering) AND after building
    /// </summary>
    /// <param name="costData"></param>
    /// <returns></returns>
    /// 
    private void Start()
    {
        foodtext.text = "Food:\n" + foodCount.ToString();
        woodtext.text = "Wood:\n" + woodCount.ToString();
        stonetext.text = "Stone:\n" + stoneCount.ToString();
    }
    public bool HasEnoughResources(BuildingCostData costData)
    {
        return (woodCount >= costData.woodCost &&
                stoneCount >= costData.stoneCost &&
                builderCount >= costData.builderCost);
    }

    /// <summary>
    /// Spend resources to building
    /// </summary>
    /// <param name="costData"></param>
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
        foodtext.text = "Food:\n" + foodCount.ToString();
    }

    public void AddWood(int wood)
    {
        woodCount += wood;
        woodtext.text = "Wood:\n" + woodCount.ToString();
    }

    public void AddStone(int stone)
    {
        stoneCount += stone;
        stonetext.text = "Stone:\n" + stoneCount.ToString();
    }

    //Spending resources methods
    public void CousumeFood(int food)
    {
        foodCount = Mathf.Max(0, foodCount - food);
        foodtext.text = foodCount.ToString();
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
