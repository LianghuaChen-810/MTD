using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RouteData", menuName = "LevelEditor/RouteData", order = 1)]

public class RouteData : ScriptableObject
{
    /// <summary>
    /// all the positions of corners and destination
    /// </summary>
    public Vector3[] points;

   // [HideInInspector]
    /// <summary>
    /// monster type
    /// </summary>
    //public int[] monstertypes;

   // [HideInInspector]
    /// <summary>
    /// moster spawn time
    /// </summary>
    //public float[] monstertimes;
}
