using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerTile : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float shootDelayTime = 2.0f;
    public Color selectedColor = new Color(.5f, .5f, .5f, 1.0f);

    private static TowerTile previousSelected = null; // All Tiles have access to same previous selected tile
    private bool isSelected = false;

    private SpriteRenderer render;
    public TowerObject tower = null;
    public BoardPosition boardPosition = new BoardPosition(0, 0);
    public int towerBonusDamage = 0;

    void Awake()
    {
        render = GetComponent<SpriteRenderer>();
    }

    public void StartShooting()
    {
        if (tower != null && tower.range > 0)
        {
            StartCoroutine(this.RepeatingShoot());
        }
    }

    IEnumerator RepeatingShoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(shootDelayTime);
            Shoot();
        }
    }

    void Shoot()
    {
        Enemy enemyToshoot = FindToShoot();

        if (enemyToshoot != null)
        {
            Debug.Log(tower.name + " is shooting at " + enemyToshoot.gameObject.name);
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.GetComponent<Bullet>().Shoot(tower, towerBonusDamage, enemyToshoot);
        } else
        {
            Debug.Log("No enemy to shoot");
        }

        Debug.Log(Time.time);
    }


    public Enemy FindToShoot()
    {

        float closest = 100.0f;
        Enemy closestEnemy = null;
        foreach (Enemy enemy in BoardManager.instance.allEnemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < closest)
            {
                closest = dist;
                closestEnemy = enemy;
            }
        }

        if (closest > tower.range * 3.0f)
        {
            Debug.Log(tower.name + "  of  " + this.name + "Did not find a close enemy");
            return null;
        }

        return closestEnemy;

        // instantiate bullet with target closest enemy in range
    }


    public void SetTower(TowerObject towerObject, int bonusDamage = 0)
    {
        if (towerObject == null)
        {
            tower = null;
            towerBonusDamage = 0;
            render.sprite = null;
            return;
        }
        tower = towerObject;
        towerBonusDamage = bonusDamage;
        render.sprite = towerObject.sprite;
    }

    private void Select()
    {
        isSelected = true;
        render.color = selectedColor;
        previousSelected = gameObject.GetComponent<TowerTile>();
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

                GUIManager.instance.MoveCounter--;
                if (GUIManager.instance.MoveCounter == 0)
                {
                    BoardManager.instance.TriggerNextPhase();
                }

                previousSelected.Deselect();
            }
        }
    }

    // Swaps towers between this tile and otherTile
    public void SwapTower(TowerTile otherTile)
    {
        if (tower == otherTile.tower)
        {
            return; // This constraints the shift of two towers that are from same type
        }

        TowerObject tempTowerObj = tower;
        int tempBonusDamage = towerBonusDamage;

        SetTower(otherTile.tower, otherTile.towerBonusDamage);
        otherTile.SetTower(tempTowerObj, tempBonusDamage);
    }


    // Finds a match of towers and creates a new one
    public void FindMatch()
    {
        if (tower == null || !tower.hasUpgrade) // prevent from matching last level towers
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