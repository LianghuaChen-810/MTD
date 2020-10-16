using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStage : MonoBehaviour
{
    public enum TutorialStageRequirement { NONE, ENEMY, TOWER, TURNS_LEFT};

    [Header ("Stage description")]
    [TextArea(3, 10)]
    public string text;
    public bool shouldHideOnFinish = true;
    public List<TutorialRenderObject> stageObjects = new List<TutorialRenderObject>();

    [Header("Stage requirements")]
    public TutorialStageRequirement requirement;

    [Header("Description of requirement")]
    public TowerObject towerToAppear;
    public EnemyObject enemyToAppear;
    public int turns_left;

    public bool isOn = false;
    public bool wasActivatedSoon = true;

    public bool RenderStage()
    {
        wasActivatedSoon = true;
        isOn = true;
        foreach (var tro in stageObjects)
        {
            tro.gameObject.SetActive(true);
        }
        StartCoroutine(ActivationPause());
        return shouldHideOnFinish;
    }

    IEnumerator ActivationPause()
    {
        yield return new WaitForSeconds(0.5f);
        wasActivatedSoon = false;
    }
    public void StopStage()
    {
        isOn = false;
        foreach (var tro in stageObjects)
        {
            tro.gameObject.SetActive(false);

        }
        TutorialManager.instance.tutTextMsg.SetActive(true);
    }
}
