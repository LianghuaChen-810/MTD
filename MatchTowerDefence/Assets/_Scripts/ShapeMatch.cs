using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that is used to generate matches after tiles are moved.
/// Uses the adjacency data in the TowerTile class.
/// </summary>
public class ShapeMatch
{
    private const int MAX_TILES_IN_DIRECTION = 20000;
    public bool matchFound = false;
    TowerObject towerType = null;
    TowerTile tileToSpawnTower = null;
    HashSet<TowerTile> tilesInShape = new HashSet<TowerTile>();


    /// <summary>
    /// Starts generation of a new shape. Uses the tower type of the start tower tile.
    /// </summary>
    /// <param name="startTile">The tile to start generation from</param>
    /// <param name="wasTileSelected">If false the shapematch will find its own tile to spawn tower</param>
    public ShapeMatch(TowerTile startTile, bool wasTileSelected = false)
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

            // Initialise a matching line in the UP and DOWN directions
            List<TowerTile> matchingTilesDir = new List<TowerTile>();
            matchingTilesDir.Add(currentTile);

            // Find all same towers in UP
            TowerTile adjacentTile = currentTile.adjTiles.up;
            while (adjacentTile != null && adjacentTile.tower == towerType)
            {
                matchingTilesDir.Add(adjacentTile);
                adjacentTile = adjacentTile.adjTiles.up;
            }

            // Find all same towers in DOWN
            adjacentTile = currentTile.adjTiles.down;
            while (adjacentTile != null && adjacentTile.tower == towerType)
            {
                matchingTilesDir.Add(adjacentTile);
                adjacentTile = adjacentTile.adjTiles.down;
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

            // Initialise a matching line in the LEFT and RIGHT directions
            matchingTilesDir = new List<TowerTile>();
            matchingTilesDir.Add(currentTile);


            // Find all same towers in LEFT
            adjacentTile = currentTile.adjTiles.left;
            while (adjacentTile != null && adjacentTile.tower == towerType)
            {
                matchingTilesDir.Add(adjacentTile);
                adjacentTile = adjacentTile.adjTiles.left;
            }

            // Find all same towers in RIGHT
            adjacentTile = currentTile.adjTiles.right;
            while (adjacentTile != null && adjacentTile.tower == towerType)
            {
                matchingTilesDir.Add(adjacentTile);
                adjacentTile = adjacentTile.adjTiles.right;
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

        // Add all tiles that are in any match connected to the start tile.
        tilesInShape = tilesInMatches;

        // If there was a match and there was no tiles selected ->
        // find a possible position where to spawn the tower by 
        // following the rules BOTTOM > LEFT > TOP > RIGHT
        if (matchFound && !wasTileSelected)
        {
            List<TowerTile> possibleTilesForTower = new List<TowerTile>();
            int miny = MAX_TILES_IN_DIRECTION;

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

            // From those tiles find the one that is mostly to the left
            int minx = MAX_TILES_IN_DIRECTION;
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

    /// <summary>
    /// Debugging method to print the shape info.
    /// </summary>
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

    /// <summary>
    /// Generates a new tower from the match shape
    /// </summary>
    public void UpdateTowerFromMatch()
    {
        // Go through towers in shape and add bonus attacks and new bonus
        int newBonusDamage = tilesInShape.Count - 3;
        foreach (TowerTile tile in tilesInShape)
        {
            newBonusDamage += tile.towerBonusDamage;
        }

        // generate new tower and set bonus damage
        TowerObject nextTower = tileToSpawnTower.tower.nextLevelTower;
        tileToSpawnTower.SetTower(nextTower, newBonusDamage);
        if (TutorialManager.instance != null)
        {
            TutorialManager.instance.TowerSpawnedEvent(nextTower);
        }

        // remove new tower from towers in shape
        tilesInShape.Remove(tileToSpawnTower);

        // remove other towers
        ClearAllOtherTiles();
    }

    /// <summary>
    /// Clears all tiles except the newly generated one.
    /// </summary>
    private void ClearAllOtherTiles()
    {
        foreach (TowerTile tile in tilesInShape)
        {
            tile.SetTower(null);
        }
    }
}