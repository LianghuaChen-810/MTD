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

    /// <summary>
    /// Sets the new visual range of the camera
    /// </summary>
    /// <param name="size"> Orthographic size of camera </param>
    /// <param name="focusPoint"> Focus point of camera </param>
    /// <param name="shiftY"> Shifting of the Y parameter of focus point</param>
    public void SetDisplay(float size, Vector3 focusPoint, float shiftY = 0.0f)
    {
        cameraComponent = GetComponent<Camera>();
        cameraComponent.orthographicSize = size;
        transform.position = new Vector3(focusPoint.x, focusPoint.y + shiftY, -11.0f);
    }


}