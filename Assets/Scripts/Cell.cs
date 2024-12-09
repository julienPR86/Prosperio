using UnityEngine;

public class Cell
{
    //attributs
    public int x { get; set; }
    public int y { get; set; }

    public Vector3 WorldPosition;
    public bool buildable { get; set; }
    public BuildingType buildingInCell;

    public TypeOfTerrain typeOfTerrain { get; set; }

    //Data for home
    private int peopleCapacity;
    private int peopleCount;
    public bool isFull;

    //constructeur
    public Cell(int x, int y, TypeOfTerrain typeOfTerrain, Vector3 worldPosition)
    {
        this.x = x;
        this.y = y;
        this.typeOfTerrain = typeOfTerrain;
        this.WorldPosition = worldPosition;
        this.peopleCapacity = 4;
        this.peopleCount = 0;
        this.isFull = false;

        //buildable only if plains
        this.buildable = (typeOfTerrain == TypeOfTerrain.Plains);
        //No building
        this.buildingInCell = BuildingType.None;
    }

    //Méthodes
    public void AddPeople()
    {
        peopleCount++;

        if (peopleCount >= peopleCapacity)
        {
            isFull = true;
        }
    }

    public void RemPeople()
    {
        peopleCount--;

        if (peopleCount < peopleCapacity)
        {
            isFull = false;
        }
    }
    //Terrain type
    public enum TypeOfTerrain
    {
        Plains,
        Forest,
        Berries,
        Stone,
    }

    public enum BuildingType
    {
        None,
        Home,
        Farm,
        School,
        Library,
        Museum,
    }
}
