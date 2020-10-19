using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace LevelEditor
{
    public class LReadFileByNameBtn : MonoBehaviour
    {
        void Start()
        {
            Button btn = GetComponent<Button>();
            btn.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {

            if (ipf.text != null)
            {

                LevelData data = LEditorManager.GetInstance().readleveldata = Resources.Load<LevelData>("LevelData/" + ipf.text + "/" + ipf.text);
                //LevelData data = Resources.Load<LevelData>("1");
                if (data != null)
                {
                    //load
                    Debug.Log("successfully load the file");

                    if (data.waves == null || data.waves.Length == 0)
                    {
                        Debug.Log("lackwaves");
                        data.waves = new MonsterData[data.wavenum];
                        for (int i = 0; i < data.wavenum; i++)
                        {
                            data.waves[i] = Resources.Load<MonsterData>("LevelData/" + ipf.text + "/" + ipf.text + "enemy" + i.ToString());
                        }
                    }

                    if (data.routes == null || data.routes.Length == 0)
                    {
                        Debug.Log("lackroutes");
                        data.routes = new RouteData[data.routenum];
                        for (int j = 0; j < data.routenum; j++)
                        {
                            data.routes[j]= Resources.Load<RouteData>("LevelData/" + ipf.text + "/" + ipf.text + "route" + j.ToString());
                            
                        }
                    }
                    LEditorManager.GetInstance().readleveldata = data;

                    //Set

                    for (int m = 0; m < LEdgeSpawner.horizontalcapacity; m++)
                    {
                        for (int n = 0; n < LEdgeSpawner.verticalcapacity; n++)
                        {
                            LEditorManager.GetInstance().map[m, n] = (LEditorManager.ElementType)(data.board[m * LEdgeSpawner.verticalcapacity + n]);
                        }
                    }
                    raycaster.LoadBoardData();

                    LMonsterOptionRouteDp.dropdownnum = data.routenum;
                    
                    foreach (RouteData rd in data.routes)
                    {
                        LRoute lr = new LRoute();
                        foreach (Vector3 pos in rd.points)
                        {
                            lr.corners.Add(pos);
                        }
                        LEditorManager.GetInstance().routes.Add(lr);
                    }

                    foreach (MonsterData md in data.waves)
                    {
                        LEditorManager.GetInstance().waves.Add(md);
                    }

                    nameipf.text = data.name;
                    basehpipf.text = data.basehp.ToString();
                    thresholdipf.text = data.conditionthreshold.ToString();
                }
            }


            step0.SetActive(false);
            step1.SetActive(true);
        }
        public InputField ipf;
        public GameObject step0;
        public GameObject step1;

        public InputField nameipf;
        public InputField basehpipf;
        public InputField thresholdipf;

        public LRayCaster raycaster;
    }
}