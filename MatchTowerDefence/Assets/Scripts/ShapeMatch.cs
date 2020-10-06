using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeMatch
{
    HashSet<Tile> tilesInShape = new HashSet<Tile>();

    TowerObject towerType = null;
    Tile tileToSpawnTower = null;
    public bool matchFound = false;

    public ShapeMatch (Tile startTile, bool wasTileSelected = false)
    {
        // Set tower type of shape
        towerType = startTile.tower;

        // If the player selected the tile the new tower should be at the selectedTile
        if (wasTileSelected)
        {
            tileToSpawnTower = startTile;
        }

        // Initialise lists for BFS search
        HashSet<Tile> tilesInMatches = new HashSet<Tile>();
        HashSet<Tile> alreadyVisitedTiles = new HashSet<Tile>();
        Queue<Tile> nextTilesToVisit = new Queue<Tile>();

        // Add starting tile to the queue
        nextTilesToVisit.Enqueue(startTile);

        // Go through the queue of tiles to visit
        while (nextTilesToVisit.Count != 0)
        {
            Tile currentTile = nextTilesToVisit.Dequeue();
            alreadyVisitedTiles.Add(currentTile);

            // Check UP and DOWN for a match-3
            List<Tile> matchingTilesDir = new List<Tile>();
            matchingTilesDir.Add(currentTile);

            // Find all same towers in UP
            RaycastHit2D hit = Physics2D.Raycast(currentTile.transform.position, Vector2.up);
            while (hit.collider != null && hit.collider.GetComponent<Tile>().tower == towerType)
            {
                matchingTilesDir.Add(hit.collider.gameObject.GetComponent<Tile>());
                hit = Physics2D.Raycast(hit.collider.transform.position, Vector2.up);
            }

            // Find all same towers in DOWN
            hit = Physics2D.Raycast(currentTile.transform.position, Vector2.down);
            while (hit.collider != null && hit.collider.GetComponent<Tile>().tower == towerType)
            {
                matchingTilesDir.Add(hit.collider.gameObject.GetComponent<Tile>());
                hit = Physics2D.Raycast(hit.collider.transform.position, Vector2.down);
            }

            // Check if there is a match of three or more
            if (matchingTilesDir.Count >= 3)
            {
                matchFound = true;

                // Add all of these tiles who were not visited before to
                // the list of matches and list of next to visit
                foreach (Tile matchedTile in matchingTilesDir)
                {
                    tilesInMatches.Add(matchedTile);
                    if (!alreadyVisitedTiles.Contains(matchedTile))
                        nextTilesToVisit.Enqueue(matchedTile);
                }
            }

            // Check LEFT and RIGHT for a match-3
            matchingTilesDir = new List<Tile>();
            matchingTilesDir.Add(currentTile);


            // Find all same towers in LEFT
            hit = Physics2D.Raycast(currentTile.transform.position, Vector2.left);
            while (hit.collider != null && hit.collider.GetComponent<Tile>().tower == towerType)
            {
                matchingTilesDir.Add(hit.collider.gameObject.GetComponent<Tile>());
                hit = Physics2D.Raycast(hit.collider.transform.position, Vector2.left);
            }

            // Find all same towers in RIGHT
            hit = Physics2D.Raycast(currentTile.transform.position, Vector2.right);
            while (hit.collider != null && hit.collider.GetComponent<Tile>().tower == towerType)
            {
                matchingTilesDir.Add(hit.collider.gameObject.GetComponent<Tile>());
                hit = Physics2D.Raycast(hit.collider.transform.position, Vector2.right);
            }

            // Check if there is a match of three or more
            if (matchingTilesDir.Count >= 3)
            {
                matchFound = true;

                // Add all of these tiles who were not visited before to
                // the list of matches and list of next to visit
                foreach (Tile matchedTile in matchingTilesDir)
                {
                    tilesInMatches.Add(matchedTile);
                    if (!alreadyVisitedTiles.Contains(matchedTile))
                        nextTilesToVisit.Enqueue(matchedTile);
                }
            }
        }

        tilesInShape = tilesInMatches;

        if (matchFound && tileToSpawnTower == null)
        {
            List<Tile> possibleTilesForTower = new List<Tile>();
            int miny = 20000;

            // Find the tiles that are closer to bottom
            foreach (Tile tile in tilesInShape)
            {
                if (tile.boardPos.y < miny)
                {
                    miny = (int)tile.boardPos.y;
                    possibleTilesForTower = new List<Tile>();
                    possibleTilesForTower.Add(tile);
                }
                else if ((int)tile.boardPos.y == miny)
                {
                    possibleTilesForTower.Add(tile);
                }
            }

            // From those tiles find the one that is most to the left
            int minx = 20000;
            foreach (Tile tile in possibleTilesForTower)
            {
                if (tile.boardPos.x < minx)
                {
                    minx = (int)tile.boardPos.x;
                    tileToSpawnTower = tile;
                }
            }
        }
    }

    public void PrintShape()
    {
        Debug.Log("------------------------------");
        Debug.Log("Print Shape: " + this);
        Debug.Log("Was selected: " + tileToSpawnTower);
        Debug.Log("Match found: " + matchFound);
        foreach (Tile tile in tilesInShape)
        {
            Debug.Log(tile.name);
        }
    }

    public void UpdateTowerFromMatch()
    {
        // Go through towers in shape and add bonus attacks and new bonus

        // generate new tower
        Debug.Log("Tower to spawn: " + tileToSpawnTower.name + " will transform into: " + tileToSpawnTower.tower.nextLevelTower);
        tileToSpawnTower.SetTower(tileToSpawnTower.tower.nextLevelTower);

        // set new bonus attack

        // remove new tower from towers in shape
        tilesInShape.Remove(tileToSpawnTower);

        // remove other towers
        ClearAllOtherTiles();
    }

    public void ClearAllOtherTiles()
    {
        foreach (Tile tile in tilesInShape)
        {
            tile.SetTower(null);
        }
    }

}
