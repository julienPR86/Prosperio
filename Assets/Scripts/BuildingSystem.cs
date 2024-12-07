using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;
using static Cell;

//TO DO :

// - Add connect with logic of grid (cells[,])
// - Add terrain control (need logic connexion) and color change (to indicate)
// - Add resources consumation

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

    //Tile utilisée pour preview (avant build)
    RuleTile curBuildingTile;
    BuildingType curBuildingType;

    //Booléen pour savoir si un bâtiment est en preview ou non (phase avant build)
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

                //Add preview tile on previewTilemap
                previewTilemap.SetTile(curMouseGridPosition, curBuildingTile);

                //Update la couleur de la tile preview
                ChangePreviewTileColor();

                //Update last mousegrid position
                lastMouseGridPosition = curMouseGridPosition;
            }

            //Player do right click (--> confirm placement)
            if (Input.GetMouseButtonDown(0))
            {
                //Terrain control
                if (CanBuild())
                {
                    //Clear preview tilemap
                    previewTilemap.ClearAllTiles();

                    //Connect to logic grid
                    Cell selectedCell = gridManager.cells[curMouseGridPosition.x, curMouseGridPosition.y];
                    selectedCell.buildable = false;
                    selectedCell.buildingInCell = curBuildingType;

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



    //Méthode pour préparer preview
    public void InitBuilding(BuildingType buildingType)
    {
        curBuildingType = buildingType;

        //MAJ tilePreview en fonction de buildingType
        switch (buildingType)
        {
            case BuildingType.Home:
                curBuildingTile = homeTile;
                break;
            case BuildingType.Farm:
                curBuildingTile = farmTile;
                break;
            case BuildingType.School:
                curBuildingTile = schoolTile;
                break;
            case BuildingType.Library:
                curBuildingTile = libraryTile;
                break;
            case BuildingType.Museum:
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

        //Update la couleur de la tile preview
        ChangePreviewTileColor();

        //On passe en mode preview
        previewMode = true;
    }

    //Regarde si on peut construire sur la tile ciblee
    private bool CanBuild()
    {
        if (isInBound())
        {
            Cell selectedCell = gridManager.cells[curMouseGridPosition.x, curMouseGridPosition.y];

            if ((selectedCell.typeOfTerrain == TypeOfTerrain.Plains) && selectedCell.buildable)
            {
                return true;
            }
            else return false;
        }

        return false;
    }

    //Change de couleur la tile pour indiquer au joueur qu'il peut placer le bâtiment ou non
    private void ChangePreviewTileColor()
    {
        if (CanBuild())
        {
            previewTilemap.SetColor(curMouseGridPosition, validColor);
        }
        else previewTilemap.SetColor(curMouseGridPosition, invalidColor);
    }

    //Verifie que la position de la souris est sur la grille/le terrain
    private bool isInBound()
    {
        return (curMouseGridPosition.x >= 0 && curMouseGridPosition.x < gridManager.width && curMouseGridPosition.y >= 0 && curMouseGridPosition.y < gridManager.height);
    }
    
}
