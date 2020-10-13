using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using MatchTowerDefence.SaveSystem;

namespace MatchTowerDefence.Managers
{
    public class GUIManager : MonoBehaviour
    {
        public static GUIManager instance;

        [Header("HUD Texts")]
        public TMP_Text phaseTxt;
        [SerializeField] private TMP_Text moveCounterTxt;

        [Header("Menu Panels")]
        public GameObject pauseMenu = null;
        public GameObject levelFinishedMenu = null;
        [SerializeField] private GameObject mainMenu = null;
        [SerializeField] private GameObject gamePanel = null;
        [SerializeField] private GameObject winPanel = null;
        [SerializeField] private GameObject losePanel = null;
        [SerializeField] private GameObject levelSelectScreen = null;
        [SerializeField] private Image backGround = null;
        [SerializeField] private Sprite winStarSprite = null;
        [SerializeField] private Sprite whiteSprite = null;
        [SerializeField] private Image[] winStars = null;
        [SerializeField] private Image[] fastForwardSwaps = null;

        [Header("Menu Panels")]
        [SerializeField] private Slider masterSlider = null;
        [SerializeField] private Slider musicSlider = null;
        [SerializeField] private Slider sfxSlider = null;

        public Button resumeButton = null;
        public Button pauseButton = null;
        public Button restartButton = null;

        private int score;
        private int moveCounter;
        private int enemiesReached = 0;
        private int currentLevel = 1;
        private int currentSpeed = 1;
        private bool tryAgain = false;

        public int EnemiesReached { get { return enemiesReached; } set { enemiesReached = value; } }

        public bool TryAgain { get { return tryAgain; } set { tryAgain = value; } }

        public int Score { get { return score; } set { score = value; phaseTxt.text = score.ToString(); } }

        public int MoveCounter { get { return moveCounter; } set { moveCounter = value; moveCounterTxt.text = moveCounter.ToString(); } }

        public void Initialise()
        {
            if (instance == null)
            {
                DontDestroyOnLoad(gameObject);
                instance = GetComponent<GUIManager>();
            }
            else
            {
                Destroy(gameObject);
            }
        }


        public void SetOnSceneLoaded()
        {
            enemiesReached = 0;
            moveCounter = 5;
            moveCounterTxt.text = moveCounter.ToString();
            RestoreSpeed();
            tryAgain = false;
            for (int i = 0; i < 3; ++i)
            {
                winStars[i].sprite = whiteSprite;
            }
        }

        public void LevelSceneInstance()
        {
            mainMenu.SetActive(false);
            backGround.gameObject.SetActive(false);
            gamePanel.SetActive(true);
            pauseMenu.SetActive(false);
            losePanel.SetActive(false);
            winPanel.SetActive(false);
            levelSelectScreen.SetActive(false);
        }

        public void MainMenuInstance()
        {
            mainMenu.SetActive(true);
            backGround.gameObject.SetActive(true);
            gamePanel.SetActive(false);
            pauseMenu.SetActive(false);
            losePanel.SetActive(false);
            winPanel.SetActive(false);
        }


        public void Play()
        {
            SceneManager.LoadScene(1);
        }

        public void BackToMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public void Quit()
        {
            Application.Quit();
        }

        public void PauseMenuToggle()
        {
            if (pauseMenu.gameObject.activeSelf)
            {
                pauseMenu.SetActive(false);
                return;
            }
            pauseMenu.SetActive(true);
        }

        public void RestartLevel()
        {
            levelFinishedMenu.SetActive(false);
            SceneManager.LoadScene(currentLevel);
        }

        public void LevelIsFinished()
        {
            levelFinishedMenu.SetActive(true);
            SetWinStars();
        }

        public void FastForwardToggle()
        {
            if (currentSpeed == 1)
            {
                DoubleSpeed();
            }
            else
            {
                RestoreSpeed();
            }
        }

        private void RestoreSpeed()
        {
            fastForwardSwaps[1].gameObject.SetActive(false);
            fastForwardSwaps[0].gameObject.SetActive(true);
            Time.timeScale = 1;
            currentSpeed = 1;
        }

        private void DoubleSpeed()
        {
            fastForwardSwaps[0].gameObject.SetActive(false);
            fastForwardSwaps[1].gameObject.SetActive(true);
            Time.timeScale = 2;
            ++currentSpeed;
        }

        private void SetWinStars()
        {
            if (enemiesReached >= 3)
            {
                losePanel.SetActive(true);
            }
            else
            {
                winPanel.SetActive(true);
                for (int i = 0; i < Mathf.Abs(enemiesReached - 3); ++i)
                {
                    winStars[i].sprite = winStarSprite;
                }
            }
        }

        public void SetVolumes()
        {
            float masterVolume, musicVolume, sfxVolume;
            GetSliderVolumes(out masterVolume, out musicVolume, out sfxVolume);

            if(SaveManager.instance != null)
            {
                SaveManager.instance.SetVolumes(masterVolume, musicVolume, sfxVolume, false);
            }
        }

        private void GetSliderVolumes(out float masterVolume, out float musicVolume, out float sfxVolume)
        {
            masterVolume = masterSlider != null ? masterSlider.value : 1;
            musicVolume = musicSlider != null ? musicSlider.value : 1;
            sfxVolume = sfxSlider != null ? sfxSlider.value : 1;
        }
    }
}
