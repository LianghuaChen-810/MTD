using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{

    public CameraManager camManager = null;
    public TileBase testTilePrefab;
    public static BoardManager instance;
    public List<TowerObject> spawnTowers = new List<TowerObject>();
    public TowerObject AOETower;
    public TowerObject NormalTower;
    public TowerObject FrostTower;

    public List<Enemy> allEnemies = new List<Enemy>();

    public float shiftDelay = 0.03f;

    public GameObject towerTilePrefab;
    public GameObject pathTilePrefab;

    int xSize, ySize;

    //private GameObject[,] towerTileObjs;

    public bool IsShifting { get; set; }

    List<TowerTile>[] towerTileColumns;

    public Tilemap tilemap = null;
    void Start()
    {
        IsShifting = false;
        instance = GetComponent<BoardManager>();


        tilemap = FindObjectOfType<Tilemap>();
        if (tilemap != null)
        {
            tilemap.CompressBounds();
            xSize = tilemap.cellBounds.size.x;
            ySize = tilemap.cellBounds.size.y;

            camManager.SetDisplay((Mathf.Max(xSize, ySize) / 2.0f ), transform.position);
            //AnalyseTilemap();
            // CreateBoard();
            CreateTowerBoard();
            // CreatePathBoard();
        }
    }

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

                string tilename = tilemap.GetTile(tilePos).name;
                if (tilename == "MapTile" || tilename == "Frost0Tile" || tilename == "AOE0Tile" || tilename == "Normal0Tile")
                {


                }
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

                    Debug.Log(newTileObj.name + " = " + newTowerObject);
                    // Edit tile info
                    TowerTile newTile = newTileObj.GetComponent<TowerTile>();
                    newTile.boardPosition = new BoardPosition(col, row);
                    newTile.SetTower(newTowerObject);

                    // Add tile as previous object
                    previousLeft[row] = newTowerObject;
                    previousBelow = newTowerObject;

                }
  
                tilemap.SetTile(tilePos, null);

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
                    shiftTiles[k].SetTower(shiftTiles[k + 1].tower,shiftTiles[k+1].towerBonusDamage);
                    shiftTiles[k + 1].SetTower(null); // 6
                }
                shiftTiles[k].SetTower(GetNewTower(shiftTiles[k]));
            }
            shiftTiles.RemoveAt(0);
            yStart++;

        }
        IsShifting = false;

    }

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


    public void TriggerNextPhase()
    {
        var allTiles = FindObjectsOfType<TowerTile>();
        foreach (TowerTile tile in allTiles)
        {
            tile.StartShooting();
        }

        var allSpawners = FindObjectsOfType<Spawner>();
        foreach (Spawner spawner in allSpawners)
        {
            spawner.StartSpawning();
        }

        GUIManager.instance.phaseTxt.text = "Defense Phase";
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
