using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LevelEditor
{
    public class LEdgeSpawner : MonoBehaviour
    {
        public static int horizontalcapacity=20;
        public static int verticalcapacity=20;
        void Start()
        {
            //spawn visual edge
            for(int i =-1;i<= horizontalcapacity; i++)
            {
                GameObject go1 = Instantiate(edgeBlock);
                go1.transform.position = new Vector3(i,-1, 0);
                GameObject go2 = Instantiate(edgeBlock);
                go2.transform.position = new Vector3(i, verticalcapacity, 0);
            }

            for (int j= 0; j <= verticalcapacity - 1; j++)
            {
                GameObject go1 = Instantiate(edgeBlock);
                go1.transform.position = new Vector3(-1, j, 0);
                GameObject go2 = Instantiate(edgeBlock);
                go2.transform.position = new Vector3(horizontalcapacity, j, 0);
            }
        }

        void Update()
        {

        }

        public GameObject edgeBlock;
    }
}