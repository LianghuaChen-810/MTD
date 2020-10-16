﻿using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEditor;
using LevelEditor;

[CreateAssetMenu(fileName = "LevelData", menuName = "LevelEditor/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    [HideInInspector]
    /// <summary>
    /// initial set up of one level
    /// </summary>
    public int[] board;

    /// <summary>
    /// route data
    /// </summary>
    public RouteData[] routes;

    /// <summary>
    /// name of the level
    /// </summary>
    public string levelName;

    /// <summary>
    /// order of the level
    /// </summary>
    public int levelNo;

    /// <summary>
    /// if this level inherit last level board, put that level here
    /// </summary>
    //public LevelData lastlevel;

    /// <summary>
    /// health point for all waves
    /// </summary>
    public int basehp;

    /// <summary>
    /// threshold of star1 and star2
    /// </summary>
    public int conditionthreshold;

    /// <summary>
    /// MonsterData for one wave
    /// </summary>
    public MonsterData firstwave;

    /// <summary>
    /// MonsterData for latter waves
    /// </summary>
    public MonsterData[] latterwaves;


    
}
