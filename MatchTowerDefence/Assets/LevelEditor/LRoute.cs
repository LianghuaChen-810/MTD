using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LevelEditor
{
    //class generating entires of routes in the memory
    public class LRoute
    {
        public struct MonsterData
        {
            public LEditorManager.MonsterType type;
            public float spawntimeoffset;
        }

        public List<MonsterData> monstersdata = new List<MonsterData>();

        public List<Vector3> corners=new List<Vector3>();
    }

}