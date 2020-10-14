using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
namespace LevelEditor
{
    public class LRouteOptionDisplayBtn : MonoBehaviour
    {

        void Start()
        {
            Button btn = GetComponent<Button>();
            btn.onClick.AddListener(OnClick);

        }

        private void OnClick()
        {
            if (LRayCaster.lineren != null)
            {
                LRayCaster.lineren.gameObject.SetActive(true);
                LRayCaster.lineren.positionCount = routedata.corners.Count;
                for (int i = 0; i < routedata.corners.Count; i++)
                {
                    LRayCaster.lineren.SetPosition(i,new Vector3( routedata.corners[i].x, routedata.corners[i].y,-2));
                }
                Invoke("EliminateLine",2f);
            }
        }

        public void EliminateLine()
        {
            if (LRayCaster.lineren != null)
            {
                LRayCaster.lineren.positionCount = 1;
                LRayCaster.lineren.SetPosition(0, Vector3.zero);
                LRayCaster.lineren.gameObject.SetActive(false);
            }
        }

        public LRoute routedata;
    }
}
