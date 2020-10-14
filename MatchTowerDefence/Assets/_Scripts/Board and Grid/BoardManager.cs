using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;


/// <summary>
/// A manager class for the board on each level
/// </summary>
public class BoardManager : MonoBehaviour
{
    // TODO: REMOVE CONTROL FROM HERE
    public CameraManager camManager = null;

    // Singleton instance
    public static BoardManager instance;

    // Types of towers to spawn 
    [Header("Tower objects available for spawning")]
    [SerializeField]
    private List<TowerObject> spawnTowers = new List<TowerObject>();
    [SerializeField]
    private TowerObject AOETower = null;
    [SerializeField]
    private TowerObject NormalTower = null;
    [SerializeField]
    private TowerObject FrostTower = null;
    [SerializeField]
    private GameObject towerTilePrefab = null;

    // Tilemap info
    private Tilemap tilemap = null;
    private int xSize, ySize;

    // List of all tower tiles in game
    public List<TowerTile> allTowerTiles = null;

    // Shifting info
    [SerializeField]
    private float shiftDelay = 0.03f;
    public bool IsShifting { get; set; }
    private List<TowerTile>[] towerTileColumns; // Holds tiles in columns for shifting

    // Pathing info
    public List<PathTile> allPathTiles = null;
    public List<PathTile> allBases = null;
    public List<PathTile> allSpawns = null;

    [SerializeField]
    private GameObject pathTilePrefab = null;



    private void Awake()
    {
        IsShifting = false;
        instance = GetComponent<BoardManager>();
    }

    /// <summary>
    ///  Initialises the board manager state, instance, tile board and tower list.
    /// </summary>
    public void Initialise()
    {
        IsShifting = false;
        instance = GetComponent<BoardManager>();

        tilemap = FindObjectOfType<Tilemap>();
        if (tilemap != null)
        {
            // Obtain board bounds.
            tilemap.CompressBounds();
            xSize = tilemap.cellBounds.size.x;
            ySize = tilemap.cellBounds.size.y;

            // TODO: remove camera control from here
            camManager.SetDisplay((Mathf.Max(xSize, ySize) / 2.0f), transform.position);

            CreateTowerBoard();
        }
    }

    /// <summary>
    /// Generates the board from the tilemap.
    /// </summary>
    private void CreateTowerBoard()
    {
        xSize = tilemap.cellBounds.size.x;
        ySize = tilemap.cellBounds.size.y;

        // Init Lists
        towerTileColumns = new List<TowerTile>[tilemap.cellBounds.size.x];
        allPathTiles = new List<PathTile>();
        allTowerTiles = new List<TowerTile>();
        allSpawns = new List<PathTile>();
        allBases = new List<PathTile>();

        // Generation
        GenerateTiles();
        GenerateTowerAdjacency();
        GeneratePathTilesAdjacency();
        SetSpawnerPathways();
        SetPathTileDistance();

        allTowerTiles.AddRange(FindObjectsOfType<TowerTile>());
    }

    private void GenerateTiles()
    {
        // Set-up data
        int minRows = tilemap.cellBounds.yMin;
        int minCols = tilemap.cellBounds.xMin;
        float startX = transform.position.x - xSize / 2;
        float startY = transform.position.y - ySize / 2;

        TowerObject[] previousLeft = new TowerObject[ySize];
        // Create Tiles and fill tile lists
        for (int col = 0; col < xSize; col++)
        {
            towerTileColumns[col] = new List<TowerTile>();
            TowerObject previousBelow = null;

            for (int row = 0; row < ySize; row++)
            {
                Vector3Int tilePos = new Vector3Int(col + minCols, row + minRows, 0);

                if (tilemap.GetTile(tilePos) == null) continue;

                string tilename = tilemap.GetTile(tilePos).name;

                // If tower type then generate towers
                if (tilename == "MapTile" || tilename == "Frost0Tile" || tilename == "AOE0Tile" || tilename == "Normal0Tile")
                {
                    // Create new tile object
                    GameObject newTileObj = Instantiate(towerTilePrefab, new Vector3(startX + col, startY + row, 0), towerTilePrefab.transform.rotation);
                    newTileObj.name = "Tile_" + col.ToString() + "_" + row.ToString();
                    newTileObj.transform.parent = transform; // 1

                    towerTileColumns[col].Add(newTileObj.GetComponent<TowerTile>());

                    TowerObject newTowerObject = null;
                    if (tilename == "MapTile")
                    {
                        // Create list of possible towers
                        List<TowerObject> possibleTowers = new List<TowerObject>();
                        possibleTowers.AddRange(spawnTowers);

                        // Remove tower types on the left and bottom from possible
                        possibleTowers.Remove(previousLeft[row]);
                        possibleTowers.Remove(previousBelow);

                        // Random generation of new tower
                        newTowerObject = possibleTowers[Random.Range(0, possibleTowers.Count)];

                    }
                    else if (tilename == "Frost0Tile")
                    {
                        newTowerObject = FrostTower;
                    }
                    else if (tilename == "Normal0Tile")
                    {
                        newTowerObject = NormalTower;
                    }
                    else
                    {
                        newTowerObject = AOETower;
                    }

                    //Debug.Log(newTileObj.name + " = " + newTowerObject);
                    // Edit tile info
                    TowerTile newTile = newTileObj.GetComponent<TowerTile>();
                    newTile.boardPosition = new BoardPosition(col, row);
                    newTile.SetTower(newTowerObject);

                    // Add tile as previous object
                    previousLeft[row] = newTowerObject;
                    previousBelow = newTowerObject;

                }

                // If pathing tile create
                // TODO: can be optimised by giving 4 prefabs for each tile orientation
                // TODO: can be optimised by having a dictionary of all tilenames to tiles for generation.
                if (tilename == "PathDown" || tilename == "PathRight" || tilename == "PathUp" || tilename == "PathLeft")
                {
                    // Create new path tile object
                    GameObject newTileObj = Instantiate(pathTilePrefab, new Vector3(startX + col, startY + row, 0), pathTilePrefab.transform.rotation);
                    newTileObj.name = "Path_" + col.ToString() + "_" + row.ToString();
                    newTileObj.transform.parent = transform; // 1

                    PathTile newPathTile = newTileObj.GetComponent<PathTile>();

                    if (tilename == "PathDown")
                    {
                        newPathTile.orientation = PathTileOrientation.DOWN;
                    } 
                    else if (tilename == "PathRight")
                    {
                        newPathTile.orientation = PathTileOrientation.RIGHT;
                    }
                    else if (tilename == "PathUp")
                    {
                        newPathTile.orientation = PathTileOrientation.UP;
                    }
                    else if (tilename == "PathLeft")
                    {
                        newPathTile.orientation = PathTileOrientation.LEFT;
                    }
                    else
                    {
                        newPathTile.orientation = PathTileOrientation.NONE;
                    }

                    allPathTiles.Add(newPathTile);

                } 
                else if (tilename == "Base")
                {
                    // Create new path tile object
                    GameObject newTileObj = Instantiate(pathTilePrefab, new Vector3(startX + col, startY + row, 0), pathTilePrefab.transform.rotation);
                    newTileObj.name = "Base_" + col.ToString() + "_" + row.ToString();
                    newTileObj.transform.parent = transform; // 1

                    PathTile newPathTile = newTileObj.GetComponent<PathTile>();
                    newPathTile.orientation = PathTileOrientation.NONE;
                    allBases.Add(newPathTile);
                    allPathTiles.Add(newPathTile);

                }
                else if (tilename == "Spawn")
                {
                    // Create new path tile object
                    GameObject newTileObj = Instantiate(pathTilePrefab, new Vector3(startX + col, startY + row, 0), pathTilePrefab.transform.rotation);
                    newTileObj.name = "Spawn_" + col.ToString() + "_" + row.ToString();
                    newTileObj.transform.parent = transform; // 1

                    PathTile newPathTile = newTileObj.GetComponent<PathTile>();
                    newPathTile.orientation = PathTileOrientation.NONE;
                    allSpawns.Add(newPathTile);
                    allPathTiles.Add(newPathTile);

                }
                // ELSE is to clear all other tiles as we have no path tile visualisation at the moment.
                else
                {
                    tilemap.SetTile(tilePos, null);
                }

            }

        }
    }

    private void GenerateTowerAdjacency()
    {
        // Generate tower tile adjacency struct
        for (int i = 0; i < towerTileColumns.Length; i++)
        {
            foreach (TowerTile tile in towerTileColumns[i])
            {
                RaycastHit2D hit = Physics2D.Raycast(tile.transform.position, Vector2.left, 1.0f); // Checking for adjacency in distance of 1.0f
                if (hit.collider != null && hit.collider.gameObject.GetComponent<TowerTile>() != null)
                {
                    tile.adjTiles.left = hit.collider.gameObject.GetComponent<TowerTile>();
                }

                hit = Physics2D.Raycast(tile.transform.position, Vector2.right, 1.0f); // Checking for adjacency in distance of 1.0f
                if (hit.collider != null && hit.collider.gameObject.GetComponent<TowerTile>() != null)
                {
                    tile.adjTiles.right = hit.collider.gameObject.GetComponent<TowerTile>();
                }

                hit = Physics2D.Raycast(tile.transform.position, Vector2.up, 1.0f); // Checking for adjacency in distance of 1.0f
                if (hit.collider != null && hit.collider.gameObject.GetComponent<TowerTile>() != null)
                {
                    tile.adjTiles.up = hit.collider.gameObject.GetComponent<TowerTile>();
                }

                hit = Physics2D.Raycast(tile.transform.position, Vector2.down, 1.0f); // Checking for adjacency in distance of 1.0f
                if (hit.collider != null && hit.collider.gameObject.GetComponent<TowerTile>() != null)
                {
                    tile.adjTiles.down = hit.collider.gameObject.GetComponent<TowerTile>();
                }
            }
        }

    }

    private void GeneratePathTilesAdjacency()
    {
        foreach (PathTile tile in allPathTiles)
        {
            // LEFT
            if (tile.orientation == PathTileOrientation.LEFT)
            {
                RaycastHit2D hit = Physics2D.Raycast(tile.transform.position, Vector2.left, 1.0f);
                if (hit.collider != null && hit.collider.gameObject.GetComponent<PathTile>() != null)
                {
                    PathTile tile2 = hit.collider.gameObject.GetComponent<PathTile>();
                    tile2.previousTiles.Add(tile);
                    tile.nextTile = tile2;
                } else
                {
                    Debug.LogError("There is no path tile following orientation "
                        + PathTileOrientation.LEFT + " of " + tile.gameObject.name);
                }
            }
            // RIGHT
            else if (tile.orientation == PathTileOrientation.RIGHT)
            {
                RaycastHit2D hit = Physics2D.Raycast(tile.transform.position, Vector2.right, 1.0f);
                if (hit.collider != null && hit.collider.gameObject.GetComponent<PathTile>() != null)
                {
                    PathTile tile2 = hit.collider.gameObject.GetComponent<PathTile>();
                    tile2.previousTiles.Add(tile);
                    tile.nextTile = tile2;
                }
                else
                {
                    Debug.LogError("There is no path tile following orientation "
                        + PathTileOrientation.RIGHT + " of " + tile.gameObject.name);
                }
            }
            // UP
            else if (tile.orientation == PathTileOrientation.UP)
            {
                RaycastHit2D hit = Physics2D.Raycast(tile.transform.position, Vector2.up, 1.0f);
                if (hit.collider != null && hit.collider.gameObject.GetComponent<PathTile>() != null)
                {
                    PathTile tile2 = hit.collider.gameObject.GetComponent<PathTile>();
                    tile2.previousTiles.Add(tile);
                    tile.nextTile = tile2;
                }
                else
                {
                    Debug.LogError("There is no path tile following orientation " 
                        + PathTileOrientation.UP + " of " + tile.gameObject.name);
                }
            }
            // RIGHT
            else if (tile.orientation == PathTileOrientation.DOWN)
            {
                RaycastHit2D hit = Physics2D.Raycast(tile.transform.position, Vector2.down, 1.0f);
                if (hit.collider != null && hit.collider.gameObject.GetComponent<PathTile>() != null)
                {
                    PathTile tile2 = hit.collider.gameObject.GetComponent<PathTile>();
                    tile2.previousTiles.Add(tile);
                    tile.nextTile = tile2;
                }
                else
                {
                    Debug.LogError("There is no path tile following orientation "
                        + PathTileOrientation.DOWN + " of " + tile.gameObject.name);
                }
            }
            // Spawns
            else if (tile.orientation == PathTileOrientation.NONE && allSpawns.Contains(tile))
            {
                // down
                RaycastHit2D hit = Physics2D.Raycast(tile.transform.position, Vector2.down, 1.0f);
                if (hit.collider != null && hit.collider.gameObject.GetComponent<PathTile>() != null)
                {
                    PathTile tile2 = hit.collider.gameObject.GetComponent<PathTile>();

                    if (tile2.orientation != PathTileOrientation.NONE)
                    {
                        tile2.previousTiles.Add(tile);
                        tile.nextTile = tile2;
                    }
                }

                // left
                hit = Physics2D.Raycast(tile.transform.position, Vector2.left, 1.0f);
                if (hit.collider != null && hit.collider.gameObject.GetComponent<PathTile>() != null)
                {
                    PathTile tile2 = hit.collider.gameObject.GetComponent<PathTile>();

                    if (tile2.orientation != PathTileOrientation.NONE)
                    {
                        tile2.previousTiles.Add(tile);
                        tile.nextTile = tile2;
                    }
                }

                // right
                hit = Physics2D.Raycast(tile.transform.position, Vector2.right, 1.0f);
                if (hit.collider != null && hit.collider.gameObject.GetComponent<PathTile>() != null)
                {
                    PathTile tile2 = hit.collider.gameObject.GetComponent<PathTile>();

                    if (tile2.orientation != PathTileOrientation.NONE)
                    {
                        tile2.previousTiles.Add(tile);
                        tile.nextTile = tile2;
                    }
                }

                // up
                hit = Physics2D.Raycast(tile.transform.position, Vector2.up, 1.0f);
                if (hit.collider != null && hit.collider.gameObject.GetComponent<PathTile>() != null)
                {
                    PathTile tile2 = hit.collider.gameObject.GetComponent<PathTile>();

                    if (tile2.orientation != PathTileOrientation.NONE)
                    {
                        tile2.previousTiles.Add(tile);
                        tile.nextTile = tile2;
                    }
                }
            }
        }
    }

    private void SetPathTileDistance()
    {

        foreach (PathTile basetile in allBases)
        {
            basetile.distFromBase = 0;

            Queue<PathTile> tilesToCalculate = new Queue<PathTile>();
            tilesToCalculate.Enqueue(basetile);

            while (tilesToCalculate.Count > 0)
            {
                PathTile curPT = tilesToCalculate.Dequeue();

                for (int i = 0; i < curPT.previousTiles.Count; i++)
                {
                    PathTile prevPT = curPT.previousTiles[i];
                    tilesToCalculate.Enqueue(prevPT);
                    prevPT.distFromBase = curPT.distFromBase + 1;
                }
            }
        }

    }
    private void SetSpawnerPathways()
    {
        List<Spawner> spawners = new List<Spawner>(FindObjectsOfType<Spawner>(true));

        foreach(Spawner spawner in spawners)
        {
            for(int i = 0; i < allSpawns.Count; i++)
            {
                if (Vector3.Distance(spawner.transform.position, allSpawns[i].transform.position) <= 0.6f)
                {
                    spawner.spawnPathTile = allSpawns[i];
                }
            }
        }

    }
    /// <summary>
    /// Finds empty tiles (after tile clearing) to begin shifting of towers.
    /// Afterwards find if any new matches occur.
    /// </summary>
    // TODO: Possible bug from Coroutine(ShiftTowerDown) and next for loop
    public IEnumerator FindNullTiles()
    {
        for (int x = 0; x < towerTileColumns.Length; x++)
        {
            for (int y = 0; y < towerTileColumns[x].Count; y++)
            {
                if (towerTileColumns[x][y].GetComponent<TowerTile>().tower == null)
                {
                    yield return StartCoroutine(ShiftTowersDown(x, y));
                    break;
                }
            }
        }

        for (int x = 0; x < towerTileColumns.Length; x++)
        {
            for (int y = 0; y < towerTileColumns[x].Count; y++)
            {
                towerTileColumns[x][y].GetComponent<TowerTile>().FindMatch();
            }
        }
    }

    /// <summary>
    /// Shifts towers down in the X column board. Starts from the YSTART
    /// and goes up to shift everything down.
    /// </summary>
    /// <param name="x"> The column board coordinate of the tower.</param>
    /// <param name="yStart">The start row board coordinate of the tower.</param>
    /// <returns></returns>
    private IEnumerator ShiftTowersDown(int x, int yStart)
    {
        IsShifting = true;
        List<TowerTile> shiftTiles = new List<TowerTile>();

        for (int y = yStart; y < towerTileColumns[x].Count; y++)
        {  // 1
            TowerTile tile = towerTileColumns[x][y].GetComponent<TowerTile>();
            shiftTiles.Add(tile);
        }

        while (yStart < towerTileColumns[x].Count)
        {
            //   Debug.Log("For X,Ystart: " + x + " " + yStart);

            var tile = towerTileColumns[x][yStart].GetComponent<TowerTile>();
            if (tile.tower != null)
            {
                yStart++;
                shiftTiles.RemoveAt(0);
                continue;
            }

            while (tile.tower == null)
            {
                yield return new WaitForSeconds(shiftDelay);
                int k = 0;
                for (k = 0; k < shiftTiles.Count - 1; k++)
                { // 5
                    shiftTiles[k].SetTower(shiftTiles[k + 1].tower, shiftTiles[k + 1].towerBonusDamage);
                    shiftTiles[k + 1].SetTower(null); // 6
                }
                shiftTiles[k].SetTower(GetNewTower(shiftTiles[k]));
            }
            shiftTiles.RemoveAt(0);
            yStart++;

        }
        IsShifting = false;

    }


    /// <summary>
    /// Generates a new tower at the given tile position.
    /// </summary>
    /// <param name="tile">The tile where to generate a new tower</param>
    /// <returns> New tower from possible tower list </returns>
    private TowerObject GetNewTower(TowerTile tile)
    {
        List<TowerObject> possibleTowers = new List<TowerObject>();
        possibleTowers.AddRange(spawnTowers);

        if (tile.adjTiles.left != null)
        {
            TowerObject towerObject = tile.adjTiles.left.tower;
            if (possibleTowers.Contains(towerObject))
                possibleTowers.Remove(towerObject);
        }
        if (tile.adjTiles.down != null)
        {
            TowerObject towerObject = tile.adjTiles.down.tower;
            if (possibleTowers.Contains(towerObject))
                possibleTowers.Remove(towerObject);
        }

        if (tile.adjTiles.right != null)
        {
            TowerObject towerObject = tile.adjTiles.right.tower;
            if (possibleTowers.Contains(towerObject))
                possibleTowers.Remove(towerObject);
        }

        if (possibleTowers.Count == 0)
            return spawnTowers[Random.Range(0, spawnTowers.Count)];
        return possibleTowers[Random.Range(0, possibleTowers.Count)];
    }
}



public struct BoardPosition
{
    public int x;
    public int y;

    public BoardPosition(int _x, int _y)
    {
        x = _x;
        y = _y;
    }
}
