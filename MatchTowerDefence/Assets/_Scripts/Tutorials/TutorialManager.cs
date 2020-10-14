using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public GameObject pressAnywhereTxt;
    public GameObject tutTextMsg;
    [SerializeField] private GameObject skipButton;
    public static TutorialManager instance;
    public TMP_Text textPanel;
    public List<TutorialStage> stages = new List<TutorialStage>();
    public TutorialStage currentStage = null;

    public bool isActive = false;
    private int nextStageIndex = 0;
    private bool shouldHideStage = false;
    private TutorialStage nextStage = null;
    private bool allowMsgSkip = false;

    // Start is called before the first frame update
    private void Start()
    {
        instance = GetComponent<TutorialManager>();
        ExecuteSearchForNextStageRequirement();
        skipButton.SetActive(true);
    }

    // Update is called once per frame
    private void Update()
    {
        //Touch needs to be handled here as well
        if (allowMsgSkip && isActive && Input.GetKeyDown(KeyCode.Mouse0))
        {
            isActive = false;
            allowMsgSkip = false;
            pressAnywhereTxt.SetActive(false);
            tutTextMsg.SetActive(false);

            if (shouldHideStage)
                currentStage.StopStage();

            // Invoke the function to wait for the next stage
            ExecuteSearchForNextStageRequirement();
        }

    }


    public void NextTutorialStage()
    {
        if (currentStage != null)
            if (currentStage.wasActivatedSoon) return;

        if (nextStageIndex < stages.Count)
        {
            isActive = true;

            ////Debug.Log("Next stage activating: " + stages[nextStageIndex]);
            // Always stop current stage before getting new one
            if(currentStage != null)
                currentStage.StopStage();

            // Get next stage
            currentStage = stages[nextStageIndex];
            nextStageIndex++;

            // Set that tutorial is active
            // isActive = true;    
            StartCoroutine(ActivateTutorial());

            // Set new text
            textPanel.text = currentStage.text;
            tutTextMsg.SetActive(true);

            // Activate stage and render objects of stage
            shouldHideStage = currentStage.RenderStage();
            //Debug.Log("Next stage index = " + nextStageIndex);

        }
        else
        {
            PlayerPrefs.SetInt("Tutored", 1);
            isActive = false;
            tutTextMsg.SetActive(false);
            skipButton.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    IEnumerator ActivateTutorial()
    {
        yield return new WaitForSeconds(1.0f);
        pressAnywhereTxt.SetActive(true);
        allowMsgSkip = true;
    }
    public void ExecuteSearchForNextStageRequirement()
    {
        //Debug.Log("SearchNextStage");
        // SWITCH FOR THE EXECUTION
        if (nextStageIndex < stages.Count)
            nextStage = stages[nextStageIndex];

        if (nextStage.requirement == TutorialStage.TutorialStageRequirement.NONE)
        {
            //Debug.Log("NONE");
            NextTutorialStage();
        }
    }


    public void TowerSpawnedEvent(TowerObject tower)
    {
        //Debug.Log("TOWER SPAWNED EVENT: " + tower.type);
        if (!isActive)
        {
            Debug.Log("TOWER SPAWNED EVENT: " + tower.type);

            if (nextStage.requirement == TutorialStage.TutorialStageRequirement.TOWER)
                if (tower.type == nextStage.towerToAppear.type)
                    NextTutorialStage();
        }
    }

    public void EnemySpawnedEvent(EnemyObject enemy)
    {
        if (!isActive)
            if (nextStage.requirement == TutorialStage.TutorialStageRequirement.ENEMY)
                if (enemy == nextStage.enemyToAppear)
                {
                    nextStage.enemyToAppear = null;
                    NextTutorialStage();
                }
    }

    public void MoveCounterReachEvent(int count)
    {
        if (!isActive)
            if (nextStage.requirement == TutorialStage.TutorialStageRequirement.TURNS_LEFT)
                if (count == nextStage.turns_left)
                 NextTutorialStage();
    }

    public void SkipTutorial()
    {
        nextStageIndex = 10;
    }
    // EXECUTION FUNCTIONS
}
