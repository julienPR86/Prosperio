using UnityEngine;

public class Cell
{
    //attributs
    public int x { get; set; }
    public int y { get; set; }

    public Vector3 WorldPosition;
    public bool buildable { get; set; }
    public TypeOfTerrain typeOfTerrain { get; set; }

    //constructeur
    public Cell(int x, int y, TypeOfTerrain typeOfTerrain, Vector3 worldPosition)
    {
        this.x = x;
        this.y = y;
        this.typeOfTerrain = typeOfTerrain;

        //buildable only if plains
        this.buildable = (typeOfTerrain == TypeOfTerrain.Plains);
        WorldPosition = worldPosition;
    }


    //Terrain type
    public enum TypeOfTerrain
    {
        Plains,
        Forest,
        Berries,
        Stone,
    }
}
