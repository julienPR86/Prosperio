using System.Collections.Generic;
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
    public Cell[,] cells;
    public List<Cell> forestcells = new List<Cell>();
    public List<Cell> stonecells = new List<Cell>();
    public List<Cell> berriescells = new List<Cell>();

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

                Vector3Int cellPosition = new Vector3Int(x, y, 0);
                Vector3 cellnotcentered = tilemap.CellToWorld(cellPosition);
                Vector3 worldPosition = cellnotcentered + new Vector3(tilemap.cellSize.x / 2f, tilemap.cellSize.y / 2f, 0);

                //--------------------------------------

                cells[x, y] = new Cell(x, y, typeOfTerrain, worldPosition);

                //----- VISUAL PART

                //Tile (visual of cell)
                RuleTile tile;

                //Select the right tile
                switch (typeOfTerrain)
                {
                    case TypeOfTerrain.Forest:
                        forestcells.Add(cells[x, y]); // On stocke la cellule dans la liste correspondante à son terrain
                        tile = forestTile;
                        break;
                    case TypeOfTerrain.Stone:
                        stonecells.Add(cells[x, y]);
                        tile = stoneTile;
                        break;
                    case TypeOfTerrain.Berries:
                        berriescells.Add(cells[x, y]);
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


