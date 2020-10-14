using LevelEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LRouteOption : MonoBehaviour
{
    void Start()
    {
        LRoute lr = new LRoute();
        LEditorManager.GetInstance().routes.Add(lr);
        del.route = lr;
        dis.routedata = lr;
        dra.route = lr;
        
    }

    void Update()
    {
        
    }

    public LRouteOptionDeleteBtn del;
    public LRouteOptionDisplayBtn dis;
    public LRouteOptionDrawBtn dra;
    public Text txt;
}
