﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int wave = 1;
    [SerializeField] float spawnDelay = 1.0f;
    private WaitForSecondsRealtime _waitTime;

    public List<EnemyObject> allEnemies = new List<EnemyObject>();
    public PathTile spawnPathTile = null;

    public GameObject enemyPrefab;

    int enemyCount = 0;

    // Start is called before the first frame update
    private void Start()
    {
        enemyCount = allEnemies.Count;
    }

    public void StartSpawning()
    {
        _waitTime = new WaitForSecondsRealtime(spawnDelay);
        StartCoroutine(SpawnMonster());
    }

    // Update is called once per frame
    private void Update()
    {

    }

    IEnumerator SpawnMonster()
    {
        int enemyCount = 0;
        while (enemyCount < allEnemies.Count)
        {
            //var pointSelected = Random.Range(0, 3); //To use as index for reaching array of spawn points.
            //var pointToSpawn = SpawnPoints[pointSelected].position;

            var clonePrefab = Instantiate(enemyPrefab, spawnPathTile.transform.position, Quaternion.identity);
            Enemy enemy = clonePrefab.GetComponent<Enemy>();
            enemy.SetEnemy(allEnemies[enemyCount]);
            enemy.tileToMoveTo = spawnPathTile;
            enemyCount++;

            yield return _waitTime;
        }
    }
}
