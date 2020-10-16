using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LevelEditor
{
    public class LRayCaster : MonoBehaviour
    {
        void Start()
        {
            lineren = lineobj;

            LEditorManager.GetInstance().InitBoard();
            visualboardelements = new GameObject[LEdgeSpawner.horizontalcapacity, LEdgeSpawner.verticalcapacity];

            for (int i = 0; i < LEdgeSpawner.horizontalcapacity; i++)
            {
                for (int j = 0; j < LEdgeSpawner.verticalcapacity; j++)
                {
                    visualboardelements[i, j] = null;
                }
            }
        }

        public GameObject[,] visualboardelements;

        [HideInInspector]
        public Vector2Int baseposition = new Vector2Int(-1, -1);

        public GameObject basevisualobject;

        List<Vector2Int> routeline = new List<Vector2Int>();

        public GameObject spawnervisualobject;
        public GameObject pathwayvisualobject;

        public void SetBaseData(int nx, int ny)
        {
            if (baseposition.x < 0)
            {
                //first time set data
            }
            else
            {
                //update data
                if (LEditorManager.GetInstance().routes.Count == 0)
                {
                    LEditorManager.GetInstance().map[baseposition.x, baseposition.y] = LEditorManager.ElementType.Empty;
                    Destroy(visualboardelements[baseposition.x, baseposition.y]);
                    visualboardelements[baseposition.x, baseposition.y] = null;
                }
                else return;
            }
            baseposition.x = nx;
            baseposition.y = ny;

            LEditorManager.GetInstance().map[nx, ny] = LEditorManager.ElementType.Base;

            GameObject bobj = Instantiate(basevisualobject);
            bobj.transform.position = new Vector3(nx, ny, 0);

            visualboardelements[nx, ny] = bobj;

        }
        RaycastHit rh;
        void Update()
        {
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(r, out rh, Mathf.Infinity))
            {
                Vector3 rhpoint = rh.point;

                int rx = Mathf.RoundToInt(rhpoint.x);
                int ry = Mathf.RoundToInt(rhpoint.y);
                if (rx >= 0 && rx < LEdgeSpawner.horizontalcapacity)
                    if (ry >= 0 && ry < LEdgeSpawner.verticalcapacity)
                    {
                        if (LEditorManager.step == 1)
                        {
                            if (Input.GetKey(KeyCode.Space))
                            {
                                //input
                                //eliminate
                                if (Input.GetMouseButtonDown(1))
                                {

                                    if (LEditorManager.GetInstance().map[rx, ry] != LEditorManager.ElementType.Empty)
                                    {
                                        LEditorManager.GetInstance().map[rx, ry] = LEditorManager.ElementType.Empty;
                                        Destroy(visualboardelements[rx, ry]);
                                        visualboardelements[rx, ry] = null;
                                    }

                                }


                                //place
                                if (LEditorManager.GetInstance().usingelement == LEditorManager.ElementType.Empty)
                                {
                                    //do nothing
                                }
                                else if (LEditorManager.GetInstance().usingelement == LEditorManager.ElementType.Base)
                                {

                                    //set the base
                                    if (Input.GetMouseButtonDown(0))
                                    {

                                        if (LEditorManager.GetInstance().map[rx, ry] == LEditorManager.ElementType.Empty)
                                        {
                                            SetBaseData(rx, ry);
                                        }


                                    }
                                }
                                else if (LEditorManager.GetInstance().usingelement == LEditorManager.ElementType.Spawner)
                                {

                                    if (Input.GetMouseButtonDown(0))
                                    {

                                        if (LEditorManager.GetInstance().map[rx, ry] == LEditorManager.ElementType.Empty)
                                        {
                                            GameObject go = Instantiate(spawnervisualobject);
                                            go.transform.position = new Vector3(rx, ry, 0);

                                            LEditorManager.GetInstance().map[rx, ry] = LEditorManager.GetInstance().usingelement;

                                            visualboardelements[rx, ry] = go;
                                        }


                                    }
                                    /*
                                    //prepare to draw lines of routes
                                    if (Input.GetMouseButtonDown(0))
                                    {
                                        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
                                        if (Physics.Raycast(r, out rh, Mathf.Infinity))
                                        {
                                            Vector3 rhpoint = rh.point;

                                            int rx = Mathf.RoundToInt(rhpoint.x);
                                            int ry = Mathf.RoundToInt(rhpoint.y);
                                            if (rx >= 0 && rx < LEdgeSpawner.horizontalcapacity)
                                                if (ry >= 0 && ry < LEdgeSpawner.verticalcapacity)
                                                    if (LEditorManager.GetInstance().map[rx, ry] == LEditorManager.ElementType.Empty)
                                                    {
                                                        Debug.Log(rx + "+" + ry);
                                                        routeline.Add(new Vector2Int(rx, ry));


                                                        lineobj.gameObject.SetActive(true);
                                                        lineobj.SetPosition(0, new Vector3(rx, ry, 0));
                                                    }

                                        }
                                    }

                                    if (Input.GetMouseButton(0))
                                    {
                                        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
                                        if (Physics.Raycast(r, out rh, Mathf.Infinity))
                                        {
                                            Vector3 rhpoint = rh.point;
                                            rhpoint.z = 0;

                                            int rx = Mathf.RoundToInt(rhpoint.x);
                                            int ry = Mathf.RoundToInt(rhpoint.y);
                                            if (rx >= 0 && rx < LEdgeSpawner.horizontalcapacity)
                                                if (ry >= 0 && ry < LEdgeSpawner.verticalcapacity)
                                                    if (LEditorManager.GetInstance().map[rx, ry] == LEditorManager.ElementType.Empty || LEditorManager.GetInstance().map[rx, ry] == LEditorManager.ElementType.Pathway)
                                                    {
                                                        int eulerdis = (routeline[routeline.Count - 1].x - rx) * (routeline[routeline.Count - 1].x - rx) + (routeline[routeline.Count - 1].y - ry) * (routeline[routeline.Count - 1].y - ry);
                                                        if (eulerdis == 1)
                                                        {
                                                            Debug.Log(rx + "+" + ry);

                                                            lineobj.positionCount = routeline.Count + 1;
                                                            lineobj.SetPosition(routeline.Count , new Vector3(rx, ry, 0));
                                                            routeline.Add(new Vector2Int(rx, ry));


                                                        }
                                                    }

                                        }
                                    }

                                    if (Input.GetMouseButtonUp(0))
                                    {

                                        lineobj.gameObject.SetActive(false);
                                        int eulerdis = (routeline[routeline.Count - 1].x - baseposition.x) * (routeline[routeline.Count - 1].x - baseposition.x) + (routeline[routeline.Count - 1].y - baseposition.y) * (routeline[routeline.Count - 1].y - baseposition.y);
                                        //  if (routeline[routeline.Count - 1].x == baseposition.x && routeline[routeline.Count - 1].y == baseposition.y) //wrong
                                        if(eulerdis==1)
                                        {
                                            //create new route
                                            LRoute lr = new LRoute();
                                            foreach (Vector2Int point in routeline)
                                            {
                                                lr.corners.Add(new Vector3(point.x, point.y, 0));
                                            }
                                            LEditorManager.GetInstance().map[routeline[0].x, routeline[0].y] = LEditorManager.ElementType.Spawner;
                                            GameObject sobj = Instantiate(spawnervisualobject);
                                            sobj.transform.position = new Vector3(routeline[0].x, routeline[0].y, 0);
                                            visualboardelements[routeline[0].x, routeline[0].y] = sobj;
                                            routeline.RemoveAt(0);

                                            foreach (Vector2Int point in routeline)
                                            {
                                                if (LEditorManager.GetInstance().map[point.x, point.y] == LEditorManager.ElementType.Empty)
                                                {
                                                    LEditorManager.GetInstance().map[point.x, point.y] = LEditorManager.ElementType.Pathway;
                                                    GameObject pobj = Instantiate(pathwayvisualobject);
                                                    pobj.transform.position = new Vector3(point.x, point.y, 0);
                                                    visualboardelements[point.x, point.y] = pobj;
                                                }
                                            }


                                            //also add this route to UI...... 


                                        }
                                        //then clear data
                                        routeline = new List<Vector2Int>();
                                        lineobj.positionCount = 1;
                                        lineobj.SetPosition(0, Vector3.zero);
                                    }
                                    */
                                }
                                else if (LEditorManager.GetInstance().usingelement == LEditorManager.ElementType.Pathway)
                                {
                                    //set pathway
                                    if (Input.GetMouseButton(0))
                                    {

                                        if (LEditorManager.GetInstance().map[rx, ry] == LEditorManager.ElementType.Empty)
                                        {
                                            GameObject go = Instantiate(pathwayvisualobject);
                                            go.transform.position = new Vector3(rx, ry, 0);

                                            LEditorManager.GetInstance().map[rx, ry] = LEditorManager.GetInstance().usingelement;

                                            visualboardelements[rx, ry] = go;
                                        }


                                    }
                                }
                                else
                                {
                                    //set a tower
                                    if (Input.GetMouseButtonDown(0))
                                    {

                                        if (LEditorManager.GetInstance().map[rx, ry] == LEditorManager.ElementType.Empty)
                                        {

                                            GameObject go = Instantiate(elementprefab);
                                            go.transform.position = new Vector3(rx, ry, 0);

                                            LEditorManager.GetInstance().map[rx, ry] = LEditorManager.GetInstance().usingelement;
                                            go.GetComponent<LTowerVisualObj>().SetSpriteByType(LEditorManager.GetInstance().usingelement);

                                            visualboardelements[rx, ry] = go;
                                        }

                                    }
                                }
                            }
                        }
                        else if (LEditorManager.step == 2)
                        {
                            if (LRouteOptionDrawBtn.candraw)
                            {
                                if (Input.GetMouseButtonDown(0))
                                {

                                    if (LEditorManager.GetInstance().map[rx, ry] == LEditorManager.ElementType.Spawner)
                                    {

                                        routeline.Add(new Vector2Int(rx, ry));


                                        lineobj.gameObject.SetActive(true);
                                        lineobj.positionCount = 1;
                                        lineobj.SetPosition(0, new Vector3(rx, ry, -2));
                                    }


                                }

                                if (Input.GetMouseButton(0))
                                {
                                    if (LEditorManager.GetInstance().map[rx, ry] == LEditorManager.ElementType.Base || LEditorManager.GetInstance().map[rx, ry] == LEditorManager.ElementType.Pathway)
                                    {
                                        int eulerdis = (routeline[routeline.Count - 1].x - rx) * (routeline[routeline.Count - 1].x - rx) + (routeline[routeline.Count - 1].y - ry) * (routeline[routeline.Count - 1].y - ry);
                                        if (eulerdis == 1)
                                        {


                                            lineobj.positionCount = routeline.Count + 1;
                                            lineobj.SetPosition(routeline.Count, new Vector3(rx, ry, -2));
                                            routeline.Add(new Vector2Int(rx, ry));


                                        }
                                    }


                                }

                                if (Input.GetMouseButtonUp(0))
                                {

                                    lineobj.gameObject.SetActive(false);
                                    //int eulerdis = (routeline[routeline.Count - 1].x - baseposition.x) * (routeline[routeline.Count - 1].x - baseposition.x) + (routeline[routeline.Count - 1].y - baseposition.y) * (routeline[routeline.Count - 1].y - baseposition.y);
                                    //  if (routeline[routeline.Count - 1].x == baseposition.x && routeline[routeline.Count - 1].y == baseposition.y) //wrong
                                    //if (eulerdis == 1)
                                    if (LEditorManager.GetInstance().map[rx, ry] == LEditorManager.ElementType.Base)
                                    {
                                        //create new route
                                        LRoute lr = new LRoute();
                                        foreach (Vector2Int point in routeline)
                                        {
                                            lr.corners.Add(new Vector3(point.x, point.y, 0));
                                        }

                                        //also add this route to UI...... 
                                        LRouteOptionDrawBtn.drawingroute.corners = lr.corners;//logically, changing refernece is wrong, changing data is right

                                    }
                                    //else directly clear data

                                    //then clear data
                                    // routeline = new List<Vector2Int>();
                                    routeline.Clear();
                                    lineobj.positionCount = 1;
                                    lineobj.SetPosition(0, Vector3.zero);
                                    //lineobj.SetPositions(new Vector3[1]);
                                }
                            }
                        }
                    }

            }
        }
        public LineRenderer lineobj;
        public GameObject elementprefab;//will be replaced by a prefab array

        public static LineRenderer lineren;
    }
}