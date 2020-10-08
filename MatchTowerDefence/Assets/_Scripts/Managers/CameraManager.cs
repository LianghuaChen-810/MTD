using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public static CameraManager instance;
    Camera cameraComponent;


    // Start is called before the first frame update
    void Start()
    {
        instance = GetComponent<CameraManager>();
        cameraComponent = GetComponent<Camera>();
    }

    void Awake()
    {
    }

    public void SetDisplay(float size, Vector3 focusPoint)
    {
        cameraComponent = GetComponent<Camera>();
        cameraComponent.orthographicSize = size;
        transform.position = new Vector3(focusPoint.x, focusPoint.y - 1.0f, -11.0f);
    }


}
