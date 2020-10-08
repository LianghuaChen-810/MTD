using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameCore.System
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager instance = null;

        private int enemyRemaining = 0;

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

        private void Update()
        {
            enemyRemaining = BoardManager.instance.allEnemies.Count;

            if(enemyRemaining == 0 && GUIManager.instance.phaseTxt.text == "Defense Phase")
            {
                GUIManager.instance.LevelIsFinished();
            }

            if(SceneManager.GetActiveScene().buildIndex > 0)
            {
                GUIManager.instance.InitiateGame();
            }
        }

    }
}
