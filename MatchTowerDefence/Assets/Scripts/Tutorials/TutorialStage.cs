using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStage : MonoBehaviour
{
    [TextArea(3, 10)]
    public string text;

    public List<TutorialRenderObject> stageObjects = new List<TutorialRenderObject>();

    bool isOn = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isOn && Input.GetKeyDown(KeyCode.Mouse0))
        {
            StopStage();
            TutorialManager.instance.NextTutorialStage();
        }
    }

    public void ExecuteStage()
    {
        foreach (var tro in stageObjects)
        {
            tro.gameObject.SetActive(true);
        }
        isOn = true;
    }

    public void StopStage()
    {
        isOn = false;
        foreach (var tro in stageObjects)
        {
            tro.gameObject.SetActive(false);

        }
        TutorialManager.instance.tutCanvas.SetActive(true);
    }
}
