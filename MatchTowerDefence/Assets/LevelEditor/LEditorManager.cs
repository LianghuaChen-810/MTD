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
        public LRoute[] routes;

        public ElementType usingelement;
    }
}