using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using static BuildingSystem;
using static Cell;
using static UnityEngine.GraphicsBuffer;

public class BuildingSystem : MonoBehaviour
{
    GridManager gridManager;
    ResourcesManager resourcesManager;
    ManageClock clock;

    //ref des tilemap
    [SerializeField] Tilemap groundTilemap;
    [SerializeField] Tilemap previewTilemap;
    [SerializeField] Tilemap buildingTilemap;

    //Color of preview tile (if building is valid or not)
    [SerializeField] Color validColor = Color.green;
    [SerializeField] Color invalidColor = Color.red;

    //building tile sprite
    [SerializeField] RuleTile homeTile, farmTile, schoolTile, libraryTile, museumTile;
    //building in progress tile
    [SerializeField] RuleTile buildingInProgressTile;
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

    //List that contain all buildings in progress (to check/update each day)
    public List<BuildingInProgress> buildingsInProgress;

    private void Awake()
    {
        gridManager = GetComponent<GridManager>();
        resourcesManager = GetComponent<ResourcesManager>();
        clock = GetComponent<ManageClock>();
    }

    private void Start()
    {
        curBuildingType = BuildingType.None;
        curBuildingTile = null;
        curBuildingCostData = null;
        buildingsInProgress = new List<BuildingInProgress>();

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

                    //Spend resources
                    resourcesManager.SpendResource(curBuildingCostData);

                    //Set building in progress tile (to remember to player that a building is in progress at this position)
                    buildingTilemap.SetTile(curMouseGridPosition, buildingInProgressTile);

                    BuildingInProgress curBuilding = new BuildingInProgress(curMouseGridPosition, curBuildingTile, curBuildingCostData.buildingTime, clock.day);
                    buildingsInProgress.Add(curBuilding);

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
        if (Input.GetKeyDown(KeyCode.B))
        {
            InitBuilding(BuildingType.Farm);
        }
        //---------------------
    }



    /// <summary>
    /// Method to prepare preview of building
    /// </summary>
    /// <param name="buildingType">
    /// Type of building
    /// </param>
    public void InitBuilding(BuildingType buildingType)
    {
        curBuildingType = buildingType;

        //Update current tile(preview) according to buildingType
        switch (buildingType)
        {
            case BuildingType.Home:
                curBuildingCostData = homeCostData;
                curBuildingTile = homeTile;
                Debug.Log("Home");
                break;
            case BuildingType.Farm:
                curBuildingCostData = farmCostData;
                curBuildingTile = farmTile;
                Debug.Log("Farm");
                break;
            case BuildingType.School:
                curBuildingCostData = schoolCostData;
                curBuildingTile = schoolTile;
                Debug.Log("School");
                break;
            case BuildingType.Library:
                curBuildingCostData = libraryCostData;
                curBuildingTile = libraryTile;
                Debug.Log("Library");
                break;
            case BuildingType.Museum:
                curBuildingCostData = museumCostData;
                curBuildingTile = museumTile;
                Debug.Log("Museum");
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

    /// <summary>
    /// Method to handle button click event
    /// </summary>
    /// <param name="buildingTypeString"></param>
    public void OnBuildButtonClicked(string buildingTypeString)
    {
        if (System.Enum.TryParse(buildingTypeString, out BuildingType buildingType))
        {
            InitBuilding(buildingType);
        }
        else
        {
            Debug.LogError("Invalid building type: " + buildingTypeString);
        }
    }


    /// <summary>
    /// Check if building on current tile is possible
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Check if current tile is in grid bounds
    /// </summary>
    /// <returns></returns>
    private bool isInBound()
    {
        return (curMouseGridPosition.x >= 0 && curMouseGridPosition.x < gridManager.width && curMouseGridPosition.y >= 0 && curMouseGridPosition.y < gridManager.height);
    }

    /// <summary>
    /// Change color of current tile to indicate to player if building is possible here
    /// </summary>
    private void ChangePreviewTileColor()
    {
        if (CanBuild())
        {
            previewTilemap.SetColor(curMouseGridPosition, validColor);
        }
        else previewTilemap.SetColor(curMouseGridPosition, invalidColor);
    }

    /// <summary>
    /// Method to update building in progress (to call each day)
    /// </summary>
    public void UpdateBuildingsInProgress()
    {
        int curDay = clock.day;

        foreach (BuildingInProgress building in buildingsInProgress)
        {
            if (curDay >= building.startDay + building.constructionTime)
            {
                //Complete building

                //save building in cell
                Cell curCell = gridManager.cells[building.position.x, building.position.y];
                curCell.buildingInCell = curBuildingType;

                //Set the tile on ground tilemap
                buildingTilemap.SetTile(building.position, building.finalTile);

                buildingsInProgress.Remove(building);
            }
        }
    }

    /// <summary>
    /// Class that represent building in progress
    /// </summary>
    public class BuildingInProgress
    {
        public Vector3Int position;
        public RuleTile finalTile;
        public int startDay;
        public int constructionTime;

        public BuildingInProgress(Vector3Int _position, RuleTile _finalTile, int _constructionTime, int _startDay)
        {
            this.position = _position;
            this.finalTile = _finalTile;
            this.constructionTime = _constructionTime;
            this.startDay = _startDay;
        }
    }
}
