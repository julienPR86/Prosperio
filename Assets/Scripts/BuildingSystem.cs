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

    //building tile sprite
    [SerializeField] RuleTile homeTile, farmTile, schoolTile, libraryTile, museumTile;

    //Tile utilisée pour preview (avant build)
    RuleTile previewTile;

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
                previewTilemap.SetTile(curMouseGridPosition, previewTile);

                //Update last mousegrid position
                lastMouseGridPosition = curMouseGridPosition;
            }

            //Player do right click (--> confirm placement)
            //TO DO : Control on placement (need logic connexion)
            if (Input.GetMouseButtonDown(0))
            {
                //Clear preview tilemap
                previewTilemap.ClearAllTiles();

                //Set the tile on ground tilemap
                buildingTilemap.SetTile(curMouseGridPosition, previewTile);

                //Leave the preview mode
                previewMode = false;
            }


            //Player leave the building mode
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
        //MAJ tilePreview en fonction de buildingType
        switch (buildingType)
        {
            case BuildingType.Home:
                previewTile = homeTile;
                break;
            case BuildingType.Farm:
                previewTile = farmTile;
                break;
            case BuildingType.School:
                previewTile = schoolTile;
                break;
            case BuildingType.Library:
                previewTile = libraryTile;
                break;
            case BuildingType.Museum:
                previewTile = museumTile;
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
        previewTilemap.SetTile(curMouseGridPosition, previewTile);

        //On passe en mode preview
        previewMode = true;
    }

    
}
