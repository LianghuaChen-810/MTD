using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelEditor
{
    public class LEditorManager
    {
        private static LEditorManager instance = new LEditorManager();
        public static LEditorManager GetInstance()
        {
            return instance;
        }

        public static int step = 0;

        public enum ElementType
        {
            Empty,
            Spawner,//spawning enemy
            Pathway,//visual route, not logical route
            Base,
            TowerType1Lv0,
            TowerType1Lv1,
            TowerType1Lv2,
            TowerType1Lv3,
            TowerType2Lv0,
            TowerType2Lv1,
            TowerType2Lv2,
            TowerType2Lv3,
            TowerType3Lv0,
            TowerType3Lv1,
            TowerType3Lv2,
            TowerType3Lv3

        }

        public enum MonsterType
        {
            Empty,
            Monster1,
            Monster2,
            Monster3
        }

        public ElementType[,] map;
        public List<LRoute> routes = new List<LRoute>();
        public MonsterData monsters;

        public ElementType usingelement = ElementType.Base;

        public void InitBoard()
        {
            map = new ElementType[LEdgeSpawner.horizontalcapacity, LEdgeSpawner.verticalcapacity];
        }

        //abandoned -- stack overflow
        public bool DepthFirstSearchRouteCheck(int x, int y)
        {

            if (map[x, y] == ElementType.Empty) return false; // cut branches
            
            else if (map[x, y] == ElementType.Spawner)
            {
                if (x >= 0 && y >= 0)
                {
                    if (x < LEdgeSpawner.horizontalcapacity && y < LEdgeSpawner.verticalcapacity)
                    {
                        //in the map
                        return true;
                    }
                    else return false;

                }
                else return false;
            }
            else if (map[x, y] == ElementType.Pathway || map[x, y] == ElementType.Base)
            {
                //not spawner, so go ahead
                if (DepthFirstSearchRouteCheck(x, y + 1)) return true;
                if (DepthFirstSearchRouteCheck(x - 1, y)) return true;
                if (DepthFirstSearchRouteCheck(x, y - 1)) return true;
                if (DepthFirstSearchRouteCheck(x + 1, y)) return true;

                return false;
            }

            else
            {
                //not on any pathways, so no further search
                return false;
            }



        }
    }
}