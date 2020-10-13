﻿using MatchTowerDefence.Level;
using MatchTowerDefence.UI;
using System;
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
        private int hasTutored = 0;

        void Update()
        {
            LevelControl.OnUpdate();
        }
        public void Initialise()
        {
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
                Debug.Log("INITIALISING AFTER SCENE IS LOADED");
                Debug.Log(FindObjectsOfType<Spawner>());
                List<GameObject> objects = new List<GameObject>();
                objects.AddRange(scene.GetRootGameObjects());

                List<Spawner> spawners = new List<Spawner>();
                foreach (var obj in objects)
                {
                    if (obj.GetComponent<Spawner>() != null)
                    {
                        spawners.Add(obj.GetComponent<Spawner>());
                    }
                }
                LevelControl.Initialise(spawners);

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
            return true; //DataStore.IsLevelCompleted(levelId);
        }

        public int GetStarsForLevel(string levelId)
        {
            if (!levelList.ContainsKey(levelId))
            {
                Debug.LogWarningFormat("Cannot check if level is completed , Not in level list", levelId);
                return 0;
            }
            return 3; //DataStore.GetNumberofStarForLevel(levelId);
        }
    }
}
