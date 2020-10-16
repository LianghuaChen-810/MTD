using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MatchTowerDefence.Managers;

public static class LevelControl
{
    public static PlayTransitions playTransition = PlayTransitions.NONE;

    public static int waves;
    public static int currentWave;

    public static int enemiesLeftInWave;
    public static List<Enemy> enemiesInWave;

    public static int enemiesPassed;

    public static LevelPhase phase;

    // Dictionary <WAVE NUMBER, LIST OF SPAWNERS>
    public static Dictionary<int, List<Spawner>> waveSpawners;
    public static List<Spawner> currentWaveSpawners;

    // Initialises the level info: enemies, waves, phase
    public static void Initialise(List<Spawner> spawnersInLevel)
    {
        phase = LevelPhase.PREPARATION;
        Debug.Log(spawnersInLevel.Count);
        waveSpawners = new Dictionary<int, List<Spawner>>();

        // Set wave-spawners dictionary
        for (int i = 0; i < spawnersInLevel.Count; i++)
        {
            var spawner = spawnersInLevel[i];

            // When first time seeing the wave number add a new empty list
            if (!waveSpawners.ContainsKey(spawner.wave))
            {
                waveSpawners.Add(spawner.wave, new List<Spawner>());
            }

            // Add the spawner to the list for its wave
            waveSpawners[spawner.wave].Add(spawner);
        }

        // Set the number of waves
        waves = waveSpawners.Keys.Count;
        currentWave = 0;
        enemiesLeftInWave = 0;
        enemiesPassed = 0;
        phase = LevelPhase.POSTDEFENCE;
    }


    /// <summary>
    /// Called by an enemy when it dies. Removes the enemy.
    /// </summary>
    /// <param name="enemy"> The enemy to remove from spawned list</param>
    public static void OnEnemyDied(Enemy enemy)
    {
        enemiesInWave.Remove(enemy);
        enemiesLeftInWave--;
    }

    /// <summary>
    /// Called by an enemy when it spawns. Adds new enemy for towers to shoot.
    /// </summary>
    /// <param name="enemy"> The enemy to add to the spawned list</param>
    public static void OnEnemySpawned(Enemy enemy)
    {
        enemiesInWave.Add(enemy);
    }

    /// <summary>
    /// Called by an enemy when it reaches the base. Removes enemy and does dmg to base.
    /// </summary>
    /// <param name="enemy"> The enemy to remove from the spawned list</param>
    public static void OnEnemyReachedBase(Enemy enemy)
    {
        enemiesInWave.Remove(enemy);
        enemiesLeftInWave--;
        enemiesPassed++;
    
        // Gamedata.Playerdata.Health -= enemy.attackDamage;
    }


    /// <summary>
    /// Update function to use in LevelManager. It controls which phase
    /// and wave of the current level it is.
    /// </summary>
    public static void OnUpdate()
    {
        if (playTransition != PlayTransitions.NONE) return;

        // TODO: SEPARE MOVE COUNTER WAY FROM GUI MANAGER
        if (phase == LevelPhase.PREPARATION && GUIManager.instance.MoveCounter == 0)
        {
            TriggerDefencePhase();
        }

        if (phase == LevelPhase.DEFENCE && enemiesLeftInWave == 0)
        {
            TriggerPostDefencePhase();
        }

        if (phase == LevelPhase.POSTDEFENCE)
        {
            if (currentWave == waves)
            {
                TriggerFinishedPhase();
            }
            else
            {
                TriggerPreparationPhase();
            }
        }
    }


    /// <summary>
    /// Starts the last phase (unknown to player) where it 
    /// notifies that the level has finished
    /// </summary>
    private static void TriggerFinishedPhase()
    {
        Debug.Log("LevelControl: Triggered Finish Phase");
        phase = LevelPhase.FINISHED;
    }

    /// <summary>
    /// Starts the post-defence phase (unknown to player), which
    /// helps reset other stuff and it is the initial phase when
    /// new level appears. (start of OnUpdate control loop)
    /// </summary>
    private static void TriggerPostDefencePhase()
    {
        Debug.Log("LevelControl: Triggered Post Defence Phase");

        // Stop towers from attacking
        foreach (TowerTile tile in BoardManager.instance.allTowerTiles)
        {
            tile.StopShooting();
        }

        GUIManager.instance.MoveCounter = 5; // TODO: REMOVE HARD CODED PART
        // Set phase
        phase = LevelPhase.POSTDEFENCE;
    }

    /// <summary>
    /// Set up the data for the next wave and begin preparation phase
    /// by allowing the player to move.
    /// </summary>
    private static void TriggerPreparationPhase()
    {
        Debug.Log("LevelControl: Triggered Preparation Phase");

        currentWave++;
        // Make spawners spawn depending on wave
        enemiesLeftInWave = 0;
        currentWaveSpawners = waveSpawners[currentWave];

        foreach (Spawner spawner in currentWaveSpawners)
        {
            enemiesLeftInWave += spawner.allEnemies.Count;
        }

        enemiesInWave = new List<Enemy>();

        // Set phase
        phase = LevelPhase.PREPARATION;
        GUIManager.instance.phaseTxt.text = "Preparation";
    }

    /// <summary>
    /// Triggers the defence phase, makes towers shoot and spawners spawn.
    /// </summary>
    private static void TriggerDefencePhase()
    {
        Debug.Log("LevelControl: Triggered Denfence Phase");

        // Set Phase
        phase = LevelPhase.DEFENCE;
        GUIManager.instance.phaseTxt.text = "Defence";

        // Make towers Shoot
        foreach (TowerTile tile in BoardManager.instance.allTowerTiles)
        {
            tile.StartShooting();
        }

        // Make spawners spawn
        foreach (Spawner spawner in currentWaveSpawners)
        {
            spawner.StartSpawning();
        }

    }
}

/// <summary>
/// Enumerator for the four possible phases of each level: preparation, defence, post-defence, finished.
/// </summary>
public enum LevelPhase { PREPARATION, DEFENCE, POSTDEFENCE, FINISHED }

/// <summary>
/// Enumerator for the four possible states of the board play to prevent any other actions: none, towers swapping, match found, board shifting.
/// </summary>
public enum PlayTransitions 
{ 
    NONE = 0, 
    TOWERSWAP = 1, 
    MATCHFOUND = 2, 
    SWAP_AND_MATCH = 3,
    BOARDSHIFTING = 4,
    SWAP_AND_SHIFT = 5,
    MATCH_AND_SHIFT = 6,
    ALL = 7 
}
