using MatchTowerDefence.Managers;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{

    Enemy enemy = null;
    bool isShot = false;
    float damage = 0;
    Vector3 lastTargetPosition = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        if (isShot)
        {
            if (enemy != null)
            {
                lastTargetPosition = enemy.transform.position;
            }

            // Move the bullets
            Vector3 direction = (lastTargetPosition - this.transform.position).normalized;
            transform.Translate(1.5f * direction * Time.deltaTime);

            // When reached enemy position
            if (Vector3.Distance(transform.position, lastTargetPosition) <= 0.01f)
            {

                Effect(enemy, damage);
                Destroy(gameObject);
            }
        }
    }


    public abstract void Effect(Enemy enemy, float dmg);

    public void ShootAt (Enemy en, float dmg)
    {
        damage = dmg;
        enemy = en;
        SFXManager.instance.PlaySFX(SFXManager.AudioClip.TowerAttack);
        isShot = true;
    }



    public static void FrostEffect(Enemy enemy, Bullet bullet)
    {

    }

    public static void NormalEffect(Enemy enemy, Bullet bullet)
    {

    }

    public static void AOEEffect (Enemy enemy, Bullet bullet)
    {

    }
}
