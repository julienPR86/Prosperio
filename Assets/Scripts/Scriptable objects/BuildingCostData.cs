using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingCostData", menuName = "ScriptableObjects/BuildingCostData")]
public class BuildingCostData : ScriptableObject
{
    public int woodCost;
    public int stoneCost;
    public int builderCost;
    public int buildingTime;
}
