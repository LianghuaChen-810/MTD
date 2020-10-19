using UnityEngine;

[CreateAssetMenu(menuName = "Assets/TowerObject")]
public class TowerObject : ScriptableObject
{
    public enum TowerType { NORMAL0, NORMAL1, NORMAL2, NORMAL3, AOE0, AOE1, AOE2, AOE3, FROST0, FROST1, FROST2, FROST3};
    public enum TowerAttackType { NORMAL, AOE, FROST };

    [Header("Tower Info")]
    public new string name;
    [TextArea(3, 10)]
    public string description;
    public string levelStr;

    [Header ("Tower Attributes")]
    public TowerType type;
    public TowerAttackType attackType;
    public float baseDamage;
    public float range;
    public Sprite sprite;
    public GameObject bulletPrefab;
 

    [Header ("Tower Upgrades")]
    public bool hasUpgrade;
    public TowerObject nextLevelTower;
}
