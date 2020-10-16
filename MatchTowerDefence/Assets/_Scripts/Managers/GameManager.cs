using GameCore.System;
using MatchTowerDefence.SaveSystem;
using UnityEngine;

namespace MatchTowerDefence.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        [SerializeField] private GameObject[] managersPrefabs;

        private LevelManager levelManager;
        private GUIManager guiManager;
        private SFXManager sfxManager;
        private SaveManager saveManager;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            // Only 1 Game Manager can exist at a time
            if (instance == null)
            {
                instance = GetComponent<GameManager>();
            }
            else
            {
                Destroy(gameObject);
            }

            if (LevelManager.instance == null)
            {
                levelManager = FindObjectOfType<LevelManager>();
            }

            if (GUIManager.instance == null)
            {
                guiManager = FindObjectOfType<GUIManager>();
            }

            if (SFXManager.instance == null)
            {
                sfxManager = FindObjectOfType<SFXManager>();
            }

            if (SaveManager.instance == null)
            {
                saveManager = FindObjectOfType<SaveManager>();
            }


        }
    }
}
