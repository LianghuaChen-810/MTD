using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

namespace LevelEditor
{
    public class LAddMonsterBtn : MonoBehaviour
    {
        void Start()
        {
            Button btn = GetComponent<Button>();
            btn.onClick.AddListener(OnClick);


            LEditorManager.GetInstance().waves.Add(ScriptableObject.CreateInstance<MonsterData>());

            Debug.Log("testtime");
            if (LEditorManager.GetInstance().readleveldata != null)
            {
                LoadData(LEditorManager.GetInstance().waves[0]);
            }
        }

        private void OnClick()
        {
            GameObject go = Instantiate(routePrefab);
            go.GetComponent<RectTransform>().SetParent(generateparent.GetComponent<RectTransform>());

            options.Add(go.GetComponent<LMonsterOption>());

            RefreshOptions();
        }

        public static void RefreshOptions()
        {
            for (int i = 0; i < options.Count; i++)
            {

                options[i].GetComponent<RectTransform>().localPosition = new Vector3(150, -20 - 33f * i, 0);
                if (options[i].GetComponent<LRouteOption>() != null) options[i].GetComponent<LRouteOption>().txt.text = "Route" + i.ToString();
            }
        }


        public GameObject routePrefab;
        public GameObject generateparent;

        public static List<LMonsterOption> options = new List<LMonsterOption>();

        public void LoadData(MonsterData md)
        {
            if (md != null)
            {
                if (options.Count > 0)
                {
                    foreach (LMonsterOption option in options)
                    {
                        Destroy(option.gameObject);
                    }
                    options.Clear();
                    Debug.Log(options.Count);
                }
                if(md.listnum!=0)
                for (int i = 0; i < md.listnum; i++) //if 0, just skip
                {
                    GameObject go = Instantiate(routePrefab);
                    go.GetComponent<RectTransform>().SetParent(generateparent.GetComponent<RectTransform>());
                    go.GetComponent<LMonsterOption>().typedp.value = md.monstertypes[i];
                    go.GetComponent<LMonsterOption>().timeipf.text = md.monstertimes[i].ToString();
                    go.GetComponent<LMonsterOption>().routedp.GetComponent<LMonsterOptionRouteDp>().SetOriValueByInt( md.monsterroutes[i]);

                    options.Add(go.GetComponent<LMonsterOption>());
                }

                RefreshOptions();
            }
            else
            {
                Debug.Log("NullWave");
            }
        }
    }
}