using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTile : MonoBehaviour
{
    public SpriteRenderer render = null;
    public List<PathTile> previousTiles = new List<PathTile>();
    public PathTile nextTile = null;
    public PathTileOrientation orientation = PathTileOrientation.NONE;
    public int distFromBase = -1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        render = GetComponent<SpriteRenderer>();
    }

}

public enum PathTileOrientation { UP, DOWN, LEFT, RIGHT, NONE };