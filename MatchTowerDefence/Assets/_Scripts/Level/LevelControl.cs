using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelControl
{
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

        waveSpawners = new Dictionary<int, List<Spawner>>();

        // Set wave-spawners dictionary
        for (int i = 0; i < spawnersInLevel.Count; i++)
        {
            var spawner = spawnersInLevel[i];

            // When first time seeing the wave number add a new empty list
            if (waveSpawners[spawner.wave] == null)
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
        currentWave = 0;
        phase = LevelPhase.POSTDEFENCE;
    }


    public static void OnEnemyDied(Enemy enemy)
    {
        enemiesInWave.Remove(enemy);
        enemiesLeftInWave--;
    }

    public static void OnEnemySpawned(Enemy enemy)
    {
        enemiesInWave.Add(enemy);
    }

    public static void OnEnemyReachedGoal(Enemy enemy)
    {
        enemiesInWave.Remove(enemy);
        enemiesLeftInWave--;
        enemiesPassed++;
    }



    public static void OnUpdate()
    {
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

    public static void TriggerFinishedPhase()
    {
        phase = LevelPhase.FINISHED;
        GUIManager.instance.LevelIsFinished();
    }

    public static void TriggerPostDefencePhase()
    {
        // Stop towers from attacking
        foreach (TowerTile tile in BoardManager.instance.allTiles)
        {
            tile.StopShooting();
        }

        // Set phase
        phase = LevelPhase.POSTDEFENCE;
    }

    // Set up data for the next wave and begin preparation phase;
    public static void TriggerPreparationPhase()
    {
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


    // Trigger the defence phase and start spawning
    public static void TriggerDefencePhase()
    {

        // Set Phase
        phase = LevelPhase.DEFENCE;
        GUIManager.instance.phaseTxt.text = "Defence";

        // Make towers Shoot
        foreach (TowerTile tile in BoardManager.instance.allTiles)
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


public enum LevelPhase { PREPARATION, DEFENCE, POSTDEFENCE, FINISHED }
