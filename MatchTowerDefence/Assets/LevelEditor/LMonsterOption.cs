using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LMonsterOption : MonoBehaviour//semi-finished
{
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        { List<string> ls = new List<string>();
            ls.Add("ppp");
            dp.AddOptions(ls);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {

            dp.ClearOptions();
           // dp.op
        }
    }

    public Dropdown dp;
}
