using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace LevelEditor
{
    public class LAddRouteBtn : MonoBehaviour
    {
        void Start()
        {
            Button btn = GetComponent<Button>();
            btn.onClick.AddListener(OnClick);

            if (LEditorManager.GetInstance().readleveldata!=null)
            {
                LoadDataAndVisualizeRoutes();
            }
        }

        private void OnClick()
        {
            GameObject go = Instantiate(routePrefab);
            go.GetComponent<RectTransform>().SetParent( generateparent.GetComponent<RectTransform>());
            
            options.Add(go.GetComponent<RectTransform>());
            
            RefreshOptions();

            //add option of monster route
            LMonsterOptionRouteDp.dropdownnum += 1;
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

        public static List<RectTransform> options=new List<RectTransform>();

        public void LoadDataAndVisualizeRoutes()
        {
           
            for(int i = 0; i < LEditorManager.GetInstance().routes.Count; i++)
            {
                GameObject go = Instantiate(routePrefab);
                go.GetComponent<RectTransform>().SetParent(generateparent.GetComponent<RectTransform>());
                go.GetComponent<LRouteOption>().LoadExistingRoute(LEditorManager.GetInstance().routes[i]);
                options.Add(go.GetComponent<RectTransform>());

            }
            RefreshOptions();
        }
    }
}