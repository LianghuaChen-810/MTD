using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutCanvas;
    public static TutorialManager instance;
    public TMP_Text textPanel;
    public bool isActive = true;
    int tutorialIndex = -1;
    public List<TutorialStage> stages = new List<TutorialStage>();

    // Start is called before the first frame update
    void Start()
    {
        instance = GetComponent<TutorialManager>();
        NextTutorialStage();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void NextTutorialStage()
    {
        if (tutorialIndex < stages.Count - 1)
        {
            isActive = true;    
            tutorialIndex++;
            // Activate next stage
            textPanel.text = stages[tutorialIndex].text;
            tutCanvas.SetActive(true);
            stages[tutorialIndex].ExecuteStage();

        } else
        {
            isActive = false;
            // FINISH TUTORIAL
            tutCanvas.SetActive(false);
        }
    }
}
