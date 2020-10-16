using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using MatchTowerDefence.SaveSystem;
using GameCore.System;
using MatchTowerDefence.UI;
using System.Collections.Generic;

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

        [Header("Buttons")]
        public Button resumeButton = null;
        public Button pauseButton = null;
        public Button restartButton = null;
        public List<LevelSelectButton> levelButtons = null;

        private LevelSelectScreen levelSelect = null;
        private int score;
        private int moveCounter;
        private int enemiesReached = 0;
        private int currentLevel = 1;
        private int currentSpeed = 1;
        private bool tryAgain = false;

        public int EnemiesReached { get { return enemiesReached; } set { enemiesReached = value; } }

        public bool TryAgain { get { return tryAgain; } set { tryAgain = value; } }

        public int Score { get { return score; } set { score = value; phaseTxt.text = score.ToString(); } }



        [Header("Selected Tower info")]
        // ST stands for Selected Tower
        [SerializeField]
        private GameObject stPanel = null;
        [SerializeField]
        private Image stImage = null;
        [SerializeField]
        private TMP_Text stNameTxt = null;
        [SerializeField]
        private TMP_Text stLevelTxt = null;
        [SerializeField]
        private TMP_Text stDescTxt = null;
        [SerializeField]
        private TMP_Text stBaseDmgTxt = null;
        [SerializeField]
        private TMP_Text stBonusDmgTxt = null;
        [SerializeField]
        private TMP_Text stRangeTxt = null;

        [SerializeField]
        private GameObject stUpgradePanel = null;
        [SerializeField]
        private Image stCurrentImage = null;
        [SerializeField]
        private Image stUpgradeImage = null;


        [Header("Level Info")]
        [SerializeField]
        private GameObject LevelInfoPanel = null;
        //[SerializeField]
        //private TMP_Text levelNameTxt = null;
        [SerializeField]
        private TMP_Text waveCounterTxt = null;
        [SerializeField]
        private GameObject sp1Panel = null;
        [SerializeField]
        private Image sp1Image = null;
        [SerializeField]
        private TMP_Text sp1LUcounterTxt = null;
        [SerializeField]
        private TMP_Text sp1HUcounterTxt = null;

        [SerializeField]
        private GameObject sp2Panel = null;
        [SerializeField]
        private Image sp2Image = null;
        [SerializeField]
        private TMP_Text sp2LUcounterTxt = null;
        [SerializeField]
        private TMP_Text sp2HUcounterTxt = null;

        public int MoveCounter { get { return moveCounter; } set { moveCounter = value; moveCounterTxt.text = moveCounter.ToString(); } }

        public void Awake()
        {
            DontDestroyOnLoad(gameObject);
            if (instance == null)
            {
                instance = GetComponent<GUIManager>();
            }
            else
            {
                Destroy(gameObject);
            }
            levelSelect = levelSelectScreen.GetComponent<LevelSelectScreen>();
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
            LevelItem level = LevelManager.instance.LevelItemCurrentScene();
            levelFinishedMenu.SetActive(true);
            SetWinStars();
            score = Mathf.Abs(enemiesReached - 3);
            levelButtons[int.Parse(level.id) - 1].scorePanel.SetStars(score);
            LevelManager.instance.CompleteLevel(level.id, score);
            levelSelect.UpdateTotalStars();
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

            if (SaveManager.instance != null)
            {
                SaveManager.instance.SetVolumes(masterVolume, musicVolume, sfxVolume, true);
            }
        }

        private void GetSliderVolumes(out float masterVolume, out float musicVolume, out float sfxVolume)
        {
            masterVolume = masterSlider != null ? masterSlider.value : 1;
            musicVolume = musicSlider != null ? musicSlider.value : 1;
            sfxVolume = sfxSlider != null ? sfxSlider.value : 1;
        }

        public void LoadSliders()
        {
            if (SaveManager.instance != null)
            {
                float master, music, sfx;
                SaveManager.instance.GetVolumes(out master, out music, out sfx);

                if (masterSlider != null)
                {
                    masterSlider.value = master;
                }
                if (musicSlider != null)
                {
                    musicSlider.value = music;
                }
                if (sfxSlider != null)
                {
                    sfxSlider.value = sfx;
                }
            }
        }

        public void SaveSliders()
        {
            float masterVolume, musicVolume, sfxVolume;
            GetSliderVolumes(out masterVolume, out musicVolume, out sfxVolume);

            if (SaveManager.instance != null)
            {
                SaveManager.instance.SetVolumes(masterVolume, musicVolume, sfxVolume, true);
            }

        }

        // TOWER INFO UPDATE FUCNTIONS

        public void UpdateSelectedTower(TowerObject towerObj, int bonusDmg)
        {

            stImage.sprite = towerObj.sprite;
            stNameTxt.text = towerObj.name;
            stLevelTxt.text = towerObj.levelStr;
            stDescTxt.text = towerObj.description;
            stBaseDmgTxt.text = towerObj.baseDamage.ToString();
            stBonusDmgTxt.text = bonusDmg.ToString();
            stRangeTxt.text = towerObj.range.ToString();

            if (!towerObj.hasUpgrade)
            {
                stUpgradePanel.SetActive(false);
            }
            else
            {
                stCurrentImage.sprite = towerObj.sprite;
                stUpgradeImage.sprite = towerObj.nextLevelTower.sprite;
                stUpgradePanel.SetActive(true);
            }
            stPanel.SetActive(true);
        }

        public void CloseSelectedTower()
        {
            stPanel.SetActive(false);
        }


        // LEVEL INFO UPDATE FUNCTIONS

        public void SetLevelInfoUI(bool isOn)
        {
            LevelInfoPanel.SetActive(isOn);
        }

        public void SetSpawner1Panel(bool isOn)
        {
            sp1Panel.SetActive(isOn);
        }

        public void SetSpawner2Panel(bool isOn)
        {
            sp2Panel.SetActive(isOn);
        }

        public void SetWaveNumber(int current, int maxWaves)
        {
            waveCounterTxt.text = current.ToString() + " / " + maxWaves.ToString();
        }

        public void SetSP1Image(Sprite sprite)
        {
            sp1Image.sprite = sprite;
        }

        public void SetSP1LU(int count)
        {
            sp1LUcounterTxt.text = "x" + count.ToString();
        }

        public void SetSP1HU(int count)
        {
            sp1HUcounterTxt.text = "x" + count.ToString();

        }

        public void SetSP2LU(int count)
        {
            sp2LUcounterTxt.text = "x" + count.ToString();

        }

        public void SetSP2HU(int count)
        {
            sp2HUcounterTxt.text = "x" + count.ToString();

        }

        public void SetSP2Image(Sprite sprite)
        {
            sp2Image.sprite = sprite;
        }
    }
}
