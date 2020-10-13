using GameCore.System;
using System;
using UnityEngine;

namespace MatchTowerDefence.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        private LevelManager levelManager;
        private GUIManager guiManager;

        private void Awake()
        {
            // Only 1 Game Manager can exist at a time
            if (instance == null)
            {
                DontDestroyOnLoad(gameObject);
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

            InitiliaseAllManagers();
        }

        private void InitiliaseAllManagers()
        {
            levelManager.Initialise();
            guiManager.Initialise();
        }
    }
}
