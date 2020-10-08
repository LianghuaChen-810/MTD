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

    public int EnemiesReached { get { return enemiesReached; } set { enemiesReached = value; } }

    public int Score { get { return score; } set { score = value; phaseTxt.text = score.ToString(); } }

    public int MoveCounter  { get { return moveCounter; } set { moveCounter = value; moveCounterTxt.text = moveCounter.ToString(); } }

    private void Awake()
    {
        instance = GetComponent<GUIManager>();

        enemiesReached = 0;
        moveCounter = 5;
        moveCounterTxt.text = moveCounter.ToString();
    }

    public void Play()
    {
        SceneManager.LoadScene("Game");
    }   
    
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void InitiateGame()
    {
        mainMenu.SetActive(false);
        backGround.gameObject.SetActive(false);
        gamePanel.SetActive(true);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(currentLevel);
        levelFinishedMenu.SetActive(false);
    }

    public void MainMenuInstance()
    {
        mainMenu.SetActive(true);
        backGround.gameObject.SetActive(true);
    }

    public void LevelIsFinished()
    {
        SetWinStars();
    }
    
    public void FastForward()
    {
        if(currentSpeed == 1)
        {
            fastForwardSwaps[0].gameObject.SetActive(false);
            fastForwardSwaps[1].gameObject.SetActive(true);
            Time.timeScale = 2;
            ++currentSpeed;
        }
        else
        {
            fastForwardSwaps[1].gameObject.SetActive(false);
            fastForwardSwaps[0].gameObject.SetActive(true);
            Time.timeScale = 1;
            --currentSpeed;
        }
    }

    private void SetWinStars()
    {
        Color star1 = winStars[0].color;
        Color star2 = winStars[1].color;
        Color star3 = winStars[2].color;

        if (enemiesReached >= 3)
        {
            star1 = Color.white;
            star2 = Color.white;
            star3 = Color.white;
            losePanel.SetActive(true);
        }
        else
        {
            switch (enemiesReached)
            {
                case 0:
                    star1 = Color.black;
                    star2 = Color.black;
                    star3 = Color.black;
                    winPanel.SetActive(true);
                    break;
                case 1:
                    star1 = Color.black;
                    star2 = Color.black;
                    star3 = Color.white;
                    winPanel.SetActive(true);
                    break;
                case 2:
                    star1 = Color.black;
                    star2 = Color.white;
                    star3 = Color.white;
                    winPanel.SetActive(true);
                    break;
            }
        }
    }




}
