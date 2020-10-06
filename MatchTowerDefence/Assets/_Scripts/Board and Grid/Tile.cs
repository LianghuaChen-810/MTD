using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour
{
    [SerializeField]
    float shootDelay = 1.0f;
    private WaitForSecondsRealtime _waitTime;



    public Color selectedColor = new Color(.5f, .5f, .5f, 1.0f);
    private static Tile previousSelected = null;

    private SpriteRenderer render;
    private bool isSelected = false;
    public TowerObject tower = null;

    private Vector2[] adjacentDirections = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };


    private bool matchFound = false;

    private bool shouldReturn = false;

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
        StartCoroutine(this.ShootMonsters());
    }

    IEnumerator ShootMonsters()
    {

        yield return new WaitForSeconds(shootDelay);
        Debug.Log(this.name + " with " + tower.name + " Starts shooting");

        if (tower != null && tower.damage != 0)
        {
            Enemy enemyToshoot = Shoot();

            if (enemyToshoot != null)
            {
                Debug.Log(tower.name + " is shooting at " + enemyToshoot.gameObject.name);
                GameObject bullet = Instantiate(BoardManager.instance.bulletPrefab, transform.position, Quaternion.identity);
                bullet.GetComponent<Bullet>().Shoot(tower, enemyToshoot);
            }

        }

        StartCoroutine(ShootMonsters());
        
    }


    public Enemy Shoot()
    {
        if (tower.range == 0.0f)
        {
            Debug.Log(tower.name + "  of  " + this.name +" tower has attack but no range??");
            return null;
        }

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

    void Update()
    {

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
        //if (GUIManager.instance.MoveCounter == 0) return;

        // 1
        if (tower == null || BoardManager.instance.IsShifting)
        {
            return;
        }
        //Debug.Log("tower = " + tower.ToString());
        //Debug.Log("Shifting = " + BoardManager.instance.IsShifting);

        if (isSelected)
        { // 2 Is it already selected?

            Deselect();
        } 
        else
        {
            if (previousSelected == null)
            { // 3 Is it the first tile selected?
                //Debug.Log("Selecting " + tower.ToString());
                Select();
            }
            else
            { // 3
                //Debug.Log("Swapping: " + tower.ToString() + " <-> " + previousSelected.tower.ToString());
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

    // Check for adjacent switching
    private GameObject GetAdjacent(Vector2 castDir)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, castDir);
        if (hit.collider != null)
        {
            return hit.collider.gameObject;
        }
        return null;
    }

    private List<GameObject> GetAllAdjacentTiles()
    {
        List<GameObject> adjacentTiles = new List<GameObject>();
        for (int i = 0; i < adjacentDirections.Length; i++)
        {
            adjacentTiles.Add(GetAdjacent(adjacentDirections[i]));
        }
        return adjacentTiles;
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


    private List<GameObject> FindMatch(Vector2 castDir)
    { // 1
        List<GameObject> matchingTiles = new List<GameObject>(); // 2


        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, castDir); // 3
        while(hits.Length > 1 && hits[1].collider != null && hits[1].collider.GetComponent<Tile>().tower == tower)
        { // 4
            matchingTiles.Add(hits[1].collider.gameObject);
            hits = Physics2D.RaycastAll(hits[1].collider.transform.position, castDir);
        }



        return matchingTiles; // 5
    }


    private void ClearMatch(Vector2[] paths) // 1
    {
        List<GameObject> matchingTiles = new List<GameObject>(); // 2
        for (int i = 0; i < paths.Length; i++) // 3
        {
            matchingTiles.AddRange(FindMatch(paths[i]));
        }
        if (matchingTiles.Count >= 2) // 4
        {
            for (int i = 0; i < matchingTiles.Count; i++) // 5
            {
                ////   matchingTiles[i].GetComponent<SpriteRenderer>().sprite = null;
                matchingTiles[i].GetComponent<Tile>().SetTower(null);
            }

            matchFound = true; // 6
        }
    }


    public void ClearAllMatches()
    {


        //Debug.Log("For tile = " + gameObject.name);
        //if (render.sprite == null)
        if (tower == null)
            return;

        
        //Debug.Log("Tower is not null = " + tower.name);

        TowerObject previousTower = tower;


        //bool drawlineOnce = false;
        ClearMatch(new Vector2[2] { Vector2.left, Vector2.right });

        //bool drawlineOnce = false;
        //if (matchFound)
        //{
        //    Debug.Log("---------------------------------------------");
        //    drawlineOnce = true;
        //    Debug.Log("MatchFound at start position: " + gameObject.name + " Going Left and Right");
        //}

        ClearMatch(new Vector2[2] { Vector2.up, Vector2.down });
        //if (matchFound)
        //{
        //    if (!drawlineOnce)
        //        Debug.Log("---------------------------------------------");
        //    Debug.Log("MatchFound at start position: " + gameObject.name + " Going Up and Down");
        //}


 

        if (matchFound)
        {
            SetTower(previousTower.nextLevelTower);
            matchFound = false;

            StopCoroutine(BoardManager.instance.FindNullTiles());
            StartCoroutine(BoardManager.instance.FindNullTiles());
        } else
            {
                shouldReturn = true;
            }
        }
}