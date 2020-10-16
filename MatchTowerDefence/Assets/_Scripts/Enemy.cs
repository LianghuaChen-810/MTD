using MatchTowerDefence.Managers;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public List<TowerTile> towersThatAreInRange = new List<TowerTile>();
    public EnemyObject enemyType;
    public SpriteRenderer render;
    public Rigidbody2D rb;

    public float health;
    public float speed;

    public float unfreezeTime = 0.0f;

    public PathTile tileToMoveTo = null;

    private Vector3 direction;

    // Update is called once per frame
    private void Awake()
    {
        render = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Sets the enemies type and attributes to enemyType
    /// </summary>
    /// <param name="_enemyType"></param>
    public void SetEnemy (EnemyObject _enemyType)
    {
        enemyType = _enemyType;
        render.sprite = enemyType.sprite;
        health = enemyType.health;
        speed = enemyType.speed;
        LevelControl.OnEnemySpawned(this);
    }

    private void FixedUpdate()
    {
        if (unfreezeTime > 0.0f)
        {
            unfreezeTime -= 1.0f;
            if (unfreezeTime <= 0.0f)
                UnFreeze();
        }
    }

    /// <summary>
    /// Adds towers that can shoot at this enemy.
    /// Created list is used when enemy dies.
    /// </summary>
    /// <param name="collider"></param>
    void OnTriggerEnter2D(Collider2D collider)
    {
        //Debug.Log("ON TRIGGER ENTER 2D from tower");
        if (collider.gameObject.GetComponent<TowerTile>() != null)
        {
            TowerTile tower = collider.gameObject.GetComponent<TowerTile>();
            if (!towersThatAreInRange.Contains(tower))
            {
                towersThatAreInRange.Add(tower);
            }
        }
    }

    /// <summary>
    /// Removes towers that cannot shoot at this enemy.
    /// Created list is used when enemy dies.
    /// </summary>
    /// <param name="collider"></param>
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<TowerTile>() != null)
        {
            TowerTile tower = collider.gameObject.GetComponent<TowerTile>();
            if (towersThatAreInRange.Contains(tower))
            {
                towersThatAreInRange.Remove(tower);
            }
        }
    }


    private void Update()
    {
        Move();

        CheckIfDies();

        // Script To Move
    }

    /// <summary>
    /// Checks if enemy dies. If so removes it from tower's attack
    /// lists and notifies the Level Control
    /// </summary>
    public void CheckIfDies()
    {
        if (health <= 0.0f)
        {
            foreach (TowerTile tower in towersThatAreInRange)
            {
                tower.enemiesInRange.Remove(this);
            }

            LevelControl.OnEnemyDied(this);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Moves the enemy some distance forward its pathway
    /// </summary>
    public void Move()
    {
        if (tileToMoveTo == null) return;

        direction = (tileToMoveTo.transform.position - this.transform.position).normalized;
        transform.Translate(speed * direction * Time.deltaTime);

        if (Vector3.Distance(transform.position, tileToMoveTo.transform.position) <= 0.01f)
        {
            if (tileToMoveTo.nextTile == null)
            {
                if (!BoardManager.instance.allBases.Contains(tileToMoveTo))
                {
                    Debug.LogError(" Enemy: " + gameObject.name + " has reached an end of pathway but it was not a base");
                }
                else
                {
                    LevelControl.OnEnemyReachedBase(this);
                    ++GUIManager.instance.EnemiesReached;
                    Destroy(gameObject);
                }
            }
            else
            {
                tileToMoveTo = tileToMoveTo.nextTile;
            }

        }
    }

    /// <summary>
    /// Add the freeze effect onto the enemy for an freezeTime time.
    /// </summary>
    /// <param name="freezeTime"></param>
    public void Freeze(float freezeTime)
    {
        if (freezeTime == 0.0f) return;
        render.color = Color.blue;
        speed = enemyType.speed * 0.75f;
        unfreezeTime = freezeTime;
    }

    /// <summary>
    /// Removes the freeze effect from the enemy
    /// </summary>
    public void UnFreeze()
    {
        render.color = Color.white;
        speed = enemyType.speed;
    }
}
