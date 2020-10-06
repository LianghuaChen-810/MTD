using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour
{
    //[SerializeField]
    //float shootDelay = 1.0f;

    public enum TileType {NONE, TOWER, PATH, SPAWN};

    public Color selectedColor = new Color(.5f, .5f, .5f, 1.0f);

    private static Tile previousSelected = null; // All Tiles have access to same previous selected tile

    private SpriteRenderer render;
    private bool isSelected = false;
    public TowerObject tower = null;


    private bool matchFound = false;


    public Vector2 boardPos = Vector2.zero;
    public TileType type = TileType.NONE;

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

                ShapeMatch shape = new ShapeMatch(previousSelected);
                if (shape.matchFound)
                    shape.PrintShape();
                shape = new ShapeMatch(this);
                if (shape.matchFound)
                    shape.PrintShape();

                //previousSelected.ClearAllMatches();
                //ClearAllMatches();

                //GUIManager.instance.MoveCounter--;
                //if (GUIManager.instance.MoveCounter == 0)
                //{
                //    BoardManager.instance.TriggerNextPhase();
                //}

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


    private void ClearMatch(Vector2[] paths)
    {
        List<GameObject> matchingTiles = new List<GameObject>(); 

        // Find matches for left-right or top-down (paths)
        for (int i = 0; i < paths.Length; i++) 
        {
            List<GameObject> matchingTilesDir = new List<GameObject>();
 
            RaycastHit2D hit = Physics2D.Raycast(transform.position, paths[i]);

            int count = 0; // Just in case of colldier loops
            while (hit.collider != null && hit.collider.GetComponent<Tile>().tower == tower)
            {

                matchingTilesDir.Add(hit.collider.gameObject);
                hit = Physics2D.Raycast(hit.collider.transform.position, paths[i]);
                if (count > 50)
                {
                    Debug.Log("Raycast loop! ");
                    break;
                }
                count++;
            }


            matchingTiles.AddRange(matchingTilesDir);
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