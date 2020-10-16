using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "LevelEditor/MonsterData", order = 1)]
public class MonsterData : ScriptableObject
{
    /// <summary>
    /// quantity of enemys in this data list
    /// </summary>
    public int listnum;

    /// <summary>
    /// the type of each enemy(need specific resource)
    /// </summary>
    public int[] monstertypes;

    /// <summary>
    /// the number of the routes that enemys are going to go
    /// </summary>
    public int[] monsterroutes;

    /// <summary>
    /// spawn time of each enemy
    /// </summary>
    public float[] monstertimes;


}
