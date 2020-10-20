using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MatchTowerDefence.Managers;

/// <summary>
/// Holds the information and functionality of a tile with a tower.
/// </summary>
public class TowerTile : MonoBehaviour
{
    private const float UNSCALE = 2.0f;
    private const float TO_RADIUS = 2.0f;
    private const float FAKE_MULTIPLIER = 1.0f;
    private const float REDUCER = 0.0f;

    // All tiles have access to same previous selected tile
    private static TowerTile previousSelected = null;
    private bool isSelected = false;

    [SerializeField]
    private Color bulletSelectedColor = new Color(.5f, .5f, .5f, 1.0f);

    // Collider info
    [SerializeField]
    private BoxCollider2D collider2d = null;

    // Tower info
    public TowerObject tower = null;
    private SpriteRenderer render;
    public int towerBonusDamage = 0;

    // Board info
    public BoardPosition boardPosition = new BoardPosition(0, 0);

    public List<Enemy> enemiesInRange = null;
    public Enemy currentEnemyInRange = null;

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
        collider2d = GetComponent<BoxCollider2D>();
    }

    /// <summary>
    /// Adds enemies when they get in range
    /// </summary>
    /// <param name="collider"></param>
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collider.gameObject.GetComponent<Enemy>();
            if (!enemiesInRange.Contains(enemy))
            {
                enemiesInRange.Add(enemy);
            }
        }
    }

    /// <summary>
    /// Removes enemies when they get out of range
    /// </summary>
    /// <param name="collider"></param>
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collider.gameObject.GetComponent<Enemy>();
            if (enemiesInRange.Contains(enemy))
            {
                enemiesInRange.Remove(enemy);
            }
        }
    }

    // TODO: Move  START and STOP to the LEVEL CONTROL for optimisation.
    /// <summary>
    /// Initialises the shooting functionality of towers
    /// with active tower that has range higher than 0
    /// </summary>
    public void StartShooting()
    {

        if (tower == null || tower.range == 0) return;

        collider2d = gameObject.GetComponent<BoxCollider2D>();
        
        if (shootCourotine == null)
        {
            shootCourotine = RepeatingShoot();
        }
        enemiesInRange = new List<Enemy>();
        currentEnemyInRange = null;
        collider2d.size = new Vector2(tower.range * UNSCALE * TO_RADIUS * FAKE_MULTIPLIER - REDUCER, tower.range * UNSCALE * TO_RADIUS * FAKE_MULTIPLIER - REDUCER);

        StartCoroutine(shootCourotine);
    }

    /// <summary>
    /// Stops the shooting functionality of towers
    /// with active tower that has range higher than 0
    /// </summary>
    public void StopShooting()
    {
        if (tower == null || tower.range == 0) return;

        collider2d.size = new Vector2(1.95f, 1.95f);
        enemiesInRange = new List<Enemy>();
        currentEnemyInRange = null;

        StopCoroutine(shootCourotine);
    }

    /// <summary>
    /// Coroutine that tries to find an enemy then shoots bullets at it
    /// every shoot delay time.
    /// </summary>
    IEnumerator RepeatingShoot()
    {
        while (true)
        {
            // If no enemy search for enemy 
            if (currentEnemyInRange == null)
            {
                Enemy closestEnemy = null;
                int minDistFromBase = 10000;

                // Find closest enemy to base from enemies in range
                for (int i = 0; i < enemiesInRange.Count; i++)
                {
                    if (enemiesInRange[i].tileToMoveTo.distFromBase < minDistFromBase)
                    {
                        minDistFromBase = enemiesInRange[i].tileToMoveTo.distFromBase;
                        closestEnemy = enemiesInRange[i];
                    }
                }

                // If no enemy was found wait for little time and try to find again.
                if (closestEnemy == null)
                {
                    yield return new WaitForSeconds(0.3f); // NEED TO SCALE WITH GAME TIME SCALE
                    continue;
                }
                currentEnemyInRange = closestEnemy;
            }

            // if enemy went outside of range
            else if (!enemiesInRange.Contains(currentEnemyInRange))
            {
                currentEnemyInRange = null;
                continue;
            }
            else
            {
                Enemy closestEnemy = null;
                int minDistFromBase = 10000;

                // Find closest enemy to base from enemies in range
                for (int i = 0; i < enemiesInRange.Count; i++)
                {
                    if (enemiesInRange[i].tileToMoveTo.distFromBase < minDistFromBase)
                    {
                        minDistFromBase = enemiesInRange[i].tileToMoveTo.distFromBase;
                        closestEnemy = enemiesInRange[i];
                    }
                }
                currentEnemyInRange = closestEnemy;

                if (closestEnemy == null)
                {
                    continue;
                }
                Shoot(currentEnemyInRange);
                yield return new WaitForSeconds(tower.shootDelayTime); // NEED TO SCALE WITH GAME TIME SCALE
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
        GameObject bullet = Instantiate(tower.bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().ShootAt(enemy, towerBonusDamage + tower.baseDamage);

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
        //Only select if the game is not paused
        if (!GUIManager.instance.pauseMenu.activeSelf)
        {
            isSelected = true;
            render.color = bulletSelectedColor;
            previousSelected = gameObject.GetComponent<TowerTile>();
            GUIManager.instance.UpdateSelectedTower(tower, towerBonusDamage);
        }
    }

    /// <summary>
    /// Deselects the tile.
    /// </summary>
    private void Deselect()
    {
        isSelected = false;
        render.color = Color.white;
        previousSelected = null;
        GUIManager.instance.CloseSelectedTower();

    }

    /// <summary>
    /// Depending on the previously selected tile it can select new, 
    /// deselect old or swap two towers between both selected tiles.
    /// </summary>
    void OnMouseDown()
    {
        if (LevelControl.playTransition != PlayTransitions.NONE) return;
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
                LevelControl.playTransition ^= PlayTransitions.TOWERSWAP;
                //Debug.Log(gameObject.name + " On mouse down (XOR TowerSwap result): " + LevelControl.playTransition);

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

                LevelControl.playTransition ^= PlayTransitions.TOWERSWAP;
                //Debug.Log(gameObject.name + " On mouse down (XOR TowerSwap result): " + LevelControl.playTransition);
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
            LevelControl.playTransition |= PlayTransitions.MATCHFOUND;
            //Debug.Log(gameObject.name + " Find Match (Or MatchFound result): " + LevelControl.playTransition);
            //shape.PrintShape();
            shape.UpdateTowerFromMatch();
            StopCoroutine(BoardManager.instance.FindNullTiles());

            LevelControl.playTransition |= PlayTransitions.BOARDSHIFTING;
            //Debug.Log(gameObject.name + " Find Match (Or Boardshifting result): " + LevelControl.playTransition);

            LevelControl.playTransition ^= PlayTransitions.MATCHFOUND;
            //Debug.Log(gameObject.name + " Find Match (XOr MatchFound result): " + LevelControl.playTransition);

            StartCoroutine(BoardManager.instance.FindNullTiles());

            SFXManager.instance.PlaySFX(SFXManager.AudioClip.Match);
        }
    }

}