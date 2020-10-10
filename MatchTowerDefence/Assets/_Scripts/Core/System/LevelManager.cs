using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameCore.System
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager instance = null;

        private TutorialManager tutorialManager;
        private int hasTutored = 0;
        private int enemyRemaining = 99999;

        public static LevelManager Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new LevelManager();
                }
                return instance;
            }
        }

        private void Awake()
        {
            BoardManager boardManager = FindObjectOfType<BoardManager>();
            tutorialManager = FindObjectOfType<TutorialManager>();
            if (boardManager != null) { boardManager.Initialise(); }

            if (PlayerPrefs.HasKey("Tutored")) { hasTutored = PlayerPrefs.GetInt("Tutored"); }

            if (tutorialManager != null && hasTutored == 1) { /*tutorialManager.gameObject.SetActive(false);*/ }
        }

        private void Update()
        {
            if (BoardManager.instance != null)
                enemyRemaining = BoardManager.instance.allEnemies.Count;

            //if(enemyRemaining == 0 && GUIManager.instance.phaseTxt.text == "Defense Phase")
            //{
            //    Debug.Log("IsLevelFinished");
            //    GUIManager.instance.LevelIsFinished();
            //}

            //if (SceneManager.GetActiveScene().buildIndex > 0)
            //{
            //    GUIManager.instance.InitiateGame();
            //}
        }

        public int GetStarsForLevel(string id)
        {
            throw new NotImplementedException();
        }
    }
}
