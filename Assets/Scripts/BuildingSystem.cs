using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;
using static Cell;
using static UnityEngine.GraphicsBuffer;

public class BuildingSystem : MonoBehaviour
{
    GridManager gridManager;
    ResourcesManager resourcesManager;

    //ref des tilemap
    [SerializeField] Tilemap groundTilemap;
    [SerializeField] Tilemap previewTilemap;
    [SerializeField] Tilemap buildingTilemap;

    //Color of preview tile (if building is valid or not)
    [SerializeField] Color validColor = Color.green;
    [SerializeField] Color invalidColor = Color.red;

    //building tile sprite
    [SerializeField] RuleTile homeTile, farmTile, schoolTile, libraryTile, museumTile;
    //building cost data
    [SerializeField] BuildingCostData farmCostData, homeCostData, schoolCostData, libraryCostData, museumCostData;

    //Selected building tile and type
    RuleTile curBuildingTile;
    BuildingType curBuildingType;
    BuildingCostData curBuildingCostData;

    //preview mode or not
    private bool previewMode;

    private Vector3Int curMouseGridPosition = Vector3Int.zero;
    private Vector3Int lastMouseGridPosition = Vector3Int.zero;

    private void Awake()
    {
        gridManager = GetComponent<GridManager>();
        resourcesManager = GetComponent<ResourcesManager>();
        
    }

    private void Start()
    {
        curBuildingType = BuildingType.None;
        curBuildingTile = null;
        curBuildingCostData = null;

        previewMode = false;
    }

    void Update()
    {
        if (previewMode)
        {
            //Get mouse world position
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //Get mouse grid position
            curMouseGridPosition = previewTilemap.WorldToCell(mouseWorldPosition);

            //If mouse position has changed since last control
            if (curMouseGridPosition != lastMouseGridPosition)
            {
                //CLear preview tilemap
                previewTilemap.ClearAllTiles();

                //Add current tile on previewTilemap
                previewTilemap.SetTile(curMouseGridPosition, curBuildingTile);

                //Update color of current tile (preview)
                ChangePreviewTileColor();

                //Update last mouse grid position
                lastMouseGridPosition = curMouseGridPosition;
            }

            //Player do right click (--> confirm placement)
            if (Input.GetMouseButtonDown(0))
            {
                if (CanBuild())
                {
                    //Clear preview tilemap
                    previewTilemap.ClearAllTiles();

                    //Connect to logic grid
                    Cell selectedCell = gridManager.cells[curMouseGridPosition.x, curMouseGridPosition.y];
                    selectedCell.buildable = false;

                    //save building in cell
                    selectedCell.buildingInCell = curBuildingType;

                    //Spend resources
                    resourcesManager.SpendResource(curBuildingCostData);

                    //Set the tile on ground tilemap
                    buildingTilemap.SetTile(curMouseGridPosition, curBuildingTile);

                    //Leave the preview mode
                    previewMode = false;
                }

                
            }


            //Player leave the building mode (CANCEL PREVIEW)
            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
            {
                previewTilemap.ClearAllTiles();
                previewMode = false;
            }
        }

        //---------------------  Input for tests
        if (Input.GetKeyDown(KeyCode.Space))
        {
            InitBuilding(BuildingType.Farm);
        }
        //---------------------
    }



    //Method to prepare preview of building
    public void InitBuilding(BuildingType buildingType)
    {
        curBuildingType = buildingType;

        //Update current tile(preview) according to buildingType
        switch (buildingType)
        {
            case BuildingType.Home:
                curBuildingCostData = homeCostData;
                curBuildingTile = homeTile;
                break;
            case BuildingType.Farm:
                curBuildingCostData = farmCostData;
                curBuildingTile = farmTile;
                break;
            case BuildingType.School:
                curBuildingCostData = schoolCostData;
                curBuildingTile = schoolTile;
                break;
            case BuildingType.Library:
                curBuildingCostData = libraryCostData;
                curBuildingTile = libraryTile;
                break;
            case BuildingType.Museum:
                curBuildingCostData = museumCostData;
                curBuildingTile = museumTile;
                break;
            default:
                return;
        }

        //Get mouse world position
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Get mouse grid position
        curMouseGridPosition = previewTilemap.WorldToCell(mouseWorldPosition);
        lastMouseGridPosition = curMouseGridPosition;

        //Set the tile on preview tilemap
        previewTilemap.SetTile(curMouseGridPosition, curBuildingTile);

        //Update color of current tile (preview)
        ChangePreviewTileColor();

        //active preview mode
        previewMode = true;
    }

    //Check if building on current tile is possible
    private bool CanBuild()
    {
        if (isInBound())
        {
            //Get cell according to target tile
            Cell selectedCell = gridManager.cells[curMouseGridPosition.x, curMouseGridPosition.y];

            //If is plains and is buildable
            if ((selectedCell.typeOfTerrain == TypeOfTerrain.Plains) && selectedCell.buildable)
            {
                return true;
            }
            else return false;
        }

        return false;
    }

    //Check if current tile is in grid bounds
    private bool isInBound()
    {
        return (curMouseGridPosition.x >= 0 && curMouseGridPosition.x < gridManager.width && curMouseGridPosition.y >= 0 && curMouseGridPosition.y < gridManager.height);
    }
    
    //Change color of current tile to indicate to player if building is possible here
    private void ChangePreviewTileColor()
    {
        if (CanBuild())
        {
            previewTilemap.SetColor(curMouseGridPosition, validColor);
        }
        else previewTilemap.SetColor(curMouseGridPosition, invalidColor);
    }
}
