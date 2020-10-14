using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

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

    private void Update()
    {
        Move();
        if (health <= 0.0f)
        {
            LevelControl.OnEnemyDied(this);
            Destroy(gameObject);
        }
        // Script To Move
    }

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
    public void Freeze(float freezeTime)
    {
        if (freezeTime == 0.0f) return;
        render.color = Color.blue;
        speed = enemyType.speed * 0.75f;
        unfreezeTime = freezeTime;
    }

    public void UnFreeze()
    {
        render.color = Color.white;
        speed = enemyType.speed;
    }
}
