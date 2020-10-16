using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor
{
    public class LMonsterOptionRouteDp : MonoBehaviour
    {
        void Start()
        {

        }

        void Update()
        {
            if (optionnum != dropdownnum)
            {
                optionnum = dropdownnum;
                dp.ClearOptions();
                List<string> ls = new List<string>();
                for(int i = 0; i < optionnum; i++)
                {
                    ls.Add("R" + i);
                }
                dp.AddOptions(ls);

            }
        }
        public Dropdown dp;

        public static int dropdownnum = 0;
        
        int optionnum = 0;
    }
}