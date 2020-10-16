using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostBullet : Bullet
{

    public float freezeTime = 3.0f;

    public override void Effect(Enemy enemy, float dmg)
    {
        if (enemy == null) return;

        enemy.DamageEnemy(dmg);
        enemy.Freeze(freezeTime);
        //throw new System.NotImplementedException();
    }
}
