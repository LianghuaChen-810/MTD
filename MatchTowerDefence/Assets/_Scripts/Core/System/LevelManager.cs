using MatchTowerDefence.Level;
using MatchTowerDefence.Managers;
using MatchTowerDefence.SaveSystem;
using MatchTowerDefence.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameCore.System
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager instance = null;

        public LevelList levelList;

        private TutorialManager tutorialManager;


        void Update()
        {
            LevelControl.OnUpdate();
        }

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
                // Finds spawners needs to be "true" to search in children of root objects because they are inactive on SceneLoad
                List<Spawner> spawners = new List<Spawner>(FindObjectsOfType<Spawner>(true));
                LevelControl.Initialise(spawners);

                // Initialise Board Manager
                BoardManager boardManager = FindObjectOfType<BoardManager>();
                if(boardManager != null)
                {
                    if(BoardManager.instance != boardManager)
                    {
                        boardManager.Initialise();
                    }
                }
                tutorialManager = FindObjectOfType<TutorialManager>();
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
                SaveManager.instance.SaveData();
                
                Debug.Log(JsonUtility.ToJson(SaveManager.instance.saveData));
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
                return SaveManager.instance.saveData.IsLevelCompleted(levelId);
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
                return SaveManager.instance.saveData.GetNoOfStarsForLevel(levelId);
            }
            return 0;
        }
    }
}
