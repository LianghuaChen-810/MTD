using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/TowerObject")]
public class TowerObject : ScriptableObject
{

    public enum TowerType { NORMAL0, NORMAL1, NORMAL2, NORMAL3, AOE0, AOE1, AOE2, AOE3, FROST0, FROST1, FROST2, FROST3};
    public enum TowerAttackType { NORMAL, AOE, FROST };

    [Header ("Tower Attributes")]
    public TowerType type;
    public TowerAttackType attackType;
    public float baseDamage;
    public float range;
    public Sprite sprite;

    [Header("Tower Effect")]
    public float freezeTime;
    public Color color;

    [Header ("Tower Upgrades")]
    public bool hasUpgrade;
    public TowerObject nextLevelTower;


}
