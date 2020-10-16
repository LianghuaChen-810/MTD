using LevelEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LMonsterOption : MonoBehaviour//semi-finished
{
    void Start()
    {
        deletebtn.monster = this;
    }

    void Update()
    {
        
    }

    public int GetMonsterType()
    {
        return typedp.value;
    }

    public float GetSpawnTime()
    {
        return Convert.ToSingle(timeipf.text);
    }

    public int GetRouteNum()
    {
        return routedp.value;
    }

    public Dropdown typedp;    
    public InputField timeipf;
    public Dropdown routedp;
    public LMonsterOptionDeleteBtn deletebtn;
    
}
