using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesManager : MonoBehaviour
{
    //Resources data
    [SerializeField] private int woodCount = 10;
    [SerializeField] private int stoneCount = 10;
    [SerializeField] private int foodCount = 10;
    [SerializeField] private int builderCount = 5;

    [SerializeField] private BuildingCostData homeCostData;
    [SerializeField] private BuildingCostData farmCostData;
    [SerializeField] private BuildingCostData schoolCostData;
    [SerializeField] private BuildingCostData libraryCostData;
    [SerializeField] private BuildingCostData museumCostData;


    public TextMeshProUGUI foodtext;
    public TextMeshProUGUI woodtext;
    public TextMeshProUGUI stonetext;
    public TextMeshProUGUI builderText;

    public Button buildHomeButton;
    public Button buildFarmButton;
    public Button buildSchoolButton;
    public Button buildLibraryButton;
    public Button buildMuseumButton;

    /// <summary>
    /// Will be used to update Building UI, at the end of day(after gathering) AND after building
    /// </summary>
    /// <param name="costData"></param>
    /// <returns></returns>
    /// 
    private void Start()
    {
        foodText.text = "Food:\n" + foodCount.ToString();
        woodText.text = "Wood:\n" + woodCount.ToString();
        stoneText.text = "Stone:\n" + stoneCount.ToString();
    }

    /// <summary>
    /// Update the build buttons interactable state
    /// </summary>
    void Update()
    {
        UpdateBuildButtons(homeCostData, farmCostData, schoolCostData, libraryCostData, museumCostData);
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
        woodText.text = "Wood:\n" + woodCount.ToString();

        stoneCount = Mathf.Max(0, stoneCount - costData.stoneCost);
        stoneText.text = "Stone:\n" + stoneCount.ToString();

        builderCount = Mathf.Max(0, builderCount - costData.builderCost);
        builderText.text = builderCount.ToString();
    }



    /// <summary>
    /// Update the build buttons interactable state
    /// </summary>
    /// <param name="homeCost"></param>
    /// <param name="farmCost"></param>
    /// <param name="schoolCost"></param>
    /// <param name="libraryCost"></param>
    /// <param name="museumCost"></param>
    public void UpdateBuildButtons(BuildingCostData homeCost, BuildingCostData farmCost, BuildingCostData schoolCost, BuildingCostData libraryCost, BuildingCostData museumCost)
    {
        buildHomeButton.interactable = HasEnoughResources(homeCost);
        buildFarmButton.interactable = HasEnoughResources(farmCost);
        buildSchoolButton.interactable = HasEnoughResources(schoolCost);
        buildLibraryButton.interactable = HasEnoughResources(libraryCost);
        buildMuseumButton.interactable = HasEnoughResources(museumCost);
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
        foodText.text = "Food:\n" + foodCount.ToString();
    }

    public void AddWood(int wood)
    {
        woodCount += wood;
        woodText.text = "Wood:\n" + woodCount.ToString();
    }

    public void AddStone(int stone)
    {
        stoneCount += stone;
        stoneText.text = "Stone:\n" + stoneCount.ToString();
    }

    //Spending resources methods
    public void CousumeFood(int food)
    {
        foodCount = Mathf.Max(0, foodCount - food);
        foodText.text = "Food:\n" + foodCount.ToString();
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
