using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/TowerObject")]
public class TowerObject : ScriptableObject
{

    public enum TowerType { NORMAL0, NORMAL1, NORMAL2, NORMAL3, AOE0, AOE1, AOE2, AOE3, FROST0, FROST1, FROST2, FROST3};
    public enum TowerAttackType { NORMAL, AOE, FROST };

    public TowerType type;
    public TowerAttackType attackType;
    public TowerObject nextLevelTower;
    public Sprite sprite;


    public float freezeTime;
    public float damage;
    public Color color;
    public float range;


    // Update is called once per frame
    void OnUpdate()
    {
        switch (attackType)
        {
            case TowerAttackType.NORMAL: 
                AttackNormal();
                break;
            case TowerAttackType.AOE:
                AttackAOE();
                break;
            case TowerAttackType.FROST:
                AttackAOE();
                break;
            default:
                AttackNormal();
                break;
            
        }
    }


    public void AttackAOE()
    {

    }

    public void AttackNormal()
    {
        
    }

    public void AttackSlow()
    {

    }
}
