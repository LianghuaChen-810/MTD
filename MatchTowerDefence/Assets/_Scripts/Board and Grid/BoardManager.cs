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

        // Link the list of all tower tiles on the board for exteral use
        allTowerTiles = new List<TowerTile>();
        allTowerTiles.AddRange(FindObjectsOfType<TowerTile>());
    }

    /// <summary>
    /// Generates the board from the tilemap.
    /// </summary>
    private void CreateTowerBoard()
    {
        int minRows = tilemap.cellBounds.yMin;
        int minCols = tilemap.cellBounds.xMin;

        xSize = tilemap.cellBounds.size.x;
        ySize = tilemap.cellBounds.size.y;

        int halfSizeX = xSize / 2;
        int halfSizeY = ySize / 2;
        float startX = transform.position.x - halfSizeX;
        float startY = transform.position.y - halfSizeY;

        // Columns of Tiles for shifting towers down
        towerTileColumns = new List<TowerTile>[tilemap.cellBounds.size.x];

        TowerObject[] previousLeft = new TowerObject[ySize];
        // Create TowerTiles
        for (int col = 0; col < xSize; col++)
        {
            towerTileColumns[col] = new List<TowerTile>();
            TowerObject previousBelow = null;

            for (int row = 0; row < ySize; row++)
            {
                Vector3Int tilePos = new Vector3Int(col + minCols, row + minRows, 0);

                if (tilemap.GetTile(tilePos) == null) continue;

                string tilename = tilemap.GetTile(tilePos).name;
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
                if (tilename != "PathDown" && tilename != "PathRight" && tilename != "PathUp" && tilename != "PathLeft")
                {
                    tilemap.SetTile(tilePos, null);
                }

            }

        }


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
