using UnityEngine;

public class Cell
{
    //attributs
    public int x { get; set; }
    public int y { get; set; }
    public bool buildable { get; set; }
    public TypeOfTerrain typeOfTerrain { get; set; }

    //constructeur
    public Cell(int x, int y, TypeOfTerrain typeOfTerrain)
    {
        this.x = x;
        this.y = y;
        this.typeOfTerrain = typeOfTerrain;

        //buildable only if plains
        this.buildable = (typeOfTerrain == TypeOfTerrain.Plains);
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
