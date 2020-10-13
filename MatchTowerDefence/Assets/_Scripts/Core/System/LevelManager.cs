using MatchTowerDefence.Level;
using MatchTowerDefence.Managers;
using MatchTowerDefence.SaveSystem;
using MatchTowerDefence.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameCore.System
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager instance = null;

        public LevelList levelList;

        private TutorialManager tutorialManager;
        private int hasTutored = 0;

        public void Awake()
        {
            DontDestroyOnLoad(gameObject);
            if (instance == null)
            {
                instance = GetComponent<LevelManager>();
            }
            else
            {
                Destroy(gameObject);
            }

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.buildIndex > 0)
            {
                BoardManager boardManager = FindObjectOfType<BoardManager>();
                tutorialManager = FindObjectOfType<TutorialManager>();

                if (boardManager != null) { boardManager.Initialise(); }

                if (PlayerPrefs.HasKey("Tutored")) { hasTutored = PlayerPrefs.GetInt("Tutored"); }

                if (tutorialManager != null && hasTutored == 1) { /*tutorialManager.gameObject.SetActive(false);*/ }

                
            }
            else
            {
                GUIManager.instance.MainMenuInstance();
            }

            GUIManager.instance.SetOnSceneLoaded();
        }


        public void CompleteLevel(string levelId, int starsEarned)
        {
            if(!levelList.ContainsKey(levelId))
            { 
                Debug.LogWarningFormat("Cannot complete level with id = {0}, Not in level list", levelId);
                return;
            }
            if (SaveManager.instance != null)
            {
                SaveManager.instance.saveData.CompleteLevel(levelId, starsEarned);
            }
        }

        public LevelItem LevelItemCurrentScene()
        {
            string sceneName = SceneManager.GetActiveScene().name;
            return levelList.GetLevelByScene(sceneName);
        }

        public bool IsLevelCompleted(string levelId)
        {
            if (!levelList.ContainsKey(levelId))
            {
                Debug.LogWarningFormat("Cannot check if level with id = {0} is completed , Not in level list", levelId);
                return false;
            }

            if (SaveManager.instance != null)
            {
                SaveManager.instance.saveData.IsLevelCompleted(levelId);
            }
            return false;
        }

        public int GetStarsForLevel(string levelId)
        {
            if (!levelList.ContainsKey(levelId))
            {
                Debug.LogWarningFormat("Cannot check if level is completed , Not in level list", levelId);
                return 0;
            }
            if (SaveManager.instance != null)
            {
                SaveManager.instance.saveData.IsLevelCompleted(levelId);
            }
            return 0;
        }
    }
}
