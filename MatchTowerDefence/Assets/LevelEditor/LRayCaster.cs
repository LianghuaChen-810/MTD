using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LevelEditor
{
    public class LRayCaster : MonoBehaviour
    {
        void Start()
        {

        }

        RaycastHit rh;
        void Update()
        {
            if (LEditorManager.GetInstance().usingelement == LEditorManager.ElementType.Empty)
            {
                //do nothing
            }
            else if (LEditorManager.GetInstance().usingelement == LEditorManager.ElementType.Base)
            {
                //set the base
            }
            else if (LEditorManager.GetInstance().usingelement == LEditorManager.ElementType.Spawner)
            {
                //prepare to draw lines of routes
            }
            else if (LEditorManager.GetInstance().usingelement == LEditorManager.ElementType.Pathway)
            {
                //do nothing
            }
            else
            {
                //set a tower
                if (Input.GetMouseButtonDown(0))
                {
                    Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(r, out rh, Mathf.Infinity))
                    {
                        Vector3 rhpoint = rh.point;
                        rhpoint.z = 0;

                        GameObject go = Instantiate(elementprefab);//not completed, need more prefabs(only for the editor, not the real ones in the game)
                        float rx = Mathf.Round(rhpoint.x);
                        float ry = Mathf.Round(rhpoint.y);
                        if (rx >= 0 && rx < LEdgeSpawner.horizontalcapacity)
                            if (ry >= 0 && ry < LEdgeSpawner.verticalcapacity)
                                go.transform.position = new Vector3(rx, ry, 0);

                    }
                }
            }
        }

        public GameObject elementprefab;//will be replaced by a prefab array
    }
}