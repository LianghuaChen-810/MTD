using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeMatch
{
    HashSet<TowerTile> tilesInShape = new HashSet<TowerTile>();

    TowerObject towerType = null;
    TowerTile tileToSpawnTower = null;
    public bool matchFound = false;

    public ShapeMatch (TowerTile startTile, bool wasTileSelected = false)
    {
        // Set tower type of shape
        towerType = startTile.tower;


        // If the player selected the tile the new tower should be at the selectedTile
        if (wasTileSelected)
        {
            tileToSpawnTower = startTile;
        }

        // Initialise lists for BFS search
        HashSet<TowerTile> tilesInMatches = new HashSet<TowerTile>();
        HashSet<TowerTile> alreadyVisitedTiles = new HashSet<TowerTile>();
        Queue<TowerTile> nextTilesToVisit = new Queue<TowerTile>();

        // Add starting tile to the queue
        nextTilesToVisit.Enqueue(startTile);

        // Go through the queue of tiles to visit
        while (nextTilesToVisit.Count != 0)
        {
            TowerTile currentTile = nextTilesToVisit.Dequeue();
            alreadyVisitedTiles.Add(currentTile);

            // Check UP and DOWN for a match-3
            List<TowerTile> matchingTilesDir = new List<TowerTile>();
            matchingTilesDir.Add(currentTile);

            // Find all same towers in UP
            RaycastHit2D hit = Physics2D.Raycast(currentTile.transform.position, Vector2.up);
            while (hit.collider != null && hit.collider.GetComponent<TowerTile>().tower == towerType)
            {
                matchingTilesDir.Add(hit.collider.gameObject.GetComponent<TowerTile>());
                hit = Physics2D.Raycast(hit.collider.transform.position, Vector2.up);
            }

            // Find all same towers in DOWN
            hit = Physics2D.Raycast(currentTile.transform.position, Vector2.down);
            while (hit.collider != null && hit.collider.GetComponent<TowerTile>().tower == towerType)
            {
                matchingTilesDir.Add(hit.collider.gameObject.GetComponent<TowerTile>());
                hit = Physics2D.Raycast(hit.collider.transform.position, Vector2.down);
            }

            // Check if there is a match of three or more
            if (matchingTilesDir.Count >= 3)
            {
                matchFound = true;

                // Add all of these tiles who were not visited before to
                // the list of matches and list of next to visit
                foreach (TowerTile matchedTile in matchingTilesDir)
                {
                    tilesInMatches.Add(matchedTile);
                    if (!alreadyVisitedTiles.Contains(matchedTile))
                        nextTilesToVisit.Enqueue(matchedTile);
                }
            }

            // Check LEFT and RIGHT for a match-3
            matchingTilesDir = new List<TowerTile>();
            matchingTilesDir.Add(currentTile);


            // Find all same towers in LEFT
            hit = Physics2D.Raycast(currentTile.transform.position, Vector2.left);
            while (hit.collider != null && hit.collider.GetComponent<TowerTile>().tower == towerType)
            {
                matchingTilesDir.Add(hit.collider.gameObject.GetComponent<TowerTile>());
                hit = Physics2D.Raycast(hit.collider.transform.position, Vector2.left);
            }

            // Find all same towers in RIGHT
            hit = Physics2D.Raycast(currentTile.transform.position, Vector2.right);
            while (hit.collider != null && hit.collider.GetComponent<TowerTile>().tower == towerType)
            {
                matchingTilesDir.Add(hit.collider.gameObject.GetComponent<TowerTile>());
                hit = Physics2D.Raycast(hit.collider.transform.position, Vector2.right);
            }

            // Check if there is a match of three or more
            if (matchingTilesDir.Count >= 3)
            {
                matchFound = true;

                // Add all of these tiles who were not visited before to
                // the list of matches and list of next to visit
                foreach (TowerTile matchedTile in matchingTilesDir)
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
            List<TowerTile> possibleTilesForTower = new List<TowerTile>();
            int miny = 20000;

            // Find the tiles that are closer to bottom
            foreach (TowerTile tile in tilesInShape)
            {
                if (tile.boardPosition.y < miny)
                {
                    miny = tile.boardPosition.y;
                    possibleTilesForTower = new List<TowerTile>();
                    possibleTilesForTower.Add(tile);
                }
                else if (tile.boardPosition.y == miny)
                {
                    possibleTilesForTower.Add(tile);
                }
            }

            // From those tiles find the one that is most to the left
            int minx = 20000;
            foreach (TowerTile tile in possibleTilesForTower)
            {
                if (tile.boardPosition.x < minx)
                {
                    minx = tile.boardPosition.x;
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
        foreach (TowerTile tile in tilesInShape)
        {
            Debug.Log(tile.name);
        }
    }

    public void UpdateTowerFromMatch()
    {
        // Go through towers in shape and add bonus attacks and new bonus
        int newBonusDamage = tilesInShape.Count - 3;
        foreach (TowerTile tile in tilesInShape)
        {
            newBonusDamage += tile.towerBonusDamage;
        }
        // generate new tower and set bonus damage
        tileToSpawnTower.SetTower(tileToSpawnTower.tower.nextLevelTower, newBonusDamage);
        // remove new tower from towers in shape
        tilesInShape.Remove(tileToSpawnTower);
        // remove other towers
        ClearAllOtherTiles();
    }

    public void ClearAllOtherTiles()
    {
        foreach (TowerTile tile in tilesInShape)
        {
            tile.SetTower(null);
        }
    }

}
