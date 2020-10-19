using LevelEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LRouteOption : MonoBehaviour
{
    void Start()
    {
        if (LEditorManager.GetInstance().readleveldata == null)
        {
            LRoute lr = new LRoute();
            LEditorManager.GetInstance().routes.Add(lr);
            del.route = lr;
            dis.routedata = lr;
            dra.route = lr;
        }

    }

    void Update()
    {

    }

    public void LoadExistingRoute(LRoute lr)
    {
        del.route = lr;
        dis.routedata = lr;
        dra.route = lr;
    }

    public LRouteOptionDeleteBtn del;
    public LRouteOptionDisplayBtn dis;
    public LRouteOptionDrawBtn dra;
    public Text txt;
}
