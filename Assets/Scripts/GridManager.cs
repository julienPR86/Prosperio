using UnityEngine;
using UnityEngine.Tilemaps;
using static Cell;

public class GridManager : MonoBehaviour
{
    //Grid dimensions
    public int width;
    public int height;

    //Tilemap reference
    [SerializeField] Tilemap tilemap;

    //Cells grid
    Cell[,] cells;

    //tile sprite references
    [SerializeField] RuleTile plainTile, forestTile, stoneTile, berriesTile;


    void Start()
    {
        cells = new Cell[width, height];
        GenerateGrid();
    }


    /// <summary>
    /// Generate the grid according to dimensions : Fill "cells" array and create all tiles which match with
    /// </summary>
    private void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //----- LOGIC PART

                TypeOfTerrain typeOfTerrain;

                //------- Random tile generation ------ (to upgrade)

                //FOR NOW -> RANDOM
                //Plains : 50% | Berries : 20% | Forest : 15% | Stone : 15% 

                int myrand = Random.Range(1, 101);

                if (myrand >= 1 && myrand <= 50)
                {
                    typeOfTerrain = TypeOfTerrain.Plains;
                }
                else if (myrand >= 51 && myrand <= 70)
                {
                    typeOfTerrain = TypeOfTerrain.Berries;
                }
                else if (myrand >= 71 && myrand <= 85)
                {
                    typeOfTerrain = TypeOfTerrain.Forest;
                }
                else
                {
                    typeOfTerrain = TypeOfTerrain.Stone;
                }
                //---------------------------------------

                cells[x, y] = new Cell(x, y, typeOfTerrain);

                //----- VISUAL PART

                //Tile (visual of cell)
                RuleTile tile;

                //Select the right tile
                switch (typeOfTerrain)
                {
                    case TypeOfTerrain.Forest:
                        tile = forestTile;
                        break;
                    case TypeOfTerrain.Stone:
                        tile = stoneTile;
                        break;
                    case TypeOfTerrain.Berries:
                        tile = berriesTile;
                        break;
                    default:
                        tile = plainTile;
                        break;
                }

                //Set tile on Tilemap
                tilemap.SetTile(new Vector3Int(x, y), tile);
            }
        }
    }


}


