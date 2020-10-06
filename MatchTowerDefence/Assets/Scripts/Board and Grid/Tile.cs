using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour
{
    //[SerializeField]
    //float shootDelay = 1.0f;
    private WaitForSecondsRealtime _waitTime;


    public Color selectedColor = new Color(.5f, .5f, .5f, 1.0f);

    private static Tile previousSelected = null; // All Tiles have access to same previous selected tile

    private SpriteRenderer render;
    private bool isSelected = false;
    public TowerObject tower = null;


    private bool matchFound = false;


    public Vector2 boardPos = Vector2.zero;

    void OnDestroy()
    {
        Debug.Log(this.name + " Destroyed");
    }

    void Awake()
    {
        render = GetComponent<SpriteRenderer>();
    }

    public void StartShooting()
    {
        //_waitTime = new WaitForSecondsRealtime(shootDelay);
        //StartCoroutine(this.ShootMonsters());
    }

    //IEnumerator ShootMonsters()
    //{

    //    yield return new WaitForSeconds(shootDelay);
    //    Debug.Log(this.name + " with " + tower.name + " Starts shooting");

    //    if (tower != null && tower.damage != 0)
    //    {
    //        Enemy enemyToshoot = Shoot();

    //        if (enemyToshoot != null)
    //        {
    //            Debug.Log(tower.name + " is shooting at " + enemyToshoot.gameObject.name);
    //            GameObject bullet = Instantiate(BoardManager.instance.bulletPrefab, transform.position, Quaternion.identity);
    //            bullet.GetComponent<Bullet>().Shoot(tower, enemyToshoot);
    //        }

    //    }

    //    StartCoroutine(ShootMonsters());
        
    //}


    //public Enemy Shoot()
    //{
    //    if (tower.range == 0.0f)
    //    {
    //        Debug.Log(tower.name + "  of  " + this.name +" tower has attack but no range??");
    //        return null;
    //    }

    //    float closest = 100.0f;
    //    Enemy closestEnemy = null;
    //    foreach (Enemy enemy in BoardManager.instance.allEnemies)
    //    {
    //        float dist = Vector3.Distance(transform.position, enemy.transform.position);
    //        if (dist < closest)
    //        {
    //            closest = dist;
    //            closestEnemy = enemy;
    //        }
    //    }

    //    if (closest > tower.range * 3.0f)
    //    {
    //        Debug.Log(tower.name + "  of  " + this.name + "Did not find a close enemy");
    //        return null;
    //    }

    //    return closestEnemy;

    //    // instantiate bullet with target closest enemy in range
    //}



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
        //SFXManager.instance.PlaySFX(Clip.Select);
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
                SwapTower(previousSelected); // 2

                previousSelected.ClearAllMatches();
                ClearAllMatches();

                GUIManager.instance.MoveCounter--;
                if (GUIManager.instance.MoveCounter == 0)
                {
                    BoardManager.instance.TriggerNextPhase();
                }

                previousSelected.Deselect();
            }
        }
    }

    public void SwapTower(Tile otherTile)
    { // 1
        if (tower == otherTile.tower)
        { // 2
            return;
        }

        TowerObject tempTowerObj = tower;
        SetTower(otherTile.tower);
        otherTile.SetTower(tempTowerObj);
    }


    // Finds matching tiles in only one direction CastDir (x/y)
    private List<GameObject> FindMatch(Vector2 castDir)
    { 
        List<GameObject> matchingTiles = new List<GameObject>();


        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, castDir);
        while(hits.Length > 1 && hits[1].collider != null && hits[1].collider.GetComponent<Tile>().tower == tower)
        { 
            matchingTiles.Add(hits[1].collider.gameObject);
            hits = Physics2D.RaycastAll(hits[1].collider.transform.position, castDir);
        }



        return matchingTiles;
    }


    private void ClearMatch(Vector2[] paths)
    {
        List<GameObject> matchingTiles = new List<GameObject>(); 
        for (int i = 0; i < paths.Length; i++) 
        {
            matchingTiles.AddRange(FindMatch(paths[i]));
        }
        if (matchingTiles.Count >= 2)
        {
            for (int i = 0; i < matchingTiles.Count; i++)
            {
                matchingTiles[i].GetComponent<Tile>().SetTower(null);
            }

            matchFound = true; 
        }
    }


    public void ClearAllMatches()
    {

        if (tower == null)
            return;


        TowerObject previousTower = tower;

        ClearMatch(new Vector2[2] { Vector2.left, Vector2.right });
        ClearMatch(new Vector2[2] { Vector2.up, Vector2.down });


        if (matchFound)
        {
            SetTower(previousTower.nextLevelTower);
            matchFound = false;

            StopCoroutine(BoardManager.instance.FindNullTiles());
            StartCoroutine(BoardManager.instance.FindNullTiles());
        }
    }
}