using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GUIManager : MonoBehaviour 
{
	public static GUIManager instance;

    [Header("HUD Texts")]
    public TMP_Text phaseTxt;
    [SerializeField] private TMP_Text moveCounterTxt;

    [Header("Menu Panels")]
    public GameObject pauseMenu;
    public GameObject levelFinishedMenu;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject levelSelectScreen;
    [SerializeField] private Image backGround;
    [SerializeField] private Sprite winStarSprite;
    [SerializeField] private Sprite whiteSprite;
    [SerializeField] private Image[] winStars;
    [SerializeField] private Image[] fastForwardSwaps;

    public Button resumeButton;
    public Button pauseButton;
    public Button restartButton;

    private int score;
    private int moveCounter;
    private int enemiesReached = 0;
    private int currentLevel = 1;
    private int currentSpeed = 1;
    private bool tryAgain = false;

    public int EnemiesReached { get { return enemiesReached; } set { enemiesReached = value; } }

    public bool TryAgain { get { return tryAgain; } set { tryAgain = value; } }

    public int Score { get { return score; } set { score = value; phaseTxt.text = score.ToString(); } }

    public int MoveCounter  { get { return moveCounter; } set { moveCounter = value; moveCounterTxt.text = moveCounter.ToString(); } }



    [Header("Selected Tower info")]
    // ST stands for Selected Tower
    [SerializeField]
    private GameObject stPanel;
    [SerializeField]
    private Image stImage;
    [SerializeField]
    private TMP_Text stNameTxt;
    [SerializeField]
    private TMP_Text stLevelTxt;
    [SerializeField]
    private TMP_Text stDescTxt;
    [SerializeField]
    private TMP_Text stBaseDmgTxt;
    [SerializeField]
    private TMP_Text stBonusDmgTxt;
    [SerializeField]
    private TMP_Text stRangeTxt;

    [SerializeField]
    private GameObject stUpgradePanel;
    [SerializeField]
    private Image stCurrentImage;
    [SerializeField]
    private Image stUpgradeImage;


    [Header("Level Info")]
    [SerializeField]
    private GameObject LevelInfoPanel;
    [SerializeField]
    private TMP_Text levelNameTxt;
    [SerializeField]
    private TMP_Text waveCounterTxt;
    [SerializeField]
    private GameObject sp1Panel;
    [SerializeField]
    private Image sp1Image;
    [SerializeField]
    private TMP_Text sp1LUcounterTxt;
    [SerializeField]
    private TMP_Text sp1HUcounterTxt;

    [SerializeField]
    private GameObject sp2Panel;
    [SerializeField]
    private Image sp2Image;
    [SerializeField]
    private TMP_Text sp2LUcounterTxt;
    [SerializeField]
    private TMP_Text sp2HUcounterTxt;

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
        if(currentSpeed == 1)
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
            for(int i = 0; i < Mathf.Abs(enemiesReached - 3); ++i)
            {
                winStars[i].sprite = winStarSprite;
            }
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

    public void SetWaveNumber (int current, int maxWaves)
    {
        waveCounterTxt.text = current.ToString() + " / " + maxWaves.ToString();
    }

    public void SetSP1Image(Sprite sprite)
    {
        sp1Image.sprite = sprite;
    }

    public void SetSP1LU (int count)
    {
        sp1LUcounterTxt.text = "x" + count.ToString();
    }

    public void SetSP1HU (int count)
    {
        sp1HUcounterTxt.text = "x" + count.ToString();

    }

    public void SetSP2LU (int count)
    {
        sp2LUcounterTxt.text = "x" + count.ToString();

    }

    public void SetSP2HU (int count)
    {
        sp2HUcounterTxt.text = "x" + count.ToString();

    }

    public void SetSP2Image(Sprite sprite)
    {
        sp2Image.sprite = sprite;
    }
}
