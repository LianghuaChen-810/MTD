using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour
{

    public static BoardManager instance;
    public List<TowerObject> spawnTowers = new List<TowerObject>();

    public List<Enemy> allEnemies = new List<Enemy>();

    public float shiftDelay = 0.03f;

    public GameObject tile;
    public int xSize, ySize;

    private GameObject[,] tiles;

    public bool IsShifting { get; set; }

    void Start()
    {
        IsShifting = false;
        instance = GetComponent<BoardManager>();

        Vector2 offset = tile.GetComponent<SpriteRenderer>().bounds.size;
        CreateBoard(offset.x, offset.y);
    }


    private void CreateBoard(float xOffset, float yOffset)
    {
        tiles = new GameObject[xSize, ySize];

        int halfSizeX = xSize / 2;
        int halfSizeY = ySize / 2;
        float startX = transform.position.x - halfSizeX;
        float startY = transform.position.y - halfSizeY;


        TowerObject[] previousLeft = new TowerObject[ySize];

        for (int x = 0; x < xSize; x++)
        {
            TowerObject previousBelow = null;

            for (int y = 0; y < ySize; y++)
            {

                // Create new tile object
                GameObject newTileObj = Instantiate(tile, new Vector3(startX + x , startY + y, 0), tile.transform.rotation);
                newTileObj.name = "Tile_" + x.ToString() + "_" + y.ToString();
                tiles[x, y] = newTileObj;
                newTileObj.transform.parent = transform; // 1

                // Create list of possible towers
                List<TowerObject> possibleTowers = new List<TowerObject>(); 
                possibleTowers.AddRange(spawnTowers); 

                // Remove tower types on the left and bottom from possible
                possibleTowers.Remove(previousLeft[y]); 
                possibleTowers.Remove(previousBelow);

                // Random generation of new tower
                TowerObject newTowerObject = possibleTowers[Random.Range(0, possibleTowers.Count)];

                // Edit tile info
                TowerTile newTile = newTileObj.GetComponent<TowerTile>();
                newTile.boardPosition = new BoardPosition(x, y);
                newTile.SetTower(newTowerObject);

                // Add tile as previous object
                previousLeft[y] = newTowerObject;
                previousBelow = newTowerObject;
            }
        }

    }


    public IEnumerator FindNullTiles()
    {
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                if (tiles[x, y].GetComponent<TowerTile>().tower == null)
                {
                    yield return StartCoroutine(ShiftTowersDown(x, y));
                    break;
                }
            }
        }

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                tiles[x, y].GetComponent<TowerTile>().FindMatch();
            }
        }
    }


    private IEnumerator ShiftTowersDown(int x, int yStart)
    {
        IsShifting = true;
        List<TowerTile> shiftTiles = new List<TowerTile>();

        for (int y = yStart; y < ySize; y++)
        {  // 1
            TowerTile tile = tiles[x, y].GetComponent<TowerTile>();
            shiftTiles.Add(tile);
        }

        while (yStart < ySize)
        {
         //   Debug.Log("For X,Ystart: " + x + " " + yStart);

            var tile = tiles[x, yStart].GetComponent<TowerTile>();
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
                shiftTiles[k].SetTower(GetNewTower(x, ySize - 1));
            }
            shiftTiles.RemoveAt(0);
            yStart++;

        }
        IsShifting = false;

    }

    private TowerObject GetNewTower(int x, int y)
    {
        List<TowerObject> possibleTowers = new List<TowerObject>();
        possibleTowers.AddRange(spawnTowers);

        if (x > 0)
        {
            TowerObject towerObj = tiles[x - 1, y].GetComponent<TowerTile>().tower;
            if (possibleTowers.Contains(towerObj))
            possibleTowers.Remove(towerObj);
        }
        if (x < xSize - 1)
        {
            TowerObject towerObj = tiles[x + 1, y].GetComponent<TowerTile>().tower;
            if (possibleTowers.Contains(towerObj))
                possibleTowers.Remove(towerObj);
        }
        if (y > 0)
        {
            TowerObject towerObj = tiles[x, y - 1].GetComponent<TowerTile>().tower;
            if (possibleTowers.Contains(towerObj))
                possibleTowers.Remove(towerObj);
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
