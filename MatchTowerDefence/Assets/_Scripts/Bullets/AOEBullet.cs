using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEBullet : Bullet
{
    public float aoeRange = 1.0f;

    public override void Effect(Enemy enemy, float dmg)
    {
        // raycast sphere
        Vector2 centre = new Vector2(transform.position.x, transform.position.y);
        Collider2D[] hits = Physics2D.OverlapCircleAll(centre, aoeRange);
        // find all enemies
        List<Enemy> enemiesToHit = new List<Enemy>();
        foreach (var hit in hits)
        {
            //Debug.Log(hit.gameObject);
            Enemy enemyHit = hit.GetComponent<Enemy>();
            if (enemyHit != null)
            {
                enemiesToHit.Add(enemyHit);
            }
        }

        // deal dmg to all enemies
        foreach (var enemyHit in enemiesToHit)
        {
            enemyHit.DamageEnemy(dmg);
        }

        //throw new System.NotImplementedException();
    }
}
