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

            string assetPath = "Assets/LevelEditor/LevelData/" + nameipf.text + "/";//can also change the path and put data into Resources folder and use Resources.Load()

            //data transformation

            LevelData levelData = ScriptableObject.CreateInstance<LevelData>();
            levelData.levelName = nameipf.text;
            levelData.levelNo = 1;
            levelData.basehp = Convert.ToInt32(basehpipf.text);
            levelData.conditionthreshold = Convert.ToInt32(thresholdipf.text);
            levelData.board = new int[LEdgeSpawner.verticalcapacity* LEdgeSpawner.horizontalcapacity];
            for (int i = 0; i < LEdgeSpawner.verticalcapacity; i++)
            {
                for (int j = 0; j < LEdgeSpawner.horizontalcapacity; j++)
                {
                    levelData.board[i * LEdgeSpawner.horizontalcapacity + j] = (int)LEditorManager.GetInstance().map[i, j];
                }
            }

            List<RouteData> routesdata = new List<RouteData>();

            foreach (LRoute route in LEditorManager.GetInstance().routes)
            {
                RouteData  rd =ScriptableObject.CreateInstance<RouteData>();

                rd.points = new Vector3[route.corners.Count];
                for(int i = 0; i < route.corners.Count; i++)
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

            //data persistence




            if (!Directory.Exists(assetPath))
                Directory.CreateDirectory(assetPath);


            string fullPath = assetPath + "/" + nameipf.text + ".asset";

            UnityEditor.AssetDatabase.DeleteAsset(fullPath);
            UnityEditor.AssetDatabase.CreateAsset(levelData, fullPath);

            for (int j = 0; j < routesdata.Count; j++)
            {
                string indiepath = assetPath + "/" + nameipf.text +"route"+j+".asset";
                UnityEditor.AssetDatabase.DeleteAsset(indiepath);
                UnityEditor.AssetDatabase.CreateAsset(routesdata[j], indiepath);
            }

            string monsterpath = assetPath + "/" + nameipf.text + "enemy" + ".asset";
            UnityEditor.AssetDatabase.DeleteAsset(monsterpath);
            UnityEditor.AssetDatabase.CreateAsset(LEditorManager.GetInstance().monsters, monsterpath);

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