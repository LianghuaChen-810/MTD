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


    public List<Transform> patrolPoints = null;
    public int currentPatrolPoint = 0;

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
        if (patrolPoints == null) return;

        direction = (patrolPoints[currentPatrolPoint].position - this.transform.position).normalized;
        transform.Translate(speed * direction * Time.deltaTime);

        if (Vector3.Distance(transform.position, patrolPoints[currentPatrolPoint].position) <= 0.01f)
        {
            currentPatrolPoint++;
            if (currentPatrolPoint >= patrolPoints.Count)
            {
                // remove counter for win!
                BoardManager.instance.allEnemies.Remove(this);
                ++GUIManager.instance.EnemiesReached;
                Destroy(gameObject);
                
            }
        }
         
        if (health <= 0.0f)
        {
            BoardManager.instance.allEnemies.Remove(this);
            Destroy(gameObject);
        }
        // Script To Move
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
