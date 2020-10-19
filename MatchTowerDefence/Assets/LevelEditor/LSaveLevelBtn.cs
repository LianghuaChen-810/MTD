using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEditor;
using System;

namespace LevelEditor
{
    public class LSaveLevelBtn : MonoBehaviour
    {

        void Start()
        {
            Button btn = GetComponent<Button>();
            btn.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
#if UNITY_EDITOR

            string assetPath = "Assets/Resources/LevelData/" + nameipf.text + "/";//can also change the path and put data into Resources folder and use Resources.Load()

            //data transformation

            LevelData levelData = ScriptableObject.CreateInstance<LevelData>();
            levelData.levelName = nameipf.text;
            levelData.levelNo = 1;
            levelData.basehp = Convert.ToInt32(basehpipf.text);
            levelData.conditionthreshold = Convert.ToInt32(thresholdipf.text);
            levelData.routenum = LEditorManager.GetInstance().routes.Count;
            levelData.board = new int[LEdgeSpawner.verticalcapacity * LEdgeSpawner.horizontalcapacity];
            for (int i = 0; i < LEdgeSpawner.horizontalcapacity; i++)
            {
                for (int j = 0; j < LEdgeSpawner.verticalcapacity; j++)
                {
                    levelData.board[i * LEdgeSpawner.verticalcapacity + j] = (int)LEditorManager.GetInstance().map[i, j];
                }
            }

            List<RouteData> routesdata = new List<RouteData>();

            foreach (LRoute route in LEditorManager.GetInstance().routes)
            {
                RouteData rd = ScriptableObject.CreateInstance<RouteData>();

                rd.points = new Vector3[route.corners.Count];
                for (int i = 0; i < route.corners.Count; i++)
                {
                    rd.points[i] = route.corners[i];
                }
                /*
                int[] monstertypes = new int[route.monstersdata.Count];
                float[] monstertimes = new float[route.monstersdata.Count];

                for (int i = 0; i < route.monstersdata.Count; i++)
                {
                    monstertypes[i] = (int)route.monstersdata[i].type;
                    monstertimes[i] = route.monstersdata[i].spawntimeoffset;
                }

                rd.monstertypes = monstertypes;
                rd.monstertimes = monstertimes;*/


                routesdata.Add(rd);
            }

            List<int> marks = new List<int>();
            Debug.Log("entity num" + LEditorManager.GetInstance().waves.Count);
            for (int m = 0; m < LEditorManager.GetInstance().waves.Count; m++)
            {
                if (LEditorManager.GetInstance().waves[m] == null || LEditorManager.GetInstance().waves[m].listnum == 0)
                {
                    marks.Add(m);
                }
            }

            foreach (int n in marks)
            {
                if (n < LEditorManager.GetInstance().moves.Count)
                {
                    LEditorManager.GetInstance().moves[n] = 0;
                }
            }

            marks.Reverse();
            foreach(int n in marks)
            {
                LEditorManager.GetInstance().waves.RemoveAt(n);
            }
            Debug.Log("file num" + LEditorManager.GetInstance().waves.Count);
            levelData.wavenum = LEditorManager.GetInstance().waves.Count;

            levelData.moves = new int[levelData.wavenum];
            int p = 0;
            for (int i = 0; i < levelData.wavenum; i++)
            {

                if (p >= LEditorManager.GetInstance().moves.Count)
                {
                    levelData.moves[i] = 1;
                }
                else
                {
                    while (LEditorManager.GetInstance().moves[p] == 0)
                    {
                        if (p < LEditorManager.GetInstance().moves.Count)
                        {
                            p++;
                        }
                        if (p >= LEditorManager.GetInstance().moves.Count)
                        {
                            levelData.moves[i] = 1;
                            break;
                        }

                    }
                    if (p < LEditorManager.GetInstance().moves.Count)
                    {
                        levelData.moves[i] = LEditorManager.GetInstance().moves[p];
                        p++;
                    }
                }

            }

           

            //data persistence




            if (!Directory.Exists(assetPath))
                Directory.CreateDirectory(assetPath);


            string fullPath = assetPath + "/" + nameipf.text + ".asset";

            UnityEditor.AssetDatabase.DeleteAsset(fullPath);
            UnityEditor.AssetDatabase.CreateAsset(levelData, fullPath);

            for (int j = 0; j < routesdata.Count; j++)
            {
                string indiepath = assetPath + "/" + nameipf.text + "route" + j + ".asset";
                UnityEditor.AssetDatabase.DeleteAsset(indiepath);
                UnityEditor.AssetDatabase.CreateAsset(routesdata[j], indiepath);
            }

            for (int m = 0; m < LEditorManager.GetInstance().waves.Count; m++)
            {
                string monsterpath = assetPath + "/" + nameipf.text + "enemy" + m + ".asset";
                UnityEditor.AssetDatabase.DeleteAsset(monsterpath);
                if(LEditorManager.GetInstance().waves[m]!=null&& LEditorManager.GetInstance().waves[m].listnum!=0) 
                    UnityEditor.AssetDatabase.CreateAsset(LEditorManager.GetInstance().waves[m], monsterpath);

            }
            UnityEditor.AssetDatabase.Refresh();


            //access the test button
            LStep3TestBtn.cantest = true;
#endif

        }



        public InputField nameipf;
        public InputField basehpipf;
        public InputField thresholdipf;
    }
}