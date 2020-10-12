using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEditor;
using LevelEditor;

[CreateAssetMenu(fileName = "LevelData", menuName = "LevelData", order = 1)]
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


}
