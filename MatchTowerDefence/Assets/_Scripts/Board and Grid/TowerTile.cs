using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Holds the information and functionality of a tile with a tower.
/// </summary>
public class TowerTile : MonoBehaviour
{

    // All tiles have access to same previous selected tile
    private static TowerTile previousSelected = null;
    private bool isSelected = false;


    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private float shootDelayTime = 2.0f;
    [SerializeField]
    private Color bulletSelectedColor = new Color(.5f, .5f, .5f, 1.0f);



    // Tower info
    public TowerObject tower = null;
    private SpriteRenderer render;
    [HideInInspector]
    public int towerBonusDamage = 0;

    // Board info
    [HideInInspector]
    public BoardPosition boardPosition = new BoardPosition(0, 0);


    // Adjacency info
    public struct AdjacentTiles
    {
        public TowerTile up;
        public TowerTile down;
        public TowerTile left;
        public TowerTile right;
    }
    public AdjacentTiles adjTiles;

    // Coroutine info
    private IEnumerator shootCourotine;

    void Awake()
    {
        shootCourotine = null;
        render = GetComponent<SpriteRenderer>();
    }


    // TODO: Move  START and STOP to the LEVEL CONTROL for optimisation.
    /// <summary>
    /// Initialises the shooting functionality of towers
    /// with active tower that has range higher than 0
    /// </summary>
    public void StartShooting()
    {
        if (shootCourotine == null)
        {
            shootCourotine = RepeatingShoot();
        }
        if (tower != null && tower.range > 0)
        {
            StartCoroutine(shootCourotine);
        }
    }

    /// <summary>
    /// Stops the shooting functionality of towers
    /// with active tower that has range higher than 0
    /// </summary>
    public void StopShooting()
    {
        if (tower != null && tower.range > 0)
        {
            StopCoroutine(shootCourotine);
        }
    }

    /// <summary>
    /// Coroutine that tries to find an enemy then shoots bullets at it
    /// every shoot delay time.
    /// </summary>
    IEnumerator RepeatingShoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(shootDelayTime);
            Enemy enemy = FindEnemyInReach();
            if (enemy != null)
            {
                Shoot(enemy);
            }
        }
    }

    /// <summary>
    /// Generates a bullet and sends it towards the enemy target
    /// </summary>
    /// <param name="enemy"> Enemy target to shoot at.</param>
    void Shoot(Enemy enemy)
    {
        //Debug.Log(tower.name + " is shooting at " + enemyToshoot.gameObject.name);
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().Shoot(tower, towerBonusDamage, enemy);

    }

    /// <summary>
    /// Function that searches for a possible enemy target to shoot at.
    /// </summary>
    /// <returns> The enemy object to shoot at.</returns>
    public Enemy FindEnemyInReach()
    {

        float closest = 100.0f;
        Enemy closestEnemy = null;
        foreach (Enemy enemy in LevelControl.enemiesInWave)
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
            //Debug.Log(tower.name + "  of  " + this.name + "Did not find a close enemy");
            return null;
        }

        return closestEnemy;

        // instantiate bullet with target closest enemy in range
    }

    /// <summary>
    /// Sets the new tower for the tile from a tower object and initial bonus damage.
    /// </summary>
    /// <param name="towerObject">The new tower scriptable object for the tile</param>
    /// <param name="bonusDamage">The new bonus damage for the tower tile</param>
    public void SetTower(TowerObject towerObject, int bonusDamage = 0)
    {
        if (towerObject == null)
        {
            tower = null;
            towerBonusDamage = 0;
            render.sprite = null;
        }
        else
        {
            tower = towerObject;
            towerBonusDamage = bonusDamage;
            render.sprite = towerObject.sprite;

        }
    }


    /// <summary>
    /// Selects the tile.
    /// </summary>
    private void Select()
    {
        isSelected = true;
        render.color = bulletSelectedColor;
        previousSelected = gameObject.GetComponent<TowerTile>();
    }

    /// <summary>
    /// Deselects the tile.
    /// </summary>
    private void Deselect()
    {
        isSelected = false;
        render.color = Color.white;
        previousSelected = null;
    }

    /// <summary>
    /// Depending on the previously selected tile it can select new, 
    /// deselect old or swap two towers between both selected tiles.
    /// </summary>
    void OnMouseDown()
    {
        if (LevelControl.phase != LevelPhase.PREPARATION) return;

        // Prevents from selecting tiles if the tutorial is on!
        if (TutorialManager.instance != null)
            if (TutorialManager.instance.isActive) return;

        // Empty towers are not affected by clicks
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
                bool success = SwapTower(previousSelected);
                if (success)
                {
                    previousSelected.FindMatch();
                    FindMatch();

                    // SHOULD MOVE THIS TO GENERAL LEVEL DATA
                    GUIManager.instance.MoveCounter--;

                    previousSelected.Deselect();
                } else
                {
                    previousSelected.Deselect();
                    Select();
                }
            }
        }
    }


    /// <summary>
    /// Swaps towers between this tile and otherTile
    /// </summary>
    /// <param name="otherTile"></param>
    public bool SwapTower(TowerTile otherTile)
    {
        if (tower == otherTile.tower)
        {
            return false; // This constraints the shift of two towers that are from same type
        }

        TowerObject tempTowerObj = tower;
        int tempBonusDamage = towerBonusDamage;

        SetTower(otherTile.tower, otherTile.towerBonusDamage);
        otherTile.SetTower(tempTowerObj, tempBonusDamage);
        return true;
    }


    // Finds a match of towers and creates a new one
    /// <summary>
    /// Checks to find a matching shape connected to the tile.
    /// </summary>
    public void FindMatch()
    {
        if (tower == null || !tower.hasUpgrade) // prevent from matching last level towers
            return;

        ShapeMatch shape = new ShapeMatch(this, previousSelected != null);
        if (shape.matchFound)
        {
            //shape.PrintShape();
            shape.UpdateTowerFromMatch();
            SFXManager.instance.PlaySFX(SFXManager.AudioClip.Match);
            StopCoroutine(BoardManager.instance.FindNullTiles());
            StartCoroutine(BoardManager.instance.FindNullTiles());
        }
    }

}