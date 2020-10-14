using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



namespace LevelEditor
{
    public class LStep1NextBtn : MonoBehaviour
    {
        void Start()
        {
            Button btn = GetComponent<Button>();
            btn.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            /*
            if(LEditorManager.GetInstance().DepthFirstSearchRouteCheck(lrc.baseposition.x, lrc.baseposition.y))
            //if (DepthFirstSearchRouteCheck(lrc.baseposition.x, lrc.baseposition.y))
            {
                //goto step2
                Debug.Log("yes");
            }
            else
            {
                //the map is not accessed

                Debug.Log("no");
            }
            */

            //abandon checking routes because of stack overflow
            //abandon dynamic programming because it is hard to keep the data effective
            step1.SetActive(false);
            step2.SetActive(true);
            LEditorManager.step = 2;
        }

        //abandoned -- stack overflow
        bool DepthFirstSearchRouteCheck(int x, int y)
        {

            if (x >= 0 && y >= 0)
            {
                if (x < LEdgeSpawner.horizontalcapacity && y < LEdgeSpawner.verticalcapacity)
                {
                    //in the map
                    if (LEditorManager.GetInstance().map[x, y] == LEditorManager.ElementType.Spawner)
                    {
                        return true;
                    }
                    else if (LEditorManager.GetInstance().map[x, y] == LEditorManager.ElementType.Pathway)
                    {
                        //not spawner, so go ahead
                    }
                    else if (LEditorManager.GetInstance().map[x, y] == LEditorManager.ElementType.Base)
                    {
                        //not spawner, so go ahead
                    }
                    else
                    {
                        //not on any pathways, so no further search
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            if (DepthFirstSearchRouteCheck(x, y + 1)) return true;
            if (DepthFirstSearchRouteCheck(x - 1, y)) return true;
            if (DepthFirstSearchRouteCheck(x, y - 1)) return true;
            if (DepthFirstSearchRouteCheck(x + 1, y)) return true;
            return false;
        }

        public LRayCaster lrc;

        public GameObject step1;
        public GameObject step2;

    }
}