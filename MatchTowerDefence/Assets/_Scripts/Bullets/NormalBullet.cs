using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : Bullet
{

    public override void Effect(Enemy enemy, float dmg)
    {
        if (enemy == null) return;

        enemy.DamageEnemy(dmg);
        //throw new System.NotImplementedException();
    }
}
