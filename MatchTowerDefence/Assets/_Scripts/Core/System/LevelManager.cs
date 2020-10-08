using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameCore.System
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager instance = null;

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
            var bm = FindObjectOfType<BoardManager>();
            bm.Initialise();
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

    }
}
