using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    TowerObject tower = null;
    Enemy enemy = null;
    bool isShot = false;
    public float damage = 0;
    SpriteRenderer render;

    Vector3 lastTargetPosition = Vector3.zero;

    void Awake()
    {
        render = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isShot)
        {
            if (enemy != null)
            {
                lastTargetPosition = enemy.transform.position;
            }
            Vector3 direction = (lastTargetPosition - this.transform.position).normalized;
            transform.Translate(1.0f * direction * Time.deltaTime);

            if (Vector3.Distance(transform.position, lastTargetPosition) <= 0.01f)
            {
                if (enemy != null)
                {
                    enemy.health -= damage;
                    enemy.Freeze(tower.freezeTime);
                }
                Destroy(gameObject);
            }
        }
    }

    public void Shoot (TowerObject to, float bonusDamage, Enemy en)
    {
       
        tower = to;
        damage = bonusDamage + to.baseDamage;
        render.color = to.color;
        enemy = en;
        isShot = true;
    }
}
