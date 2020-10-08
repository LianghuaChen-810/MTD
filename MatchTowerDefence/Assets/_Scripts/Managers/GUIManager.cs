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
    [SerializeField] private Image backGround;
    [SerializeField] private Sprite winStarSprite;
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


    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Awake()
    {
        instance = GetComponent<GUIManager>();

        enemiesReached = 0;
        moveCounter = 5;
        moveCounterTxt.text = moveCounter.ToString();

        DontDestroyOnLoad(this);
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        mainMenu.SetActive(false);
        backGround.gameObject.SetActive(false);
        gamePanel.SetActive(true);
        pauseMenu.SetActive(false);
        RestoreSpeed();
        tryAgain = false;
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }   
    
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void InitiateGame()
    {

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

    public void MainMenuInstance()
    {
        mainMenu.SetActive(true);
        backGround.gameObject.SetActive(true);
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
        --currentSpeed;
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

}
