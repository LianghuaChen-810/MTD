using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GUIManager : MonoBehaviour {
	public static GUIManager instance;

	[SerializeField] public Text phaseTxt;
    [SerializeField] private Text moveCounterTxt;

    [Header("Menu Panels")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private Image backGround;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] public GameObject pauseMenu;


    [SerializeField] public Button resumeButton;

    private int score;
    private int moveCounter;

    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
            phaseTxt.text = score.ToString();
        }
    }

    public int MoveCounter
    {
        get
        {
            return moveCounter;
        }
        set
        {
            moveCounter = value;
            moveCounterTxt.text = moveCounter.ToString();
        }
    }

    void Awake() {
		instance = GetComponent<GUIManager>();

        moveCounter = 5;
        moveCounterTxt.text = moveCounter.ToString();
    }

    public void Play()
    {
        SceneManager.LoadScene("Game");
    }

    public void InitiateGame()
    {
        mainMenu.SetActive(false);
        backGround.gameObject.SetActive(false);
        gamePanel.SetActive(true);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());
    }
}
