using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

namespace LevelEditor
{
    public class LAddMonsterBtn : MonoBehaviour//semi-finished
    {
        void Start()
        {
            Button btn = GetComponent<Button>();
            btn.onClick.AddListener(OnClick);
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
    }
}