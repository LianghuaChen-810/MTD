using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour
{

    public Color selectedColor = new Color(.5f, .5f, .5f, 1.0f);

    private static Tile previousSelected = null; // All Tiles have access to same previous selected tile

    private SpriteRenderer render;
    private bool isSelected = false;

    public TowerObject tower = null;
    public Vector2 boardPos = Vector2.zero;

    void Awake()
    {
        render = GetComponent<SpriteRenderer>();
    }

    public void SetTower(TowerObject towerObject)
    {
        if (towerObject == null)
        {
            tower = null;
            render.sprite = null;
            return;
        }
        tower = towerObject;
        render.sprite = towerObject.sprite;
    }

    private void Select()
    {
        isSelected = true;
        render.color = selectedColor;
        previousSelected = gameObject.GetComponent<Tile>();
    }

    private void Deselect()
    {
        isSelected = false;
        render.color = Color.white;
        previousSelected = null;
    }

    void OnMouseDown()
    {
        if (GUIManager.instance.MoveCounter == 0) return;

        if (tower == null || BoardManager.instance.IsShifting)
        {
            return;
        }

        if (isSelected)
        { 
            Deselect();
        } 
        else
        {
            if (previousSelected == null)
            { 
                Select();
            }
            else
            {
                SwapTower(previousSelected);

                previousSelected.FindMatch();
                FindMatch();

                //GUIManager.instance.MoveCounter--;
                //if (GUIManager.instance.MoveCounter == 0)
                //{
                //    BoardManager.instance.TriggerNextPhase();
                //}

                previousSelected.Deselect();
            }
        }
    }

    // Swaps towers between this tile and otherTile
    public void SwapTower(Tile otherTile)
    {
        if (tower == otherTile.tower)
        {
            return;
        }

        TowerObject tempTowerObj = tower;
        SetTower(otherTile.tower);
        otherTile.SetTower(tempTowerObj);
    }


    // Finds a match of towers and creates a new one
    public void FindMatch()
    {
        if (tower == null)
            return;

        ShapeMatch shape = new ShapeMatch(this, previousSelected != null);
        if (shape.matchFound)
        {
            //shape.PrintShape();
            shape.UpdateTowerFromMatch();

            StopCoroutine(BoardManager.instance.FindNullTiles());
            StartCoroutine(BoardManager.instance.FindNullTiles());
        }
    }

}